using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ScriptableConsumable item = ItemDatabase.GetItem("Consumable", "Restorative") as ScriptableConsumable;
		if (item != null) {
			print("Got one: " + item.name + " At the price of: " + item.value + 
			      "\n It restores " + item.statBoosts[0].boost + item.statBoosts[0].statName);
		}
	}	
}
