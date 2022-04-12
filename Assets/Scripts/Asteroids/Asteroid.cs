using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	[SerializeField]
	float massMult = 1f;
	[SerializeField]
	public GameObject explosionParticles;

	Rigidbody rb;
	bool exploding = false;


	public void Explode()
	{
		if (!exploding)
		{
			//GameObject g = Instantiate(explosionParticles, gameObject.transform.position, gameObject.transform.rotation);
			//g.transform.localScale = gameObject.transform.localScale / 2;
			//g.GetComponent<ParticleSystem>().Play();
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponent<Collider>().enabled = false;
			exploding = true;
			//Destroy(g, g.GetComponent<ParticleSystem>().main.duration);
			Destroy(gameObject);
		}
	}

	public void UpdateMass()
	{
		if (rb != null)
		{
			float average = gameObject.transform.localScale.x + gameObject.transform.localScale.y + gameObject.transform.localScale.z;
			average = average / 3 * massMult;

			rb.mass = average;
		}
	}

	void Awake()
	{
		//UpdateMass();
	}

	// Start is called before the first frame update
	void Start()
	{
		TryGetComponent<Rigidbody>(out rb);
	}

	private void OnCollisionEnter(Collision collision)
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
}
