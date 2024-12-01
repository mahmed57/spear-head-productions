using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2SetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = transform.position;
    }


}
