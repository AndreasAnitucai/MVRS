using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{

    [SerializeField]
    private GameObject LoseScreen;

    [SerializeField]
    private TMP_Text _scoreText;

    public int score;
    public enum SpawnShape
    {
        Circle,
        Box
    }
    public GameObject[] lootItems = null;
    public int spawnCount = 0;
    public float spawnRadius = 0;
    public SpawnShape spawnShape = SpawnShape.Circle;
    public Vector2 boxSize = new Vector2 (0, 0);
    public Vector3 spawningOffset = new Vector3 (0, 0, 0);
    private int itemAmount;

    public void SpawnLoot()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPoint = FindValidNavMeshSpawnPoint(transform.position, spawnRadius);
            GameObject itemToSpawn = lootItems[Random.Range(0, lootItems.Length)];
            itemAmount++;
            Instantiate(itemToSpawn, randomPoint + spawningOffset,Quaternion.identity);
        }
    }

    public Vector3 FindValidNavMeshSpawnPoint(Vector3 center, float radius)
    {
        Vector3 randomPoint;

        switch(spawnShape)
        {
            case SpawnShape.Circle:
                randomPoint = center +Random.insideUnitSphere * radius;
                break;
            case SpawnShape.Box:
                float halfWidth = boxSize.x * 0.5f;
                float halfHight = boxSize.y * 0.5f;
                randomPoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), 1, Random.Range(-halfHight, halfHight));
                break;
            default:
                randomPoint = center;
                break;
        }

        NavMeshHit hit;

        if(NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            //Debug.Log("Couldn't find valid position, spawning in center");
            return center;
        }
    }

    private void OnDrawGizmosSelected()
    {
        switch (spawnShape)
        {
            case SpawnShape.Circle:
                Gizmos.DrawWireSphere(transform.position, spawnRadius);
                break;
            case SpawnShape.Box:
                Vector3 size = new Vector3(boxSize.x,0, boxSize.y);
                Gizmos.DrawWireCube(transform.position, size);
                break;
            default:
                break;
        }
    }

    void Start()
    {
        score = 0;
        SpawnLoot();
        _scoreText.text = "Score :" + 0;
        LoseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(itemAmount <= 10)
        {
            SpawnLoot();
        }
    }
    public void increaseScore()
    {
        score++;
    }
    public void decreaseLootAmount()
    {
        itemAmount--;
    }
    public void LostGame()
    {
        LoseScreen.SetActive(true);
        _scoreText.text = "Score: " + score;
    }
    private void OnDestroy()
    {
        WriteToFile.AccessPoint.WriteScoreToFile(ReturnScore());
    }
    public int ReturnScore()
    {

        return score;

    }
}
