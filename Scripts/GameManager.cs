using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject quitConfirmScreenGameOver;
    public GameObject quitConfirmScreen;
    public GameObject gameOverScreen;
    public GameObject score;
    public GameObject homeScreen;
    public GameObject SpawnManager;
    public GameObject Background;
    public GameObject homeScreenButtons;
    public GameObject gameOverScreenButtons;
    public GameObject highScoreScreen;
    public GameObject instructions;
    public SpawnManager Spawn;
    public AudioSource audioSource;
    public AudioSource gameOver;
    public AudioSource crowd;
    public AudioSource powerUp;
    public static GameManager instance;
    public Animator animator;
    public Button ToggleMusic;
    public Sprite Mute;
    public Sprite UnMute;
    bool isRunning = true;
    bool isMuted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        instance = this;
        isMuted = PlayerPrefs.GetInt("Muted") == 1;
        crowd.mute = isMuted;
        if (isMuted)
        {
            ToggleMusic.GetComponent<Image>().sprite = UnMute;
        }
        else
        {
            ToggleMusic.GetComponent<Image>().sprite = Mute;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
            StartCoroutine(crowdNoise());
    }
    public void MuteGame()
    {
        isMuted = !isMuted;
        crowd.mute = isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        if (isMuted)
        {
            ToggleMusic.GetComponent<Image>().sprite = UnMute;
        }
        else
        {
            ToggleMusic.GetComponent<Image>().sprite = Mute;
        }
    }
    public void GameOver()
    {
        gameOver.Play();
        SpawnManager.SetActive(false);
        StartCoroutine(callGameOver());
    }
    public void onQuit()
    {
        quitConfirmScreen.SetActive(true);
        homeScreenButtons.SetActive(false);
    }
    public void onQuitGameOVer()
    {
        quitConfirmScreenGameOver.SetActive(true);
        gameOverScreenButtons.SetActive(false);
        score.SetActive(false);
    }
    public void onQuitYes()
    {
        Application.Quit();
    }
    public void onQuitNo()
    {
        quitConfirmScreen.SetActive(false);
        homeScreenButtons.SetActive(true);
    }
    public void onQuitNoGameOver()
    {
        quitConfirmScreenGameOver.SetActive(false);
        gameOverScreenButtons.SetActive(true);
    }
    public void onPlay()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        animator.SetTrigger("AnimTrig");
        PlayerMovement.score = 0;
        score.SetActive(true);
        //SpawnManager.SetActive(true);
        Background.SetActive(true);
        homeScreen.SetActive(false);
        //Spawn.spawnPlayer();
        audioSource.Play();
        StartCoroutine(gameplay());
        StartCoroutine(crowdNoise());
    }
    public void Instructions()
    {
        homeScreenButtons.SetActive(false);
        instructions.SetActive(true);
    }
    public void Score()
    {
        homeScreenButtons.SetActive(false);
        highScoreScreen.SetActive(true);
    }
    public void Cancel()
    {
        homeScreenButtons.SetActive(true);
        instructions.SetActive(false);
        highScoreScreen.SetActive(false);
    }
    public void Reset()
    {
        Debug.Log("score reset");
        PlayerPrefs.SetFloat("Best", 0);
        ScoreManager.instance.highScore = 0;
        Debug.Log("bestscore reset");
        PlayerMovement.score = 0;
        ScoreManager.instance.highScoreText.text = "0";
        //ScoreManager.instance.bestScoreText.text = "0";
    }
    public void Home()
    {
        SceneManager.LoadScene("Gameplay Scene");
        //gameOverScreen.SetActive(false);
        //homeScreen.SetActive(true);
    }
    IEnumerator crowdNoise()
    {
        isRunning = true;
        yield return new WaitForSeconds(2);
        crowd.Play();
        while (crowd.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(5);
        isRunning = false;
    }
    IEnumerator gameplay()
    {
        yield return new WaitForSeconds(3);
        Destroy(GameObject.FindGameObjectWithTag("Opponent"));
        SpawnManager.SetActive(true);
        Spawn.spawnPlayer();

    }
    IEnumerator callGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        score.SetActive(false);
        Background.SetActive(false);
        crowd.Stop();
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
