﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem {
	//this class is a data container
	//it stores the name of an item and the quantity that the player posesses
	//Serialiable for saving purposes
	public string name;
	public int quantity = 1;
	public int uses;
	public string type;

	public InventoryItem(string _name, int _quantity, string _type)
	{
		name = _name;
		quantity = _quantity;
		type = _type;
	}

}
