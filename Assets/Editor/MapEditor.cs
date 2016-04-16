using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (DrawDefaultInspector () || GUILayout.Button("Generate Map"))
		{
			base.OnInspectorGUI ();
			Debug.Log("GUI inspected, regenerating map");
			MapGenerator mapGenerator = target as MapGenerator;
			mapGenerator.GenerateMap ();
		}
	}
}
