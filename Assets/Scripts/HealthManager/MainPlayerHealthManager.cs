using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPlayerHealthManager : MonoBehaviour
{
    public Image healthBarFill;
    public float currentHealth = 100f;
    public float maxHealth = 100f;

    public GameObject gameOverScreen;

    // Update is called once per frame
    void Update()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    public void damage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player health: " + currentHealth);

        healthBarFill.fillAmount = currentHealth / maxHealth;

        if (currentHealth == 0)
        {
            death();
        }
    }

    public void heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    void death()
    {
        Debug.Log("Player has died!");
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

}