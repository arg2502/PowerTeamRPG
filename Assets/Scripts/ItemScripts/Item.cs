using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Item : MonoBehaviour {

    //Attributes
    public string name;
    public string description;
    public int quantity = 1;
    public int price;
    public Sprite sprite;

	// Use this for initialization
	protected void Start () {
	
	}

    public void Use()
    {
        // items will have there own overrides
    }
}
