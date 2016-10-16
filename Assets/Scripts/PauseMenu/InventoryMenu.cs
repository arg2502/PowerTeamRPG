using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryMenu : Menu {

    public List<Item> itemList;

	// Use this for initialization
	void Start () {

        // set the correct list of items
        if (GameControl.control.whichInventory == "consumables")
        {
            foreach (GameObject go in GameControl.control.consumables) { itemList.Add(go.GetComponent<ConsumableItem>());}
        }
        else if (GameControl.control.whichInventory == "reusables")
        {
            foreach (GameObject go in GameControl.control.reusables) { itemList.Add(go.GetComponent<ReusableItem>()); }
        }
        else if (GameControl.control.whichInventory == "weapons")
        {
            foreach (GameObject go in GameControl.control.weapons) { itemList.Add(go.GetComponent<WeaponItem>()); }
        }
        else if (GameControl.control.whichInventory == "armor")
        {
            foreach (GameObject go in GameControl.control.equipment) { itemList.Add(go.GetComponent<ArmorItem>()); }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
