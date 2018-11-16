using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableItem : ScriptableObject {

	//Override unity's default name variable
	public new string name;

	//Variables universal to all items
	public Sprite sprite;
	public string description;
	//public int quantity;
	//public int uses;
	public int value;
	public Boosts[] statBoosts;
	//There will be little to no functionality in the item classes
	//Instead, Items will act as data containers, with the menus 
	//holding all of the functionality for the items
}
