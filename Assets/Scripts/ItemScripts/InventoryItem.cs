using System.Collections;
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
    public int Remaining { get { return quantity - uses; } }
	public string type;
    public string subtype;

	public InventoryItem(string _name, int _quantity, string _type, string _subtype = "")
	{
		name = _name;
		quantity = _quantity;
		type = _type;
        subtype = _subtype;
	}

}
