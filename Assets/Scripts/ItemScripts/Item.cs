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

    // variables used to make effects easier to impliment
    public int hpChange;
    public int pmChange;
    public int atkChange;
    public int defChange;
    public int mgkAtkChange;
    public int mgkDefChange;
    public int luckChange;
    public int evadeChange;
    public int spdChange;

	// Use this for initialization
	protected void Start () {
	
	}

    public void Use()
    {
        // items will have there own overrides
    }
}
