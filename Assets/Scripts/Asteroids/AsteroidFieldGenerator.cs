using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldGenerator : MonoBehaviour
{
	public int numberOfAsteroids;
	public int seed;
	public GameObject[] staticAsteroidPrefabs;
	public GameObject[] dynamicAsteroidPrefabs;

	public Vector3 areaRadius;
	public Vector3 safeAreaRadius;

	public bool dynamic = false;
	public Vector3 initialMinVelocity;
	public Vector3 initialMaxVelocity;

	//public float rotationSpeedMult;
	public float asteroidMinRotationSpeed = 0.5f;
	public float asteroidMaxRotationSpeed = 10f;

	public float asteroidMinSize = 1f;
	public float asteroidMaxSize = 10f;

	public float asteroidMinVelocity = 0f;
	public float asteroidMaxVelocity = 10f;

	public float overlapTolerance = 0f;

	static GameObject asteroidPool;


	 void Awake()
	{
		if (asteroidPool == null)
		{
			asteroidPool = new GameObject("Asteroid Field");
		}
	}

	public void Generate2DAsteroidField(int NumObjects, GameObject[] asteroids, Vector3 areaRadius, Vector3 safeRadius)
	{
		//Logger.Log("Generating 2D asteroid field");

		Vector3 pos;

		for (int i = 0; i < NumObjects; i++)
		{
			do
			{
				pos = new Vector3(Random.Range(-areaRadius.x, areaRadius.x), 1f, Random.Range(-areaRadius.z, areaRadius.z));
			} while ((pos.x > safeRadius.x && pos.x < -safeRadius.x) || (pos.y > safeRadius.z && pos.y < -safeRadius.z));

			Quaternion rot = Random.rotation;
			GameObject currAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length - 1)], pos, rot, asteroidPool.transform);

			Rigidbody rb = currAsteroid.GetComponent<Rigidbody>();
			float rotationSpeedMult = rb.mass;

			float min = asteroidMinRotationSpeed;
			float max = asteroidMaxRotationSpeed;

			if (rotationSpeedMult != 0)
			{
				min *= rotationSpeedMult;
				max *= rotationSpeedMult;
			}

			//if (Random.value > 0.2f)
			//{
			currAsteroid.GetComponent<Rigidbody>().AddTorque(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
			//}
		}
	}

	public void Generate3DAsteroidField(int numObjects, GameObject[] asteroids, Vector3 areaRadius, Vector3 safeRadius)
	{
		Logger.Log("Generating 3D asteroid field");

		int skips = 0; //Amount of times an overlap has caused a skip
		int maxSkips = 50; //How many times we can skip instantiating before breaking the loop

		for (int i = 0; i < numObjects; i++)
		{
			Vector3 pos;
			Quaternion rot;
			float scale;


			//Find a position that is not within the safe radius
			do
			{
				pos = new Vector3(Random.Range(-areaRadius.x, areaRadius.x), Random.Range(-areaRadius.y, areaRadius.y), Random.Range(-areaRadius.z, areaRadius.z));
			} while ((pos.x > safeRadius.x && pos.x < -safeRadius.x) || (pos.y > safeRadius.y && pos.y < -safeRadius.y) || (pos.z > safeRadius.z && pos.z < -safeRadius.z));

			rot = Random.rotation; //Create a random starting rotation

			asteroidMinSize = asteroidMinSize > 0 ? asteroidMinSize : 1; //Clamp min size to 1 or above
			asteroidMaxSize = asteroidMaxSize > 0 ? asteroidMaxSize : 1; //Clamp max size to 1 or above

			scale = Random.Range(asteroidMinSize, asteroidMaxSize); //Create a random scale

			if (Physics.OverlapSphere(pos, scale + overlapTolerance, ~0, QueryTriggerInteraction.Ignore).Length == 0)
			{
				GameObject currAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length)], pos, rot, asteroidPool.transform); //Instantiate 
				currAsteroid.transform.localScale = new Vector3(scale, scale, scale); //Scale the asteroid

				RandomSpin rs = currAsteroid.GetComponent<RandomSpin>();
				float rotationSpeedMult = 0; //Set the rotation speed multiplied based on the rigidbody mass

				float min = asteroidMinRotationSpeed;
				float max = asteroidMaxRotationSpeed;

				if (rotationSpeedMult != 0) //No support for stopping rotation via multiplier
				{
					min *= rotationSpeedMult;
					max *= rotationSpeedMult;
				}

				rs.SetRotationSpeed(Random.Range(min, max));
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

	public void GenerateDynamic3DAsteroidField(int numObjects, GameObject[] asteroids, Vector3 areaRadius, Vector3 safeRadius)
	{
		Logger.Log("Generating dynamic 3D asteroid field");

		int skips = 0; //Amount of times an overlap has caused a skip
		int maxSkips = 50; //How many times we can skip instantiating before breaking the loop

		for (int i = 0; i < numObjects; i++)
		{
			Vector3 pos;
			Quaternion rot;
			float scale;


			//Find a position that is not within the safe radius
			do
			{
				pos = new Vector3(Random.Range(-areaRadius.x, areaRadius.x), Random.Range(-areaRadius.y, areaRadius.y), Random.Range(-areaRadius.z, areaRadius.z));
			} while ((pos.x > safeRadius.x && pos.x < -safeRadius.x) || (pos.y > safeRadius.y && pos.y < -safeRadius.y) || (pos.z > safeRadius.z && pos.z < -safeRadius.z));

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

				//rs.SetRotationSpeed(Random.Range(min, max));

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

		//areaRadius = WorldConstants.WORLD_SIZE;
		//safeAreaRadius = WorldConstants.SAFE_AREA_SIZE_ASTEROIDS;
		if (asteroidPool == null)
			asteroidPool = new GameObject("Asteroids");

		if (areaRadius.magnitude > 0 && safeAreaRadius.magnitude >= 0 && (areaRadius.magnitude > safeAreaRadius.magnitude))
		{
			if (areaRadius.y >= -1 && areaRadius.y <= 1)
			{
				Generate2DAsteroidField(numberOfAsteroids, staticAsteroidPrefabs, areaRadius, safeAreaRadius);
			}
			else
			{
				if(dynamic)
					GenerateDynamic3DAsteroidField(numberOfAsteroids, dynamicAsteroidPrefabs, areaRadius, safeAreaRadius);
				else
					Generate3DAsteroidField(numberOfAsteroids, staticAsteroidPrefabs, areaRadius, safeAreaRadius);
			}
		}
		else
		{
			Debug.LogError("Failed to create asteroid field, invalid values for generation");
		}
	}
}
