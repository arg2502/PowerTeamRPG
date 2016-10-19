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

    // counts how many denigens were commanded to use this item, since simply decreasing quantity upon issuing an item's use
    // does not take into account what happens if the denigen dies before using said item, or if victory is acheived before then
    // out of battle it makes sure multiple heroes don't wear the same armor
    public int uses;

    // variables used to make effects easier to impliment
    public string statusChange;
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
