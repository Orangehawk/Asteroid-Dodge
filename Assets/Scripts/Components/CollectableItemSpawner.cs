
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItemSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject[] itemPrefabs;
	[SerializeField]
	GameObject itemPool;

	[SerializeField]
	Vector3 areaRadius;
	[SerializeField]
	Vector3 safeAreaRadius;

	[SerializeField]
	int totalItemsToSpawn = 1;
	[SerializeField]
	bool spawnAllAtOnce = false;
	[SerializeField]
	float minTimeBetweenSpawns = 10;
	[SerializeField]
	float maxTimeBetweenSpawns = 30;

	float lastSpawnTime;
	int itemsSpawned = 0;


	void Awake()
	{
		if (itemPool == null)
		{
			itemPool = new GameObject("Item Pool");
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		if (spawnAllAtOnce)
		{
			InstansiateItem(totalItemsToSpawn, itemPrefabs, areaRadius, safeAreaRadius);
		}
		else
		{
			SpawnOneItem();
		}
	}

	void SpawnOneItem()
	{
		InstansiateItem(1, itemPrefabs, areaRadius, safeAreaRadius);

		itemsSpawned++;

		Debug.Log($"{Time.time - lastSpawnTime}s since last spawn");
		lastSpawnTime = Time.time;
	}

	void InstansiateItem(int numItems, GameObject[] items, Vector3 areaRadius, Vector3 safeRadius)
	{
		Debug.Log($"Instansiating {numItems} items");

		int skips = 0; //Amount of times an overlap has caused a skip
		int maxSkips = 50; //How many times we can skip instantiating before breaking the loop

		for (int i = 0; i < numItems; i++)
		{
			Vector3 pos;
			Quaternion rot;

			//Find a position that is not within the safe radius
			do
			{
				pos = new Vector3(Random.Range(-areaRadius.x, areaRadius.x), Random.Range(-areaRadius.y, areaRadius.y), Random.Range(-areaRadius.z, areaRadius.z));
			} while ((pos.x > safeRadius.x && pos.x < -safeRadius.x) || (pos.y > safeRadius.y && pos.y < -safeRadius.y) || (pos.z > safeRadius.z && pos.z < -safeRadius.z));

			rot = Random.rotation; //Create a random starting rotation

			if (Physics.OverlapSphere(pos, 1, ~0, QueryTriggerInteraction.Ignore).Length == 0)
			{
				GameObject currAsteroid = Instantiate(items[Random.Range(0, items.Length)], pos, rot, itemPool.transform); //Instantiate 
			}
			else
			{
				//Debug.Log("Found overlap, skipping");
				i--;
				if (skips++ > maxSkips)
				{
					Debug.Log($"Too many skips, aborting item generation. Total items generated: {i}/{numItems}");
					break;
				}
			}

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!spawnAllAtOnce)
		{
			if (itemsSpawned < totalItemsToSpawn)
			{
				float timeSinceLastSpawn = Time.time - lastSpawnTime;

				if (timeSinceLastSpawn >= maxTimeBetweenSpawns)
				{
					Debug.Log("Over max timer, spawning item");
					SpawnOneItem();
				}
				else if (timeSinceLastSpawn >= minTimeBetweenSpawns && timeSinceLastSpawn <= maxTimeBetweenSpawns)
				{
					if (Random.Range(1, 4) > 2)
					{
						Debug.Log("Random success, spawning item");
						SpawnOneItem();
					}
				}
			}
		}
	}
}
