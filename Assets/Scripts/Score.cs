using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Score : MonoBehaviour
{

    public int score;
    public GameObject[] Items;
    public int numberOfItems = 40; // Number of items to spawn
    public float radius = 10f; // Radius within which to spawn the items
    public float yAxisValue = 1f; // Fixed Y-axis value
    public int maxRetries = 30; // Maximum number of retries to find a valid position
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        SpawnItems();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void increaseScore()
    {
        score++;
    }


    void SpawnItems()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            bool validPosition = false;

            for (int j = 0; j < maxRetries; j++)
            {
                spawnPosition = GetRandomPositionOnNavMesh();
                if (spawnPosition != Vector3.zero)
                {
                    validPosition = true;
                    break;
                }
            }

            if (validPosition)
            {
                // Set the Y-axis value
                spawnPosition.y = yAxisValue;

                // Instantiate a random item from the array
                GameObject itemToSpawn = Items[Random.Range(0, Items.Length)];
                Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid NavMesh position after maximum retries");
            }
        }
    }

    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
