using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class P2Sword : MonoBehaviour
{

    // how long the sword stays out for
    private float swordTime;

    // bullet game object
    public GameObject bullet;

    // players one and two
    private GameObject playerOne;
    private GameObject playerTwo;

    // script for player ones healt
    PlayerOneHealth P1HealthScript;

    // Use this for initialization
    void Start()
    {
        // set time for how long sword exists
        swordTime = Time.time + 0.1f;

        // get player two health script
        playerOne = GameObject.Find("Player 1");
        P1HealthScript = playerOne.GetComponent<PlayerOneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        // count up time to destroy sword
        float currentTime = Time.time;

        // destroy sword when it has existed long enough
        if (currentTime > swordTime)
        {
            // destroy sword
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // get tag of object colliding into
        string name = other.gameObject.tag;

        // check if sword collided into player ones bullet
        if (name == "P1Bullet")
        {
            // refect player ones bullet
            Instantiate(bullet, transform.position, PlayerTwo.bulletRotation);

            // destroy player ones bullet
            Destroy(other.gameObject);
        }

        // check if sword has collided with player one
        if (name == "PlayerOne")
        {
            // decrease player ones health
            P1HealthScript.decreaseHealth();

            // check if player one has died
            if (P1HealthScript.currentHealth <= 0)
            {
                // detroy player one and restart game
                Destroy(other.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
