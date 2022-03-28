using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLauncher : MonoBehaviour
{
	[SerializeField]
	int seed = 0;
	[SerializeField]
	public GameObject[] asteroidPrefabs;
	[SerializeField]
	GameObject target;
	[SerializeField]
	float startDistance = 50;
	[SerializeField]
	float startVelocity = 20;
	[SerializeField]
	float maxLaunches = 10;
	[SerializeField]
	float timeBetweenLaunches = 10;

	[SerializeField]
	float asteroidMinRotationSpeed = 20f;
	[SerializeField]
	float asteroidMaxRotationSpeed = 100f;
	[SerializeField]
	float asteroidMinSize = 10f;
	[SerializeField]
	float asteroidMaxSize = 50f;
	[SerializeField]
	float overlapTolerance = 100f;

	static GameObject launchedAsteroidPool;
	int asteroidsLaunched = 0;
	float timeLastLaunched = 0;


	void Awake()
	{
		if (launchedAsteroidPool == null)
		{
			launchedAsteroidPool = new GameObject("Launched Asteroids");
		}
	}

	public bool LaunchAsteroid(float distance, float velocity)
	{
		//int skips = 0; //Amount of times an overlap has caused a skip
		int maxSkips = 50; //How many times we can skip instantiating before breaking the loop

		for (int i = 0; i < maxSkips; i++)
		{
			Vector3 pos;
			Quaternion rot;
			float scale;

			pos = (Random.onUnitSphere * distance) + target.transform.position; //Find a position on a sphere

			rot = Random.rotation; //Create a random starting rotation

			asteroidMinSize = asteroidMinSize > 0 ? asteroidMinSize : 1; //Clamp min size to 1 or above
			asteroidMaxSize = asteroidMaxSize > 0 ? asteroidMaxSize : 1; //Clamp max size to 1 or above

			scale = Random.Range(asteroidMinSize, asteroidMaxSize); //Create a random scale

			if (Physics.OverlapSphere(pos, scale + overlapTolerance, ~0, QueryTriggerInteraction.Ignore).Length == 0)
			{
				GameObject currAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length - 1)], pos, rot, launchedAsteroidPool.transform); //Instantiate 
				currAsteroid.transform.localScale = new Vector3(scale, scale, scale); //Scale the asteroid

				currAsteroid.GetComponent<Asteroid>().UpdateMass(); //Update the rigidbody mass of the asteroid
				Rigidbody rb = currAsteroid.GetComponent<Rigidbody>(); //Get the rigidbody
				float rotationSpeedMult = rb.mass; //Set the rotation speed multiplied based on the rigidbody mass

				float min = asteroidMinRotationSpeed;
				float max = asteroidMaxRotationSpeed;

				if (rotationSpeedMult != 0) //No support for stopping rotation via multiplier
				{
					min *= rotationSpeedMult;
					max *= rotationSpeedMult;
				}

				rb.AddTorque(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max)); //Add a random rotation to the asteroid
				rb.AddForce((target.transform.position - currAsteroid.transform.position).normalized * velocity, ForceMode.VelocityChange);

				//IFF iff = currAsteroid.AddComponent<IFF>();
				//iff.objectType = IFF.ObjectType.Asteroid;
				//iff.hostility = IFF.Hostility.Hostile;

				//UIManager.instance.AddTarget(currAsteroid);
				Debug.Log("Asteroid Launched at distance " + Vector3.Distance(currAsteroid.transform.position, target.transform.position));
				return true;
			}
			else
			{
				Debug.Log($"Found overlap at {pos}, skipping");
				if (i > maxSkips)
				{
					Debug.Log("Too many skips, aborting generation");
					return false;
				}
			}

		}

		return false;
	}

	// Start is called before the first frame update
	void Start()
	{
		if (asteroidPrefabs.Length == 0 || target == null)
		{
			Debug.LogError("AsteroidLauncher: No target set, disabling script");
			this.enabled = false;
		}
		else
		{
			Random.InitState(seed);
			launchedAsteroidPool = new GameObject("Launched Asteroids");
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (asteroidsLaunched < maxLaunches && Time.time >= timeLastLaunched + timeBetweenLaunches)
		{
			Debug.Log("Launching asteroid");

			if (LaunchAsteroid(startDistance, startVelocity))
			{
				timeLastLaunched = Time.time;
				asteroidsLaunched++;
			}
		}
	}
}
