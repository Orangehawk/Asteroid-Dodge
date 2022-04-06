using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Debug.Log("Player entered boundary");
			GameManager.instance.SetBoundaryCountdown(false);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player left boundary");
			GameManager.instance.SetBoundaryCountdown(true);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
