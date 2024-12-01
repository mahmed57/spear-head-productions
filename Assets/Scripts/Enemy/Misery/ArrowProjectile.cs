using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{

    public float damage;
    
    public GameObject player;

    PlayerHealthManager playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealthManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {       

            if (collision.tag == "Player"){

                    playerHealth.deal_damage(damage);
                    
            }

            if ((collision.tag != "Despair") && (collision.tag != "RageTanker") && (collision.tag != "Misery") && (collision.tag != "Room") && (collision.tag != "Boss"))
            {
                Debug.Log(collision.tag);
                
                Destroy(gameObject);
            
            }
    } 

}
