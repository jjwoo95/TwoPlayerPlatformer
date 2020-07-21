using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHealth : MonoBehaviour {

    // health related variables
    public float maxHealth = 100f;
    public float currentHealth = 0f;
    public GameObject healthBar;

	// Use this for initialization
	void Start () {
        // set the starting health
        currentHealth = maxHealth;
    }

    public void decreaseHealth()
    {
        // decrease health 
        currentHealth -= 5f;

        // calculate and set health bar
        float calculateHealth = currentHealth / maxHealth;
        SetHealthBar(calculateHealth);
    }

    public void SetHealthBar(float myHealth)
    {
        // change health for display
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(myHealth, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}



