using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidRingGenerator : MonoBehaviour
{
	[SerializeField]
	int numberOfAsteroids;
	[SerializeField]
	int seed;
	[SerializeField]
	GameObject[] asteroidPrefabs;

	[SerializeField]
	Vector3 initialMinVelocity;
	[SerializeField]
	Vector3 initialMaxVelocity;

	[SerializeField]
	float asteroidMinRotationSpeed = 0.5f;
	[SerializeField]
	float asteroidMaxRotationSpeed = 10f;

	[SerializeField]
	float ringRadius = 1;
	[SerializeField]
	float ringHeight = 1;
	[SerializeField]
	float maxRingThickness;

	[SerializeField]
	float asteroidMinSize = 1f;
	[SerializeField]
	float asteroidMaxSize = 10f;

	[SerializeField]
	float overlapTolerance = 0f;

	[SerializeField]
	bool enableOnValidate = false;

	static GameObject asteroidPool;


	void Awake()
	{
		if (asteroidPool == null)
		{
			asteroidPool = new GameObject("Asteroid Ring");
		}
	}

	private void OnValidate()
	{
		if (enableOnValidate)
		{
			DeleteRing();
			GenerateDynamic3DAsteroidRing(numberOfAsteroids, asteroidPrefabs, ringRadius, ringHeight, maxRingThickness);
		}
	}

	void DeleteRing()
	{
		foreach(Transform child in asteroidPool.transform)
		{
			Destroy(child.gameObject);
		}
	}

	void  GenerateDynamic3DAsteroidRing(int numObjects, GameObject[] asteroids, float radius, float height, float maxThickness)
	{
		Logger.Log("Generating dynamic 3D asteroid ring");

		int skips = 0; //Amount of times an overlap has caused a skip
		int maxSkips = 50; //How many times we can skip instantiating before breaking the loop

		for (int i = 0; i < numObjects; i++)
		{
			Vector3 pos;
			Quaternion rot;
			float scale;

			float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
			pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (radius + Random.Range(0, maxThickness));
			pos.y = Random.Range(-height, height);

			rot = Random.rotation; //Create a random starting rotation

			asteroidMinSize = asteroidMinSize > 0 ? asteroidMinSize : 1; //Clamp min size to 1 or above
			asteroidMaxSize = asteroidMaxSize > 0 ? asteroidMaxSize : 1; //Clamp max size to 1 or above

			scale = Random.Range(asteroidMinSize, asteroidMaxSize); //Create a random scale

			if (Physics.OverlapSphere(pos, scale + overlapTolerance, ~0, QueryTriggerInteraction.Ignore).Length == 0)
			{
				GameObject currAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length)], pos, rot, asteroidPool.transform); //Instantiate 
				currAsteroid.transform.localScale = new Vector3(scale, scale, scale); //Scale the asteroid

				Rigidbody rb = currAsteroid.GetComponent<Rigidbody>();
				float rotationSpeedMult = 0; //Set the rotation speed multiplied based on the rigidbody mass

				float min = asteroidMinRotationSpeed;
				float max = asteroidMaxRotationSpeed;

				if (rotationSpeedMult != 0) //No support for stopping rotation via multiplier
				{
					min *= rotationSpeedMult;
					max *= rotationSpeedMult;
				}

				Vector3 rotationAxis = new Vector3(Random.value, Random.value, Random.value);

				//Ensure all 3 components add up to 1
				float div = rotationAxis.x + rotationAxis.y + rotationAxis.z - 1;
				div /= 3;

				if (div > 0)
				{
					rotationAxis.x -= div;
					rotationAxis.y -= div;
					rotationAxis.z -= div;
				}

				//Spin the asteroid
				rb.AddTorque(rotationAxis * Random.Range(min, max), ForceMode.Impulse);

				//Add velocity to the asteroid
				rb.AddForce(Random.Range(initialMinVelocity.x, initialMaxVelocity.x), Random.Range(initialMinVelocity.y, initialMaxVelocity.y), Random.Range(initialMinVelocity.z, initialMaxVelocity.z), ForceMode.VelocityChange);
			}
			else
			{
				//Logger.Log("Found overlap, skipping");
				i--;
				if (skips++ > maxSkips)
				{
					Logger.Log($"Too many skips, aborting asteroid field generation. Total asteroids generated: {i}/{numObjects}");
					break;
				}
			}

		}
	}

	void Start()
	{
		Random.InitState(seed);

		if (numberOfAsteroids > 0 && ringRadius > 0)
		{
			GenerateDynamic3DAsteroidRing(numberOfAsteroids, asteroidPrefabs, ringRadius, ringHeight, maxRingThickness);
		}
		else
		{
			Debug.LogError("Failed to create asteroid ring, invalid values for generation");
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Home))
		{
			GenerateDynamic3DAsteroidRing(numberOfAsteroids, asteroidPrefabs, ringRadius, ringHeight, maxRingThickness);
		}

		if (Input.GetKeyDown(KeyCode.End))
		{
			DeleteRing();
		}
	}
}
