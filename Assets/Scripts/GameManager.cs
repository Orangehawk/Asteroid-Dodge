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

	public float creditsAmount { get; private set; }
	public float dronesAmount { get; private set; }

	GameState gameState;
	UIManager uiManager;
	bool playerHasItem;
	float boundaryWarningCountdown;
	float boundaryMaxTime = 10f;
	bool boundaryTimerRunning;


	void Awake()
	{
		if (instance != null && instance != this)
		{
			Debug.LogError("GameManager instance already exists!");
			Destroy(this);
		}
		else
		{
			instance = this;
		}

		Cursor.lockState = CursorLockMode.None;
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 60;
	}

	// Start is called before the first frame update
	void Start()
	{
		uiManager = UIManager.instance;
		SetGameState(GameState.Paused);
		uiManager.SetObjectiveText("Find the capsule item!");
		boundaryWarningCountdown = 0;
	}

	public void SetBoundaryCountdown(bool active)
	{
		if(active)
		{
			boundaryWarningCountdown = Time.time;
			boundaryTimerRunning = true;
			uiManager.UpdateBoundaryWarningCountdown(boundaryMaxTime);
			uiManager.SetBoundaryWarning(true);
		}
		else
		{
			//boundaryWarningCountdown = 0;
			boundaryTimerRunning = false;
			uiManager.SetBoundaryWarning(false);
		}
	}

	public void ItemCollected()
	{
		playerHasItem = true;
		uiManager.SetObjectiveText("Return the capsule item to the ship!");
		uiManager.SetItemCollected(true);
	}

	public bool GetItemCollected()
	{
		return playerHasItem;
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
				Debug.Log($"Setting gamestate to {gameState}");
				break;
			case GameState.Paused:
				if (uiManager != null)
					uiManager.SetPauseMenu(true);
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
				Debug.Log($"Setting gamestate to {gameState}");
				break;
			case GameState.Menu:
				if (uiManager != null)
					uiManager.SetPauseMenu(false);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.None;
				Debug.Log($"Setting gamestate to {gameState}");
				break;
			case GameState.GameOver:
				if (uiManager != null)
					uiManager.SetPauseMenu(false);
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
				Debug.Log($"Setting gamestate to {gameState}");
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
				Debug.LogWarning("Tried to pause but game is not playing/paused!");
			}
		}
		else
		{
			Debug.LogWarning("Tried to pause but game is over!");
		}
	}

	public void AddCredits(float amount)
	{
		creditsAmount += amount;
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	void Update()
	{
		if(boundaryTimerRunning)
		{
			float timeRemaining = boundaryMaxTime - (Time.time - boundaryWarningCountdown);

			if (timeRemaining <= 0)
			{
				Debug.Log("Boundary time exceeded");
				PlayerShip.instance.Kill();
			}

			uiManager.UpdateBoundaryWarningCountdown(timeRemaining);
		}
	}
}
