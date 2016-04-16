using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		Debug.Log("GUI inspected, regenerating map");
		MapGenerator mapGenerator = target as MapGenerator;
		mapGenerator.GenerateMap ();
	}
}
