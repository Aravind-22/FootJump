using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] easyObstacle;
    public GameObject[] mediumObstacle;
    public GameObject[] hardObstacle;
    public GameObject[] powerUps;
    public GameObject player;
    public GameObject Timer;
    public GameObject temp;
    public GameObject powerupIndicator;
    public static SpawnManager instance;
    public static int score = 100;
    float spawnRangeX,spawnPosZ=-4.5f;
    float spawnPlayerPosX = -0.25f,spawnPlayerPosY = 3.0f,spawnPlayerPosZ = -4.5f;
    float powerupSpawnX = 0, powerupSpawnY = 1.75f, powerupSpawnZ = -4.5f;
    int spawnXMin, spawnXMax; 
    int easyIndex,easyLength,medLength,medIndex,hardLength,hardIndex;
    int spawnTime = 3,delay = 2;
    int obstacleSpawnMin, obstacleSpawnMax;
    int instantiateMin = 1, instantiateMax = 4;
    Vector3 spawnPlayerPos;
    public Transform target;

    void Start()
    {
        spawnXMin = 4; spawnXMax = 7;
        obstacleSpawnMin = 2; obstacleSpawnMax = 5;
        easyLength = easyObstacle.Length;
        medLength = mediumObstacle.Length;
        hardLength = hardObstacle.Length;
        InvokeRepeating("spawnObstacle", spawnTime, Random.Range(obstacleSpawnMin,obstacleSpawnMax));
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (score == Mathf.RoundToInt(PlayerMovement.score))
        {
            Debug.Log("Scores match");
            spawnPowerUps();
            StartCoroutine(deletePowerUps());
        }
    }
    void spawnObstacle()
    {
        easyIndex = Random.Range(0, easyLength);
        medIndex = Random.Range(0, medLength);
        hardIndex = Random.Range(0, hardLength);
        spawnRangeX = Random.Range(spawnXMin,spawnXMax);
        Vector3 spawnPos = new Vector3(spawnRangeX,transform.position.y,spawnPosZ);
        if (PlayerMovement.score >= 500)
        {
            spawnXMin = 3;  spawnXMax = 5; obstacleSpawnMin = 1; obstacleSpawnMax = 3;
            int instantiateThree = Random.Range(instantiateMin, instantiateMax);
            if (instantiateThree == 1)
                Instantiate(easyObstacle[easyIndex], spawnPos, easyObstacle[easyIndex].transform.rotation);
            if (instantiateThree == 2)
                Instantiate(mediumObstacle[medIndex], spawnPos, mediumObstacle[medIndex].transform.rotation);
            if (instantiateThree == 3)
                Instantiate(hardObstacle[hardIndex], spawnPos, hardObstacle[hardIndex].transform.rotation);
        }
        else if (PlayerMovement.score >= 250)
        {
            spawnXMax = 6; obstacleSpawnMin=1; obstacleSpawnMax = 4;
            int instantiateTwo = Random.Range(instantiateMin, instantiateMax - 1);
            if (instantiateTwo == 1)
                Instantiate(easyObstacle[easyIndex], spawnPos, easyObstacle[easyIndex].transform.rotation);
            if (instantiateTwo == 2)
                Instantiate(mediumObstacle[medIndex], spawnPos, mediumObstacle[medIndex].transform.rotation);
        }
        else
        {
            Instantiate(easyObstacle[easyIndex], spawnPos, easyObstacle[easyIndex].transform.rotation);
        }

    }
    public void spawnPlayer()
    {
        spawnPlayerPos = new Vector3(spawnPlayerPosX, spawnPlayerPosY, spawnPlayerPosZ);
        StartCoroutine(callPlayer());
    }
    void spawnPowerUps()
    {
        Debug.Log("Powerups spawned");
        score += 100;
        int powerUp = Random.Range(1, 3);
        Vector3 spawnPowerUp = new Vector3(powerupSpawnX, powerupSpawnY, powerupSpawnZ);
        if (powerUp == 1)
            Instantiate(powerUps[0],spawnPowerUp, powerUps[0].transform.rotation);
        if (powerUp == 2)
            Instantiate(powerUps[1],spawnPowerUp, powerUps[1].transform.rotation);
        Timer.SetActive(true);
    }
    IEnumerator callPlayer()
    {
        yield return new WaitForSeconds(2);
        temp = Instantiate(player, spawnPlayerPos, player.transform.rotation);
    }
    IEnumerator deletePowerUps()
    {
        yield return new WaitForSeconds(delay);
        Destroy(GameObject.FindGameObjectWithTag("PowerUp"));
        if (PlayerMovement.instance.isPoweredUp == false)
        {
            Timer.SetActive(false);
        }
    }
}
