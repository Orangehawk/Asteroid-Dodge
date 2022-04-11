using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public enum GameState
	{
		Paused,
		Playing,
		Menu,
		GameOver
	}

	float creditsAmount;

	GameState gameState;
	UIManager uiManager;
	bool playerHasItem;
	

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Logger.Log("GameManager instance already exists!", LogLevel.ERROR);
			Destroy(this);
		}
		else
		{
			instance = this;
		}

		Cursor.lockState = CursorLockMode.None;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

	// Start is called before the first frame update
	void Start()
	{
		uiManager = UIManager.instance;
		SetGameState(GameState.Paused);
		uiManager.SetObjectiveText("Find the capsule item!");
	}

	public static void DebugToggleFrameRate()
	{
		if (Application.targetFrameRate == 60)
		{
			Application.targetFrameRate = 144;
		}
		else
		{
			Application.targetFrameRate = 60;
		}
	}

	public float GetCredits()
	{
		return creditsAmount;
	}

	public void DepositItem()
	{
		playerHasItem = false;
		uiManager.SetObjectiveText("Continue searching for items, or return to the ship to leave");
		uiManager.SetItemCollected(false);
		AddCredits(500);
	}

	public void SetItemCollected()
	{
		playerHasItem = true;
		uiManager.SetObjectiveText("Return the item to the ship!");
		uiManager.SetItemCollected(true);
	}

	public bool GetItemCollected()
	{
		return playerHasItem;
	}

	public void NewGame()
	{
		LevelManager.instance.LoadLevel(0);
		SetCredits(0);
	}

	public void GameWin()
	{
		SetGameState(GameState.GameOver);
		uiManager.GameWin();
	}

	public void GameOver()
	{
		SetGameState(GameState.GameOver);
		uiManager.GameOver();
	}

	public void SetGameState(GameState state)
	{
		gameState = state;

		switch (state)
		{
			case GameState.Playing:
				if (uiManager != null)
					uiManager.SetPauseMenu(false);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
				Logger.Log($"Setting gamestate to {gameState}");
				break;
			case GameState.Paused:
				if (uiManager != null)
					uiManager.SetPauseMenu(true);
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
				Logger.Log($"Setting gamestate to {gameState}");
				break;
			case GameState.Menu:
				if (uiManager != null)
					uiManager.SetPauseMenu(false);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.None;
				Logger.Log($"Setting gamestate to {gameState}");
				break;
			case GameState.GameOver:
				if (uiManager != null)
					uiManager.SetPauseMenu(false);
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
				Logger.Log($"Setting gamestate to {gameState}");
				break;
		}
	}

	public GameState GetGameState()
	{
		return gameState;
	}

	public void TogglePause()
	{
		if (GetGameState() != GameState.GameOver)
		{
			if (gameState == GameState.Playing)
			{
				gameState = GameState.Paused;
				uiManager.SetPauseMenu(true);
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
			}
			else if (gameState == GameState.Paused)
			{
				gameState = GameState.Playing;
				uiManager.SetPauseMenu(false);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				Logger.Log("Tried to pause but game is not playing/paused!", LogLevel.WARNING);
			}
		}
		else
		{
			Logger.Log("Tried to pause but game is over!", LogLevel.WARNING);
		}
	}

	public void SetCredits(float amount)
	{
		creditsAmount = amount;
		uiManager.UpdateCredits(creditsAmount);
	}

	public void AddCredits(float amount)
	{
		creditsAmount += amount;
		uiManager.UpdateCredits(creditsAmount);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	void Update()
	{
		
	}
}
