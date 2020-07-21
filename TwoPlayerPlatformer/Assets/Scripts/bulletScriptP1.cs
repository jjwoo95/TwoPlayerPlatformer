using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bulletScriptP1 : MonoBehaviour {

    // bullet speed
    private int bulletSpeed = 10;

    // bullet direction
    private Rigidbody2D bulletMove;

    // player one and two
    private GameObject playerOne;
    private GameObject playerTwo;

    // health script for player one and two
    PlayerOne P1ControlScript;
    PlayerTwoHealth P2HealthScript;


    void Start()
    {
        // get rigid body component
        bulletMove = GetComponent<Rigidbody2D>();

        // get script for player one health
        playerOne = GameObject.Find("Player 1");
        P1ControlScript = playerOne.GetComponent<PlayerOne>();

        // get script for player two health
        playerTwo = GameObject.Find("Player 2");
        P2HealthScript = playerTwo.GetComponent<PlayerTwoHealth>();

        // move bullet to the left
        if (P1ControlScript.bulletLeft)
        {
            // bullet moving left
            bulletMove.velocity = new Vector3(-bulletSpeed, 0, 0);
        }

        // move bullet ot the right
        else if (P1ControlScript.bulletRight)
        {
            // bullet moving right
            bulletMove.velocity = new Vector3(bulletSpeed, 0, 0);
        }

        // move bullet diagonally right and up
        else if (P1ControlScript.bulletTopRight)
        {
            // bullet moving right
            bulletMove.velocity = new Vector3(bulletSpeed, bulletSpeed, 0);
        }

        // move bullet diagonally left and up
        else if (P1ControlScript.bulletTopLeft)
        {
            // bullet moving right
            bulletMove.velocity = new Vector3(-bulletSpeed, bulletSpeed, 0);
        }

        // move bullet diagonally right and down
        else if (P1ControlScript.bulletBottomRight)
        {
            // bullet moving right
            bulletMove.velocity = new Vector3(bulletSpeed, -bulletSpeed, 0);
        }

        // move bullet diagonally left and down
        else if (P1ControlScript.bulletBottomLeft)
        {
            // bullet moving right
            bulletMove.velocity = new Vector3(-bulletSpeed, -bulletSpeed, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // get tag of object colliding into
        string name = other.gameObject.tag;

        // check if bullet collided into player two
        if (name == "PlayerTwo")
        {
            // decrease health and destroy bullet
            P2HealthScript.decreaseHealth();
            Destroy(gameObject);

            // check if player two is dead
            if(P2HealthScript.currentHealth <= 0)
            {
                // destroy player two and restart game
                Destroy(other.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // check if bullet collided into a wall
        if (name == "Obstacle")
        {
            // destroy bullet
            Destroy(gameObject);
        }
    }
}







