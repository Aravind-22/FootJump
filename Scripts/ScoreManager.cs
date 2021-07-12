using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreTextGameOver;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI bestScoreText;
    public float highScore;
    public static ScoreManager instance;
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("Best");
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Mathf.RoundToInt(PlayerMovement.score).ToString();
        scoreTextGameOver.text = scoreText.text;
        if(PlayerMovement.score > highScore)
        {
            highScore = PlayerMovement.score;
            PlayerPrefs.SetFloat("Best", highScore);
        }
        highScore = PlayerPrefs.GetFloat("Best");
        highScoreText.text = Mathf.RoundToInt(highScore).ToString();
        bestScoreText.text = highScoreText.text;
    }
}
