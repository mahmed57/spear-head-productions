using System.Collections;
using UnityEngine;

public class Fury : MonoBehaviour
{
    private float heavy_attack_damage_default;
    private float light_attack_damage_default;
    public PlayerAttack player_attack;

    public float cooldown = 3f;

    private void OnEnable()
    {
        StartCoroutine(BoostAttack());
    }

    private IEnumerator BoostAttack()
    {
        if(player_attack.heavy_attack_damage <= player_attack.default_hv_attack_dam)
        {
            player_attack.heavy_attack_damage += player_attack.default_hv_attack_dam * 0.04f;
            player_attack.light_attack_damage += player_attack.default_li_attack_dam * 0.04f;
        }

        yield return new WaitForSeconds(cooldown);

        player_attack.heavy_attack_damage = player_attack.default_hv_attack_dam;
        player_attack.light_attack_damage = player_attack.default_li_attack_dam;

        this.enabled = false;
        gameObject.SetActive(false);
    }
}
