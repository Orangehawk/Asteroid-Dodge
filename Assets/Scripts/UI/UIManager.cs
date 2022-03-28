using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	static public UIManager instance;

	[SerializeField]
	Text speedText;
	[SerializeField]
	Text creditsText;
	[SerializeField]
	Text livesText;
	[SerializeField]
	Text objectiveText;
	[SerializeField]
	Image capsuleCollectedImage;
	[SerializeField]
	Text boundaryWarningCountdown;


	[SerializeField]
	GameObject playerUI;
	[SerializeField]
	GameObject darkOverlay;
	[SerializeField]
	GameObject pauseMenu;
	[SerializeField]
	GameObject boundaryWarning;

	[SerializeField]
	GameObject gameOverMenu;
	[SerializeField]
	GameObject gameWinMenu;

	GameManager gameManager;
	GameObject player;
	GameObject mothership;
	PlayerShip playerShip;
	Rigidbody rb;
	List<GameObject> menus;

	void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
			instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		gameManager = GameManager.instance;
		player = PlayerShip.instance.gameObject;
		mothership = Mothership.instance.gameObject;

		if (player != null)
		{
			playerShip = player.GetComponent<PlayerShip>();
			rb = player.GetComponent<Rigidbody>();
		}

		if (mothership != null)
		{

		}


		menus = new List<GameObject>();
		Transform menuParent = pauseMenu.transform.parent;
		for (int i = 0; i < menuParent.childCount; i++)
		{
			GameObject child = menuParent.GetChild(i).gameObject;
			if (child != darkOverlay && child != pauseMenu)
			{
				menus.Add(child);
			}
		}
	}

	public void SetObjectiveText(string text)
	{
		objectiveText.text = $"Objective: {text}";
	}

	public void SetItemCollected(bool collected)
	{
		capsuleCollectedImage.enabled = collected;
	}

	public void SetPauseMenu(bool enable)
	{
		if (enable)
		{
			playerUI.SetActive(false);
			darkOverlay.SetActive(true);
			pauseMenu.SetActive(true);
		}
		else
		{
			playerUI.SetActive(true);
			darkOverlay.SetActive(false);
			pauseMenu.SetActive(false);
		}
	}

	public void SetBoundaryWarning(bool enable)
	{
		boundaryWarning.SetActive(enable);
	}

	public void UpdateBoundaryWarningCountdown(float countdown)
	{
		boundaryWarningCountdown.text = $"Return to the mission area: {countdown}";
	}

	public void GameWin()
	{
		playerUI.SetActive(false);
		gameWinMenu.SetActive(true);
	}

	public void GameOver()
	{
		playerUI.SetActive(false);
		gameOverMenu.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{
		speedText.text = Math.Round(rb.velocity.magnitude, 2).ToString() + " m/s";
		creditsText.text = $"${Math.Round(gameManager.creditsAmount)}";
	}
}
