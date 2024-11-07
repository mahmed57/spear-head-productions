using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : CharacterHealthManager
{
    public Image healthBarFill;
    public GameObject gameOverScreen;

    public GameObject health_particle_effect;

    private float next_particle_time;
    public float particle_effect_cooldown = 1f;

    void Update()
    {
        healthBarFill.fillAmount = present_health / max_health;

        if(health_particle_effect.activeSelf)
        {
            if(Time.time > next_particle_time)
            {
                health_particle_effect.SetActive(false);
            }
        }

    }

    public override void deal_damage(float damage)
    {
        base.deal_damage(damage);

        next_particle_time =Time.time + particle_effect_cooldown;
        
        health_particle_effect.SetActive(true);

        update_health_bar();
    }

    protected override void handle_death()
    {
    
        Debug.Log("Player has died!");
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;

    }

    private void heal(float heal_amount)
    {
        if(is_over_heal(heal_amount))
        {
            present_health = max_health;
        }
        else
        {
            present_health += heal_amount;
        }
        
        update_health_bar();

    }

    private bool is_over_heal(float heal_amount){
            return (present_health + heal_amount) > max_health;
    }

    private void update_health_bar()
    {
        Debug.Log("Player health: " + present_health);

        healthBarFill.fillAmount = present_health / max_health;
    }    
}
