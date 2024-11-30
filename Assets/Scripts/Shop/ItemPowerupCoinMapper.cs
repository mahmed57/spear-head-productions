using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerupCoinMapper : MonoBehaviour
{   
    public Dictionary<string, List<int>> PowerupItemDictionary;

    void Start()
    {
        set_powerup_item_dic();  
    }

    private void set_powerup_item_dic()
    {
        PowerupItemDictionary = new  Dictionary<string, List<int>>();
        PowerupItemDictionary.Add("BL1", new List<int> {4,15});
        PowerupItemDictionary.Add("BL2", new List<int> {4,15});
        PowerupItemDictionary.Add("BL3", new List<int> {4,15});
        PowerupItemDictionary.Add("eg1", new List<int> {3,15});
        PowerupItemDictionary.Add("eg2", new List<int> {3,15});
        PowerupItemDictionary.Add("eg3", new List<int> {3,15});
        PowerupItemDictionary.Add("QS1", new List<int> {2,15});
        PowerupItemDictionary.Add("QS2", new List<int> {2,15});
        PowerupItemDictionary.Add("QS3", new List<int> {2,15});
        PowerupItemDictionary.Add("qmove1", new List<int> {2,15});
        PowerupItemDictionary.Add("qmove2", new List<int> {2,15});
        PowerupItemDictionary.Add("qmove3", new List<int> {2,15});
        PowerupItemDictionary.Add("fury", new List<int> {10,15});
        PowerupItemDictionary.Add("magicblade", new List<int> {7,15});
        PowerupItemDictionary.Add("tw", new List<int> {5,15});
        PowerupItemDictionary.Add("qs_comp", new List<int> {5,15});
        PowerupItemDictionary.Add("rol", new List<int> {15,15});
        PowerupItemDictionary.Add("ams", new List<int> {8,15});
        PowerupItemDictionary.Add("seeker", new List<int> {10,15});
        PowerupItemDictionary.Add("soulshard", new List<int> {12,15});


    }



}
