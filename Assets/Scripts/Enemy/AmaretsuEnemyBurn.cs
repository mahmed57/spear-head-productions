using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmaretsuEnemyBurn : MonoBehaviour
{
    public float cooldown = 1f;

    public float damage = 0.2f;

    public GameObject burn_particle_effect;
    public void burn()
    {
        if(!burn_particle_effect.activeSelf)
        {
            StartCoroutine(BurnAttack());
        }
    }    

    private IEnumerator BurnAttack()
    {

        burn_particle_effect.SetActive(true);

        give_damage_burn(damage);

        yield return new WaitForSeconds(cooldown);

        give_damage_burn(damage);

        yield return new WaitForSeconds(cooldown);

        give_damage_burn(damage);

        yield return new WaitForSeconds(cooldown);

        give_damage_burn(damage);

        yield return new WaitForSeconds(cooldown);
        
        burn_particle_effect.SetActive(false);

    }

    void give_damage_burn(float damage)
    {
        if(gameObject.tag == "Boss")
        {
            gameObject.GetComponent<BossHealthManager>().deal_damage(damage);
        }
        
        else
        {
            gameObject.GetComponent<EnemyHealthManager>().deal_damage(damage);
        }

    }
}
