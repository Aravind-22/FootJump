using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    float moveObstacle = 2.5f;
    float obstacleLimit = 10;
    float bgLimit = 45.5f;
    public static int score;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        score = 250;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag=="Obstacle")
        {
            if (Mathf.RoundToInt(PlayerMovement.score)>score)
            {
                moveObstacle += 0.5f;
                score += 250;
            }
            transform.Translate(Vector3.left * Time.deltaTime * moveObstacle);
            if (gameObject.transform.position.x < -obstacleLimit)
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            if (transform.position.x < -bgLimit)
            {
                transform.position = startPos;
            }
        }
    }
}
