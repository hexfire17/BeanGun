using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour
{
	public void Awake ()
	{
		_name = gameObject.name;
	}

	public void Debug(string message)
	{
		if (_debug)
		{
			Log ("Debug", message);
		}
	}

	public void Info(string message)
	{
		if (_info) 
		{
			Log ("Info", message);
		}
	}

	private void Log (string logLevel, string message)
	{
		UnityEngine.Debug.Log (_name + " [" + logLevel + "] " + message);
	}

	private string _name;

	public bool _debug;
	public bool _info;
}
