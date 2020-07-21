using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    // stamina related variables
    public float maxStamina = 100f;
    public float currentStamina = 0f;
    public GameObject staminaBar;
    public bool fiftyStamina;

    // Use this for initialization
    void Start()
    {
        // set the starting stamina
        currentStamina = maxStamina;

        // set above fifty stamina to true
        fiftyStamina = true;
    }

    // Update is called once per frame 
    void Update()
    {
        // if player has at least fifty stamina set to true
        if (currentStamina >= 50)
            fiftyStamina = true;

    }

    public void decreaseStamina()
    {
        // decrease stamina 
        currentStamina -= 0.5f;

        // calculate and set stamina bar
        float calculateHealth = currentStamina / maxStamina;
        setStaminaBar(calculateHealth);
    }

    public void increaseStamina()
    {
        // increase stamina
        currentStamina += 0.4f;

        // calculate and set stamina bar
        float calculateHealth = currentStamina / maxStamina;
        setStaminaBar(calculateHealth);
    }

    public void setStaminaBar(float myStamina)
    {
        // set at least fifty stamina to false
        if(myStamina <= 0)
        {
            fiftyStamina = false;
        }

        // change stamina for display
        staminaBar.transform.localScale = new Vector3(Mathf.Clamp(myStamina, 0f, 1f), staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
    }
}



