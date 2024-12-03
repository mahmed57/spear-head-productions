using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class FuryController : MonoBehaviour
{
    
    GameObject amaretsu;

    public bool enable;
    public GameObject shop;

    void Start()
    {
        amaretsu = gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if(shop.GetComponent<PlayerBag>().collectedPowerups.Contains("fury") && enable && !amaretsu.activeSelf)
        {
            amaretsu.SetActive(true);
            
            amaretsu.GetComponent<Fury>().enabled = true;
        
            enable = false;
        }
    }
}
