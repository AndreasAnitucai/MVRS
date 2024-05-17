using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{

    [SerializeField]
    private GameObject LoseScreen;

    [SerializeField]
    private TMP_Text _scoreText;



    [SerializeField]
    private GameObject ScoreHolder;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score :" + 0;
        LoseScreen.SetActive(false);
    }
    public void LostGame()
    {
        LoseScreen.SetActive(true);
        _scoreText.text = "Score: " + ScoreHolder.GetComponent<Score>().score;
    }
void Update()
    {

    }
}
