using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
	UIManager uiManager;

	float boundaryWarningCountdown;
	float boundaryMaxTime = 10f;
	bool boundaryTimerRunning;


	// Start is called before the first frame update
	void Start()
    {
		uiManager = UIManager.instance;
		boundaryWarningCountdown = 0;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Debug.Log("Player entered boundary");
			SetBoundaryCountdown(false);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player left boundary");
			SetBoundaryCountdown(true);
		}
	}

	public void SetBoundaryCountdown(bool active)
	{
		if (active)
		{
			boundaryWarningCountdown = Time.time;
			boundaryTimerRunning = true;
			uiManager.UpdateBoundaryWarningCountdown(boundaryMaxTime);
			uiManager.SetBoundaryWarning(true);
		}
		else
		{
			boundaryTimerRunning = false;
			uiManager.SetBoundaryWarning(false);
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (boundaryTimerRunning)
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
