using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && !GameManager.instance.GetItemCollected())
		{
			GameManager.instance.SetItemCollected();
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
