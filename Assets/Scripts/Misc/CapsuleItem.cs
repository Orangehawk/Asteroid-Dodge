using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			GameManager.instance.ItemCollected();
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
