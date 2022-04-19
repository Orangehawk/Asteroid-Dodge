#define ENABLE_LOGS

//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public enum LogLevel
{
	DEBUG,
	INFO,
	WARNING,
	ERROR,
	CRITICAL
}

public static class Logger// : MonoBehaviour
{
	//public static Logger instance;

	static LogLevel currentLogLevel = LogLevel.DEBUG;

	static bool logToFile = false;


	//void Awake()
	//{
	//	if (instance != null && instance != this)
	//	{
	//		Debug.LogError("Logger instance already exists!");
	//		Destroy(this);
	//	}
	//	else
	//	{
	//		instance = this;
	//	}
	//}

	//void Start()
	//{

	//}

	public static void SetLogLevel(LogLevel logLevel)
	{
		currentLogLevel = logLevel;
	}

	public static LogLevel GetCurrentLogLevel()
	{
		return currentLogLevel;
	}

	public static void SetLogToFile(bool active)
	{
		logToFile = active;
	}

	public static void Log(string input, LogLevel logLevel = LogLevel.DEBUG)
	{
		if (!logToFile)
		{
			if ((int)logLevel >= (int)currentLogLevel)
			{
				switch (logLevel)
				{
					case LogLevel.DEBUG:
						Debug.Log($"DEBUG: {input}");
						break;
					case LogLevel.INFO:
						Debug.Log($"INFO: {input}");
						break;
					case LogLevel.WARNING:
						Debug.LogWarning($"WARNING: {input}");
						break;
					case LogLevel.ERROR:
						Debug.LogError($"ERROR: {input}");
						break;
					case LogLevel.CRITICAL:
						Debug.LogError($"CRITICAL: {input}");
						break;
				}
			}
		}
	}
}
