using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespairHealthManager : MonoBehaviour
{

    public float currentHealth = 30f;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void damage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Despair Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            death();
        }
    }

    private void death()
    {
        Debug.Log("Despair has died!");
        Destroy(gameObject);
    }
}