using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{

    public static PlayerHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public float currentHealth, maxHealth;
    
    public Slider healthSlider;

    public GameObject deathEffect;
    
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerStatController.instance.health[0].value;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }*/
    }

    public void TakeDamage(float damageToTake)
    {
        // Check if the player is invincible before taking damage
        if (!playerController.isInvincible)
        {
            currentHealth -= damageToTake;

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                
                LevelManager.instance.EndLVL();
                
                SFXManager.instance.PlaySFXPitched(2);

                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            healthSlider.value = currentHealth;
        }
    }
    
}