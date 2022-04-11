using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Target
{
	enum TargetType
	{
		Friendly,
		Neutral,
		Hostile
	}

	public GameObject target;
	public Image waymark;
	public IFF iff;
	public Text variableText;

	public Target(GameObject targetObj, Image marker, IFF iffInfo)
	{
		target = targetObj;
		waymark = marker;
		iff = iffInfo;
		variableText = waymark.GetComponentInChildren<Text>();
	}

	public void SetText(string text)
	{
		variableText.text = text;
	}

	public void Enable()
	{
		waymark.enabled = true;
		variableText.enabled = true;
	}

	public void Disable()
	{
		waymark.enabled = false;
		variableText.enabled = false;
	}
}

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

	[SerializeField]
	GameObject waymarkPool;
	[SerializeField]
	Image waymarkShip;
	[SerializeField]
	Image waymarkDrone;
	[SerializeField]
	Image waymarkAsteroid;
	[SerializeField]
	Image waymarkContainer;
	[SerializeField]
	Image waymarkObject;

	[SerializeField]
	Color colourFriendly;
	[SerializeField]
	Color colourNeutral;
	[SerializeField]
	Color colourHostile;
	[SerializeField]
	Color colourUnknown;

	[SerializeField]
	float waymarkMinSize = 20f;
	[SerializeField]
	float waymarkMaxSize = 60f;
	[SerializeField]
	float waymarkScaleMinDistance = 100f;
	[SerializeField]
	float waymarkScaleMaxDistance = 1000f;
	[SerializeField]
	bool hideWaymarksBehindObjects = true;

	GameManager gameManager;
	GameObject player;
	GameObject mothership;
	PlayerShip playerShip;
	Rigidbody rb;
	List<Target> targets;
	List<GameObject> menus;
	float waymarkScaleSlope;


	private void OnValidate()
	{
		waymarkScaleSlope = (waymarkMinSize - waymarkMaxSize) / (waymarkScaleMaxDistance - waymarkScaleMinDistance);
	}

	void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
			instance = this;

		targets = new List<Target>();
		menus = new List<GameObject>();


		gameManager = GameManager.instance;
		player = PlayerShip.instance.gameObject;
		mothership = Mothership.instance.gameObject;

		if (player != null)
		{
			playerShip = player.GetComponent<PlayerShip>();
			rb = player.GetComponent<Rigidbody>();
		}
		else
		{
			Debug.LogWarning("UIManager: Player is null!");
		}

		if (mothership != null)
		{

		}
		else
		{
			Debug.LogWarning("UIManager: Mothership is null!");
		}

		waymarkScaleSlope = (waymarkMinSize - waymarkMaxSize) / (waymarkScaleMaxDistance - waymarkScaleMinDistance);
	}

	// Start is called before the first frame update
	void Start()
	{
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

	Image GetIFFWaymark(IFF iff)
	{
		switch (iff.objectType)
		{
			case IFF.ObjectType.Ship:
				return waymarkShip;
			case IFF.ObjectType.Drone:
				return waymarkDrone;
			case IFF.ObjectType.Asteroid:
				return waymarkAsteroid;
			case IFF.ObjectType.Container:
				//Debug.LogWarning("No implementation for container in GetIFFWaymark");
				return waymarkContainer;
			default:
				Debug.LogWarning("No object type set, setting to object");
				return waymarkObject;
		}
	}

	Color GetIFFColour(IFF iff)
	{
		switch (iff.hostility)
		{
			case IFF.Hostility.Friendly:
				return colourFriendly;
			case IFF.Hostility.Neutral:
				return colourNeutral;
			case IFF.Hostility.Hostile:
				return colourHostile;
			case IFF.Hostility.Unkown:
				return colourUnknown;
			default:
				Debug.LogWarning("No object hostility set, setting to unknown");
				return colourUnknown;
		}
	}

	Image CreateWaymark(IFF iff)
	{
		Image marker;

		marker = Instantiate(GetIFFWaymark(iff), waymarkPool.transform);
		marker.color = GetIFFColour(iff);

		return marker;
	}

	void KeepFullyOnScreen(GameObject panel)
	{
		RectTransform rect = panel.GetComponent<RectTransform>();
		RectTransform CanvasRect = gameObject.GetComponent<RectTransform>();

		var sizeDelta = CanvasRect.sizeDelta - rect.sizeDelta;
		var panelPivot = rect.pivot;
		var position = rect.anchoredPosition;
		position.x = Mathf.Clamp(position.x, -sizeDelta.x * panelPivot.x, sizeDelta.x * (1 - panelPivot.x));
		position.y = Mathf.Clamp(position.y, -sizeDelta.y * panelPivot.y, sizeDelta.y * (1 - panelPivot.y));
		rect.anchoredPosition = position;
	}

	//Returns true if target is still valid, false if not
	bool UpdateWaymark(Target target, bool showDistance = false)
	{
		if (target.target != null)
		{
			Vector3 vec = Camera.main.WorldToScreenPoint(target.target.transform.position);
			RaycastHit hit;
			int layerMask = ~LayerMask.GetMask("Player", "Ignore Raycast");

			if (hideWaymarksBehindObjects)
			{
				//If target is in front AND either there are no colliders between us and the target OR the collider we hit _is_ the target
				if (vec.z >= 0 && (!Physics.Linecast(player.transform.position, target.target.transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore) || hit.transform.gameObject == target.target))
				{
					vec.z = 0;
					target.Enable();

					target.waymark.rectTransform.position = vec;

					float distance = Vector3.Distance(player.transform.position, target.target.transform.position);
					float newWaypointScale = waymarkMaxSize + waymarkScaleSlope * (distance - waymarkScaleMinDistance);
					newWaypointScale = Mathf.Clamp(newWaypointScale, waymarkMinSize, waymarkMaxSize);
					target.waymark.rectTransform.sizeDelta = new Vector2(newWaypointScale, newWaypointScale);

					if (showDistance)
						target.SetText($"{Mathf.Round(distance)}m");
				}
				else
				{
					target.Disable();
				}
			}
			else if(vec.z >= 0)
			{
				vec.z = 0;
				target.Enable();

				target.waymark.rectTransform.position = vec;

				float distance = Vector3.Distance(player.transform.position, target.target.transform.position);
				float newWaypointScale = waymarkMaxSize + waymarkScaleSlope * (distance - waymarkScaleMinDistance);
				newWaypointScale = Mathf.Clamp(newWaypointScale, waymarkMinSize, waymarkMaxSize);
				target.waymark.rectTransform.sizeDelta = new Vector2(newWaypointScale, newWaypointScale);

				if (showDistance)
					target.SetText($"{Mathf.Round(distance)}m");
			}
			else
			{
				target.Disable();
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	bool InTargetList(GameObject obj)
	{
		foreach (Target t in targets)
		{
			//Target already exists
			if (t.target == obj)
			{
				return true;
			}
		}

		return false;
	}

	public void AddTarget(GameObject target, IFF iff = null)
	{
		if (InTargetList(target))
		{
			return;
		}

		if (iff == null)
		{
			if (!target.TryGetComponent<IFF>(out iff))
			{
				return;
			}
		}

		if (target != null)
		{
			Target t = new Target(target, CreateWaymark(iff), iff);
			UpdateWaymark(t);
			targets.Add(t);
			Debug.Log("Added target");
		}
	}

	Target GetTarget(GameObject target)
	{
		foreach (Target t in targets)
		{
			//Target already exists
			if (t.target == target)
			{
				return t;
			}
		}

		return null;
	}

	void RemoveTarget(Target target)
	{
		foreach (Target t in targets)
		{
			if (t.Equals(target))
			{
				for (int i = 0; i < t.waymark.transform.childCount; i++)
				{
					Destroy(t.waymark.transform.GetChild(i).gameObject);
				}

				Destroy(t.waymark.gameObject);
				targets.Remove(t);
				Debug.Log("Removed target");
				return;
			}
		}

		Debug.Log("Failed to remove target");
	}

	public void RemoveTarget(GameObject target)
	{
		foreach (Target t in targets)
		{
			if (t.target == target)
			{
				for (int i = 0; i < t.waymark.transform.childCount; i++)
				{
					Destroy(t.waymark.transform.GetChild(i).gameObject);
				}

				Destroy(t.waymark.gameObject);
				targets.Remove(t);
				Debug.Log("Removed target");
				return;
			}
		}

		Debug.Log("Failed to remove target");
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

	public void UpdateCredits(float credits)
	{
		creditsText.text = $"${Math.Round(credits)}";
	}

	public void UpdateLives(int lives)
	{
		livesText.text = $"Lives: {lives}";
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

		for (int i = 0; i < targets.Count; i++)
		{
			if (UpdateWaymark(targets[i], true) == false)
			{
				Logger.Log("Lost target, removing", LogLevel.WARNING);
				RemoveTarget(targets[i--]); //Remove target and decrement i
			}
		}
	}
}
