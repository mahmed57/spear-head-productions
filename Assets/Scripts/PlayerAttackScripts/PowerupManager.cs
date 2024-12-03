using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private PlayerAttack player_attack;
    private PlayerMovements player_movements;
    private PlayerHealthManager player_health;
    private float default_heavy_damage;
    private float default_light_damage;
    private float default_attack_range;
    private float default_duration;
    private float default_speed;

    private ROLEnemyDamage player_rol;

    void Start()
    {
        player_attack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        player_movements = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovements>();
        player_health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthManager>();
        player_rol = GameObject.FindGameObjectWithTag("Player").GetComponent<ROLEnemyDamage>();

        default_heavy_damage = player_attack.heavy_attack_damage;
        default_light_damage = player_attack.light_attack_damage;
        default_attack_range = player_attack.attackRange;
        default_duration = player_movements.dashDuration;
        default_speed = player_movements.speed;

    }

    public void apply_powerup(string powerup_name)
    {
        Invoke("apply_" + powerup_name, 0);
    }

    public void remove_powerup(string powerup_name)
    {
        Invoke("remove_" + powerup_name, 0);
    }

    public void apply_BL1()
    {   
        player_attack.light_attack_damage += default_light_damage * 0.03f;
        player_attack.heavy_attack_damage += default_heavy_damage * 0.03f;
        player_attack.default_li_attack_dam += default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam += default_heavy_damage * 0.03f;

    }

    /*** Apply Functions **/
    public void apply_BL2()
    {
        player_attack.light_attack_damage += default_light_damage * 0.03f;
        player_attack.heavy_attack_damage += default_heavy_damage * 0.03f;
        player_attack.default_li_attack_dam += default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam += default_heavy_damage * 0.03f;

    }
    public void apply_BL3()
    {
        player_attack.light_attack_damage += default_light_damage * 0.03f;
        player_attack.heavy_attack_damage += default_heavy_damage * 0.03f;
        player_attack.default_li_attack_dam += default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam += default_heavy_damage * 0.03f;

    }
    public void apply_eg1()
    {
        player_attack.attackRange += default_attack_range * 0.02f;
    }
    public void apply_eg2()
    {
        player_attack.attackRange += default_attack_range * 0.02f;
    }
    public void apply_eg3()
    {
        player_attack.attackRange += default_attack_range * 0.02f;
    }
    public void apply_QS1()
    {
        player_movements.dashSpeed += 2.5f;
        player_movements.dashDuration -=  0.05f;
    }
    public void apply_QS2()
    {
        player_movements.dashSpeed += 2.5f;
        player_movements.dashDuration -=  0.05f;    
    }
    public void apply_QS3()
    {
        player_movements.dashSpeed += 2.5f;
        player_movements.dashDuration -=  0.05f;       
    }
    public void apply_qmove1()
    {
        player_movements.speed += default_speed * 0.02f;
    }
    public void apply_qmove2()
    {
        player_movements.speed += default_speed * 0.02f;
    }
    public void apply_qmove3()
    {
        player_movements.speed += default_speed * 0.02f;
    }
    public void apply_fury()
    {
        
    }
    public void apply_magicblade()
    {
        
    }
    public void apply_tw()
    {
        player_movements.speed -= default_speed * 0.05f;
        player_attack.heavy_attack_damage += default_heavy_damage * 0.05f;
        player_attack.light_attack_damage += default_light_damage * 0.05f;
        player_attack.default_li_attack_dam += default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam += default_heavy_damage * 0.03f;
        
    }
    public void apply_qs_com()
    {
        player_movements.speed += default_speed * 0.05f;
        player_attack.heavy_attack_damage -= default_heavy_damage * 0.05f;
        player_attack.light_attack_damage -= default_light_damage * 0.05f;

    }
    public void apply_rol()
    {
        player_rol.rol_enabled = true;
    }
    public void apply_ams()
    {
        player_attack.amaretsu_enabled = true;
    }
    public void apply_soulshard()
    {
        player_health.max_health += 25;
    }

    /*** Remove Functions ***/

    public void remove_BL1()
    {
        player_attack.light_attack_damage -= default_light_damage * 0.03f;
        player_attack.heavy_attack_damage -= default_heavy_damage * 0.03f;
        player_attack.default_li_attack_dam -= default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam -= default_heavy_damage * 0.03f;
        
    }
    public void remove_BL2()
    {
        player_attack.light_attack_damage -= default_light_damage * 0.03f;
        player_attack.heavy_attack_damage -= default_heavy_damage * 0.03f;
        player_attack.default_li_attack_dam -= default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam -= default_heavy_damage * 0.03f;

    }
    public void remove_BL3()
    {
        player_attack.light_attack_damage -= default_light_damage * 0.03f;
        player_attack.heavy_attack_damage -= default_heavy_damage * 0.03f;
        player_attack.light_attack_damage -= default_light_damage * 0.05f;
        player_attack.default_li_attack_dam -= default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam -= default_heavy_damage * 0.03f;

    }
    public void remove_eg1()
    {
        player_attack.attackRange -= default_attack_range * 0.02f;
    }
    public void remove_eg2()
    {
        player_attack.attackRange -= default_attack_range * 0.02f;        
    }
    public void remove_eg3()
    {
        player_attack.attackRange -= default_attack_range * 0.02f;        
    }
    public void remove_QS1()
    {
        player_movements.dashSpeed -= 2.5f;
        player_movements.dashDuration +=  0.05f;
    }
    public void remove_QS2()
    {
        player_movements.dashSpeed -= 2.5f;
        player_movements.dashDuration +=  0.05f;       
    }
    public void remove_QS3()
    {
        player_movements.dashSpeed -= 2.5f;
        player_movements.dashDuration +=  0.05f;
        
    }
    public void remove_qmove1()
    {
        player_movements.speed -= default_speed * 0.02f;
    }
    public void remove_qmove2()
    {
        player_movements.speed -= default_speed * 0.02f;
    }
    public void remove_qmove3()
    {
        player_movements.speed -= default_speed * 0.02f;
    }
    public void remove_fury()
    {
        
    }
    public void remove_magicblade()
    {
        
    }
    public void remove_tw()
    {
        player_movements.speed += default_speed * 0.05f;
        player_attack.heavy_attack_damage -= default_heavy_damage * 0.05f;
        player_attack.light_attack_damage -= default_light_damage * 0.05f;
        player_attack.default_li_attack_dam -= default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam -= default_heavy_damage * 0.03f;
         
    }
    public void remove_qs_com()
    {
        player_movements.speed -= default_speed * 0.05f;
        player_attack.heavy_attack_damage += default_heavy_damage * 0.05f;
        player_attack.light_attack_damage += default_light_damage * 0.05f;
        player_attack.default_li_attack_dam += default_light_damage * 0.03f;
        player_attack.default_hv_attack_dam += default_heavy_damage * 0.03f;
       
    }

    public void remove_rol()
    {
        player_rol.rol_enabled = false;   
    }

    public void remove_ams()
    {
        player_attack.amaretsu_enabled = false;
    }
    public void remove_soulshard()
    {
        player_health.max_health -= 25;
    }

}
