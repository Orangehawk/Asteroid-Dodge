using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;

	[SerializeField]
	Scene mainMenu;
	[SerializeField]
    Scene[] levels;

	int currentLevel = 0;


	void Awake()
	{
		if (instance != null && instance != this)
		{
			Debug.LogError("LevelManager instance already exists!");
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

	void LoadScene(Scene scene)
	{
		SceneManager.LoadScene(scene.name);
	}

	public void LoadMainMenu()
	{
		LoadScene(mainMenu);
		currentLevel = -1;
	}

	//public void LoadLevel(Scene level)
	//{
	//	LoadScene(level);
	//}

	public void LoadLevel(int level)
	{
		if (level >= 0 && level < levels.Length)
		{
			LoadScene(levels[level]);
			currentLevel = level;
		}
	}

	public void LoadNextLevel()
	{
		if (currentLevel + 1 < levels.Length)
		{
			LoadScene(levels[++currentLevel]);
		}
	}

	public void ReloadLevel()
	{
		LoadScene(SceneManager.GetActiveScene());
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
