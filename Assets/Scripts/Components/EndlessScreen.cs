using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessScreen : MonoBehaviour
{
	[SerializeField]
	GameObject exitPos;

	[SerializeField]
	bool copyX;
	[SerializeField]
	bool copyY;
	[SerializeField]
	bool copyZ;

	// Start is called before the first frame update
	void Start()
    {

    }

	void OnTriggerEnter(Collider other)
	{
		if(!other.CompareTag("Player"))
		{
			//Move the asteroid along one or more axes
			Vector3 pos = other.transform.position;

			if (copyX)
				pos.x = exitPos.transform.position.x;

			if (copyY)
				pos.y = exitPos.transform.position.y;

			if (copyZ)
				pos.z = exitPos.transform.position.z;

			other.transform.position = pos;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
