using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    public int score;
    public GameObject[] Items;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        for (int i = 0; i < 50; i++)
        {
            Instantiate(Items[(int)Random.Range(0, Items.Length)], new Vector3(Random.Range(-25,265), 1, Random.Range(-25, 225)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void increaseScore()
    {
        score++;
    }
}
