using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldGenerator : MonoBehaviour
{
	public int numberOfAsteroids;
	public int seed;
	public GameObject[] asteroidPrefabs;

	public Vector3 areaRadius;
	public Vector3 safeAreaRadius;

	//public float rotationSpeedMult;
	public float asteroidMinRotationSpeed = 0.5f;
	public float asteroidMaxRotationSpeed = 10f;

	public float asteroidMinSize = 1f;
	public float asteroidMaxSize = 10f;

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
		//Debug.Log("Generating 2D asteroid field");

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

	public void Generate3DAsteroidField(int NumObjects, GameObject[] asteroids, Vector3 areaRadius, Vector3 safeRadius)
	{
		Debug.Log("Generating 3D asteroid field");

		int skips = 0; //Amount of times an overlap has caused a skip
		int maxSkips = 50; //How many times we can skip instantiating before breaking the loop

		for (int i = 0; i < NumObjects; i++)
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

				//currAsteroid.GetComponent<Asteroid>().UpdateMass(); //Update the rigidbody mass of the asteroid
				//Rigidbody rb = currAsteroid.GetComponent<Rigidbody>(); //Get the rigidbody
				RandomSpin rs = currAsteroid.GetComponent<RandomSpin>();
				//float rotationSpeedMult = rb.mass; //Set the rotation speed multiplied based on the rigidbody mass
				float rotationSpeedMult = 0; //Set the rotation speed multiplied based on the rigidbody mass

				float min = asteroidMinRotationSpeed;
				float max = asteroidMaxRotationSpeed;

				if (rotationSpeedMult != 0) //No support for stopping rotation via multiplier
				{
					min *= rotationSpeedMult;
					max *= rotationSpeedMult;
				}

				rs.SetRotationSpeed(Random.Range(min, max));

				//if (Random.value > 0.2f)
				//{
				//currAsteroid.GetComponent<Rigidbody>().AddTorque(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max)); //Add a random rotation to the asteroid
			}
			else
			{
				//Debug.Log("Found overlap, skipping");
				i--;
				if (skips++ > maxSkips)
				{
					Debug.Log("Too many skips, aborting asteroid field generation");
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
				Generate2DAsteroidField(numberOfAsteroids, asteroidPrefabs, areaRadius, safeAreaRadius);
			}
			else
			{
				Generate3DAsteroidField(numberOfAsteroids, asteroidPrefabs, areaRadius, safeAreaRadius);
			}
		}
		else
		{
			Debug.Log("Failed to create asteroid field, invalid values for generation");
		}
	}
}
