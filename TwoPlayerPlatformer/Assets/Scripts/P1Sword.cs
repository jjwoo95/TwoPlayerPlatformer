using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class P1Sword : MonoBehaviour {

    // bullet game object
    public GameObject bullet;

    // how long the sword stays out for
    private float swordTime;

    // players one and two
    private GameObject playerOne;
    private GameObject playerTwo;

    // script for player twos health
    PlayerTwoHealth P2HealthScript;

    // Use this for initialization
    void Start () {
        // set time for how long sword exists
        swordTime = Time.time + 0.1f;

        // get player two health script
        playerTwo = GameObject.Find("Player 2");
        P2HealthScript = playerTwo.GetComponent<PlayerTwoHealth>();
    }
	
	// Update is called once per frame
	void Update () {
        // count up time to destroy sword
        float currentTime = Time.time;

        // destroy sword when it has existed long enough
        if(currentTime > swordTime)
        {
            // destroy sword
            Destroy(gameObject);
        } 
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // get tag of object colliding into
        string name = other.gameObject.tag;

        // check if sword collided into player twos bullet
        if (name == "P2Bullet")
        {
            // destroy player twos bullet
            Destroy(other.gameObject);
        }
        
        // check if sword has collided with player two
        if (name == "PlayerTwo")
        {
            // decrease player twos health
            P2HealthScript.decreaseHealth();

            // check if player two has died
            if (P2HealthScript.currentHealth <= 0)
            {
                // detroy player two and restart game
                Destroy(other.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
