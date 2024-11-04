using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{

    bool is_start = true;

    void Update()
    {
        if(is_start)
        {
            is_start = false;

            transform.position = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerStatistics>().start_pos;

            GetComponent<Collider2D>().enabled = true;

        }
    }
}
