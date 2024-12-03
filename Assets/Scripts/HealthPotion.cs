using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    
    public float heal = 3f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            other.GetComponent<PlayerHealthManager>().heal(heal);
            Destroy(gameObject);
        }
    }

}
