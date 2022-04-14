using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveData
{
	public enum Type
	{
		Object,
		PlayerPref
	}

	public Type type;
	public string id; //Name for Object, Key for PlayerPref
	public Dictionary<string, string> data;

	public SaveData(Type objectType, string objectId, Dictionary<string, string> objectData)
	{
		type = objectType;
		objectId = id;
		objectData = data;
	}
}

public class SaveManager : MonoBehaviour
{
	public static SaveManager instance;

	List<SaveData> saveData;


	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
			Logger.Log("SaveManager instance already exists!", LogLevel.ERROR);
		}
		else
		{
			instance = this;
		}

		saveData = new List<SaveData>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	public void Save()
	{
		var interfaces = FindObjectsOfType<MonoBehaviour>().Where(item => item is ISaveableObject);
		foreach (MonoBehaviour item in interfaces)
		{
			saveData.Add(new SaveData(SaveData.Type.Object, item.name, ((ISaveableObject)item).Save()));
		}

		//interfaces = FindObjectsOfType<MonoBehaviour>().Where(item => item is ISaveablePref);
		//foreach (MonoBehaviour item in interfaces)
		//{
		//	ISaveablePref pref = (ISaveablePref)item;
		//	Dictionary<string, string> d = pref.Save();
		//	PlayerPrefs.SetString(d.Keys);
		//	saveData.Add(new SaveData(SaveData.Type.Object, ((ISaveablePref)item).Save()));
		//}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
