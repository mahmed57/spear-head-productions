using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{

    public HashSet<string> collectedPowerups;
    public HashSet<string> collectedItems;

    void Start()
    {
        collectedItems = new HashSet<string>();
        collectedPowerups = new HashSet<string>();
    }

}
