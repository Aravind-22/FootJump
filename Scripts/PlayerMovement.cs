using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody playerRb;
    public bool isOnGround = true;
    public float jumpForce=2f;
    public float distance;
    public static float score=0;
    float scorePerSecond = 5;
    float playerLimitY = 0.5f;
    float playerScale=1;
    bool isCollision = false;
    public bool isPoweredUp = false;
    Vector3 origScale;
    Vector3 targetDirection;
    public static PlayerMovement instance;
    //public GameObject powerupIndicator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        distance = 0;
        InvokeRepeating("Score", 2, Time.deltaTime);
        instance = this;
     }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Began && Input.GetTouch(0).position.y<Screen.height*0.75f && isOnGround)
        {
            playerRb.AddForce(new Vector3(0.05f,1,0) * jumpForce,ForceMode.Impulse);
            StartCoroutine(MovePlayer());
            isOnGround = false;
        }
        if (transform.position.y<-playerLimitY)
        {
            GameManager.instance.GameOver();
        }
        //Score();
        if (isCollision)
        {
            PowerUp2();
        }
        //targetDirection = transform.position - SpawnManager.instance.target.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {

            if (collision.gameObject.tag == "Ground")
                isOnGround = true;
        }
     }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject != null)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                Debug.Log("Collided");
                GameManager.instance.GameOver();
                Destroy(GameObject.FindGameObjectWithTag("PowerUp"));
                Destroy(gameObject);
                Destroy(other.gameObject);
                SpawnManager.score = 100;
                MoveLeft.score = 250;
            }
            else if (other.gameObject.tag == "PowerUp")
            {
                isPoweredUp = true;
                GameManager.instance.powerUp.Play();
                Destroy(other.gameObject);
                int powerUp = Random.Range(1, 3);
                if (powerUp == 1)
                {
                    PowerUp1();
                    StartCoroutine(deleteTimer1());
                }
                if (powerUp == 2)
                {
                    isCollision = true;
                    PowerUp2();
                    StartCoroutine(deleteTimer1());
                }

            }
        }
    }
    public void Score()
    {
        distance += scorePerSecond * Time.deltaTime;
        score = distance;
    }
    void PowerUp1()
    {
        origScale = transform.localScale;
        transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        jumpForce = 7;
        StartCoroutine(ShrinkPlayer());
    }
    void PowerUp2()
    {
        SpawnManager.instance.powerupIndicator.SetActive(true);
        SpawnManager.instance.powerupIndicator.transform.position = transform.position;
        GameObject[] tempGameObject = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject item in tempGameObject)
        {
                item.GetComponent<Collider>().enabled = false;
                Debug.Log("Box collider trigger disabled");
        }
        StartCoroutine(NoCollisions());
    }
    IEnumerator ShrinkPlayer()
    {
        yield return new WaitForSeconds(5);
        transform.localScale = origScale;
        jumpForce = 6;
    }
    IEnumerator NoCollisions()
    {
        yield return new WaitForSeconds(5);
        SpawnManager.instance.powerupIndicator.SetActive(false);
        isCollision = false;
        Debug.Log("Box collider trigger enabled");
        //GameObject[] tempGameObject;
        //tempGameObject = temp;
        //foreach (GameObject item in tempGameObject)
        //{
        //        item.GetComponent<Collider>().enabled = true;
        //        Debug.Log("Box collider trigger enabled");
        //}
    }
    IEnumerator deleteTimer1()
    {
        yield return new WaitForSeconds(5);
        SpawnManager.instance.Timer.SetActive(false);
        isPoweredUp = false;
    }

    IEnumerator deleteTimer2()
    {
        yield return new WaitForSeconds(5);
        SpawnManager.instance.Timer.SetActive(false);
        isPoweredUp = false;
    }

    IEnumerator MovePlayer()
    {
        yield return new WaitForSeconds(1.7f);
        //Vector3 newDirection = Vector3.RotateTowards(transform.position,targetDirection,5000*Time.deltaTime,0);
        //transform.rotation = Quaternion.LookRotation(newDirection);
        transform.position = Vector3.MoveTowards(transform.position, SpawnManager.instance.target.position, 50*Time.fixedDeltaTime);
    }
}
