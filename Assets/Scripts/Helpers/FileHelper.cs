using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class FileHelper
{
	const string filePath = "Data/";
	const string extension = ".txt";
	const char separator = '|';


	static void CheckFile(string path, string fileName)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		if (!File.Exists(path + fileName))
		{
			File.Create(path + fileName).Close();
		}
	}

	public static void DeleteFile(string fileName)
	{
		if (File.Exists(fileName))
		{
			File.Delete(fileName);
		}
	}

	public static void WriteToFile(string fileName, string data, bool append = false)
	{
		string path = filePath + fileName + extension;
		CheckFile(filePath, fileName + extension);
		StreamWriter sw = new StreamWriter(path, append);
		sw.Write(data + '\n');
		sw.Close();
		AssetDatabase.ImportAsset(path);
	}

	public static string ReadFromFile(string fileName)
	{
		string path = filePath + fileName + extension;

		if (File.Exists(path))
		{
			StreamReader reader = new StreamReader(path);
			return reader.ReadToEnd();
		}

		Logger.Log($"Failed to find file at {path}", LogLevel.WARNING);
		return null;
	}

	public static bool KeyExistsInFile(string fileName, string key)
	{
		string path = filePath + fileName + extension;

		if (File.Exists(path))
		{
			string[] lines = File.ReadAllLines(path);

			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Contains(key))
				{
					return true;
				}
			}
		}

		return false;
	}

	public static void WriteKeyToFile(string fileName, string key, string value)
	{
		string path = filePath + fileName + extension;

		CheckFile(filePath, fileName + extension);
		string[] lines = File.ReadAllLines(path);
		bool keyExists = false;

		for (int i = 0; i < lines.Length; i++)
		{
			if (lines[i].Contains(key))
			{
				lines[i] = key + separator + value;
				keyExists = true;
				Logger.Log($"Found {key} at {path}, setting to {lines[i]}");
			}
		}

		if (keyExists)
		{
			File.WriteAllLines(path, lines);
		}
		else
		{
			Logger.Log($"Failed to find key {key} at {path}, writing new key {key + separator + value + '\n'}");
			File.AppendAllText(path, key + separator + value + '\n');
		}

		AssetDatabase.ImportAsset(path);
	}

	public static string ReadStringFromFile(string fileName, string key)
	{
		string path = filePath + fileName + extension;
		string line;

		if (File.Exists(path))
		{
			StreamReader reader = new StreamReader(path);
			line = reader.ReadLine();

			while (line != null)
			{
				if (line.Contains(key))
				{
					Logger.Log("Found key");
					reader.Close();
					return line.Split(separator)[1];
				}

				line = reader.ReadLine();
			}

			reader.Close();
			Logger.Log($"Failed to find key {key} at {path}", LogLevel.WARNING);
		}

		Logger.Log($"Failed to find file at {path}", LogLevel.WARNING);
		return null;
	}

	public static float ReadFloatFromFile(string fileName, string key)
	{
		string line = ReadStringFromFile(fileName, key);

		if (line != null)
		{
			float result;
			if (float.TryParse(line, out result))
			{
				return result;
			}
			else
			{
				Logger.Log($"Failed to parse key {key} value {line} at {filePath + fileName + extension}", LogLevel.WARNING);
				return -1;
			}
		}
		else
		{
			return -1;
		}
	}
}
