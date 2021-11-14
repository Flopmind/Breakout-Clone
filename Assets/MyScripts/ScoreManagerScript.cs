using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ScoreManagerScript : NetworkBehaviour
{

    [SerializeField]
    private GameObject scoreText;
    private int blockCount;

    [SerializeField]
    [SyncVar]
    private int myScore;

    public int Score
    {
        get { return myScore; }
        set
        {
            myScore = value;
            UpdateScoreText();
        }
    }

    void Start()
    {
        // Initialize score and score text.
        Score = 0;
        blockCount = GameObject.FindGameObjectsWithTag("Block").Length;
    }

    void Update()
    {
        Score = Mathf.Abs(blockCount - GameObject.FindGameObjectsWithTag("Block").Length) * 100;
    }

    // Updates the score text to reflect the current score.
    private void UpdateScoreText()
    {
        scoreText.GetComponent<Text>().text = "Score: " + myScore.ToString();
    }
}
