using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int speed = 2;
    GameManager gameManager;
    public ObstacleType obsType;

    public enum ObstacleType
    {
        Train,
        Wall
    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        if (obsType == ObstacleType.Train)
            speed = 8;
        else if (obsType == ObstacleType.Wall)
            speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.levelFinished && obsType == ObstacleType.Train)
            transform.Translate(Vector3.back * Time.deltaTime * speed);
    }
}
