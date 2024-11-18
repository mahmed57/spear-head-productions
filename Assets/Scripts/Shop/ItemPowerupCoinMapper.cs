using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerupCoinMapper : MonoBehaviour
{   
    public Dictionary<string, List<int>> PowerupDictionary;
    public Dictionary<string, List<int>> ItemDictionary;

    void Start()
    {
        set_powerup_dic();
        set_item_dic();        
    }

    private void set_powerup_dic()
    {
        PowerupDictionary = new  Dictionary<string, List<int>>();
        PowerupDictionary.Add("BL1", new List<int> {4,15});
        PowerupDictionary.Add("BL2", new List<int> {4,15});
        PowerupDictionary.Add("BL3", new List<int> {4,15});
        PowerupDictionary.Add("eg1", new List<int> {3,15});
        PowerupDictionary.Add("eg2", new List<int> {3,15});
        PowerupDictionary.Add("eg3", new List<int> {3,15});
        PowerupDictionary.Add("QS1", new List<int> {2,15});
        PowerupDictionary.Add("QS2", new List<int> {2,15});
        PowerupDictionary.Add("QS3", new List<int> {2,15});
        PowerupDictionary.Add("qmove1", new List<int> {2,15});
        PowerupDictionary.Add("qmove2", new List<int> {2,15});
        PowerupDictionary.Add("qmove3", new List<int> {2,15});
        PowerupDictionary.Add("fury", new List<int> {10,15});
        PowerupDictionary.Add("magicblade", new List<int> {7,15});
        PowerupDictionary.Add("tw", new List<int> {5,15});
        PowerupDictionary.Add("qs_comp", new List<int> {5,15});

    }

    private void set_item_dic()
    {
        ItemDictionary = new Dictionary<string, List<int>>();
        ItemDictionary.Add("soulshard", new List<int> {12,15});
    }



}
