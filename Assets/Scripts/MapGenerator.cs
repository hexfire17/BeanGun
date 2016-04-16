using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
	
	public void Start()
	{
		GenerateMap();	
	}
	
	public void GenerateMap()
	{
		_currentMap = _maps [_mapIndex];

		Debug.Log("Generating map with seed: " + _currentMap._seed);
		string holderName = "Generated_Map";
		Point playerSpawn = new Point((int)_currentMap._size._x/2, (int)_currentMap._size._y/2);
		
		if (transform.FindChild (holderName))
		{
			Debug.Log("MapFound: " + transform.FindChild (holderName).gameObject.name);
			DestroyImmediate(transform.FindChild(holderName).gameObject);	
		}
		
		Transform mapHolder = new GameObject(holderName).transform;
		mapHolder.parent = transform;
		
		_points = new List<Point> ();
		float decimalOutlinePercent = _outlinePercent;
		decimalOutlinePercent = decimalOutlinePercent / 100;
		for (int x = 0; x < _currentMap._size._x; x++)
		{
			for (int y = 0; y < _currentMap._size._y; y++)
			{
				Point point = new Point (x, y);
				_points.Add (point);
				Vector3 tilePosition = point.toVector3 (_currentMap._size, _tileSize);
				Transform newTile = Instantiate(_tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90)) as Transform;
				newTile.localScale = Vector3.one * (1- decimalOutlinePercent) * _tileSize;
				newTile.parent = mapHolder;
			}
		}
		
		bool [,] obsticleMap = new bool [(int)_currentMap._size._x, (int)_currentMap._size._y];
		_shuffledPoints = new Queue<Point> (Utility.FisherShuffle(_points.ToArray (), _currentMap._seed));

		int totalTiles = (int) (_currentMap._size._x * _currentMap._size._y);
		int obsticleTiles = (int) (totalTiles * (_currentMap._obsticlePercent / 100f));
		int currentObsticleCount = 0;
		for (int i = 0; i < obsticleTiles; i++)
		{
			Point randomPoint = getRandPoint ();
			obsticleMap[randomPoint._x, randomPoint._y] = true;
			currentObsticleCount++;
			if (!randomPoint.Equals(playerSpawn) && MapIsFullyAccessible(obsticleMap, currentObsticleCount))
			{
				Vector3 position = randomPoint.toVector3(_currentMap._size, _tileSize);
				Transform obsticle = Instantiate(_obsticlePrefab, (position + (Vector3.up * .5f * _tileSize * (1-decimalOutlinePercent))), Quaternion.identity) as Transform;
				obsticle.localScale = Vector3.one * (1-decimalOutlinePercent) * _tileSize;
				obsticle.parent = mapHolder;
			}
			else
			{
				obsticleMap[randomPoint._x, randomPoint._y] = false;
				currentObsticleCount--;
			}
		}
		
		Transform maskLeft = Instantiate(_navMeshMaskPrefab, Vector3.left * (_currentMap._size._x + _maxMapSize.x) / 4 * _tileSize, Quaternion.identity) as Transform;
		maskLeft.localScale = new Vector3((_maxMapSize.x - _currentMap._size._x) / 2, 1, _currentMap._size._y) * _tileSize;
		
		Transform maskRight = Instantiate(_navMeshMaskPrefab, Vector3.right * (_currentMap._size._x + _maxMapSize.x) / 4 * _tileSize, Quaternion.identity) as Transform;
		maskRight.localScale = new Vector3((_maxMapSize.x - _currentMap._size._x) / 2, 1, _currentMap._size._y) * _tileSize;
		
		Transform maskTop = Instantiate(_navMeshMaskPrefab, Vector3.forward * (_currentMap._size._y + _maxMapSize.y) / 4 * _tileSize, Quaternion.identity) as Transform;
		maskTop.localScale = new Vector3(_maxMapSize.x, 1, (_maxMapSize.y - _currentMap._size._y) / 2) * _tileSize;

		Transform maskBottom = Instantiate(_navMeshMaskPrefab, Vector3.back * (_currentMap._size._y + _maxMapSize.y) / 4 * _tileSize, Quaternion.identity) as Transform;
		maskBottom.localScale = new Vector3(_maxMapSize.x, 1, (_maxMapSize.y - _currentMap._size._y) / 2) * _tileSize;

		maskLeft.parent = mapHolder;
		maskRight.parent = mapHolder;
		maskBottom.parent = mapHolder;
		maskTop.parent = mapHolder;

		_navmeshFloor.localScale = new Vector3(_maxMapSize.x, _maxMapSize.y) * _tileSize;
	}
	
	bool MapIsFullyAccessible(bool [,] obsticleMap, int numObsticles)
	{
		int currFloodCount = numObsticles;
		Point center = _currentMap._mapCenter;
		bool [,] floodMap = coppyArr (obsticleMap);
		
		Queue<Point> pointsToVisit = new Queue<Point> ();
		pointsToVisit.Enqueue(center);
		floodMap[center._x, center._y] = true;
		currFloodCount++;
		int iterations = 0;
		while (pointsToVisit.Count > 0)
		{
			iterations++;
			Point point = pointsToVisit.Dequeue ();
				
			// radius of 1 search, no diag
			List<Point> neighbors = new List<Point> ();
			neighbors.Add (new Point(point._x, point._y-1));
			neighbors.Add (new Point(point._x, point._y+1));
			neighbors.Add (new Point(point._x-1, point._y));
			neighbors.Add (new Point(point._x+1, point._y));
			foreach (Point currPoint in neighbors)
			{
				if (currPoint._y < _currentMap._size._y &&  currPoint._x < _currentMap._size._x &&
				    currPoint._y >= 0 && currPoint._x >= 0 &&
				    floodMap[currPoint._x,currPoint._y] == false)
				{
					pointsToVisit.Enqueue(currPoint);
					floodMap[currPoint._x, currPoint._y] = true;
					currFloodCount++;
				}
			}
		}
		
		Debug.Log("Iterations: " + iterations + ", " + currFloodCount);
		return (currFloodCount == (obsticleMap.GetLength(0) * obsticleMap.GetLength(1)));
	}
	
	T [,] coppyArr<T>(T [,] arr)
	{
		T [,] newArr = new T[arr.GetLength(0), arr.GetLength(1)];
		for (int x = 0; x < arr.GetLength(0); x++)
		{
			for (int y = 0; y < arr.GetLength(1); y++)
			{
				newArr[x, y] = arr[x, y];
			}
		}
		return newArr;
	}
	
	public Point getRandPoint()
	{
		Point point = _shuffledPoints.Dequeue();	
		_shuffledPoints.Enqueue (point);
		return point;
	}
	
	[System.Serializable]
	public struct Point
	{
		public Point(int x, int y)
		{
			_x = x;
			_y = y;
		}
		
		public Vector3 toVector3 (Point mapSize, float tileSize)
		{
			return new Vector3(-mapSize._x/2 + 0.5f + _x, 0, -mapSize._y/2 + 0.5f + _y) * tileSize;
		}
		
		public bool Equals(Point other)
		{
			return (_x == other._x) && (_y == other._y);	
		}
		
		public int _x;
		public int _y;
	}

	[System.Serializable]
	public class Map
	{
		public Point _size;
		[Range(0,100)]
		public int _obsticlePercent;
		public int _seed;
		public float _minObsticleHeight;
		public float _maxObsticleHeight;
		public Color _foreGroundColor;
		public Color _backGroundColor;

		public Point _mapCenter
		{
			get { return new Point (_size._x / 2, _size._y / 2); }
		}
	}

	public Map[] _maps;
	public int _mapIndex;
	Map _currentMap;

	public Transform _tilePrefab;
	public Transform _obsticlePrefab;
	
	[Range(0,100)]
	public int _outlinePercent;
	public float _tileSize;
	
	List<Point> _points;
	Queue<Point> _shuffledPoints;

	public Transform _navmeshFloor;
	public Vector2 _maxMapSize;
	
	public Transform _navMeshMaskPrefab;
}
