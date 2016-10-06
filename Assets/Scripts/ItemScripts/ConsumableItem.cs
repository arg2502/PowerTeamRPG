﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ConsumableItem : Item {
    // counts how many denigens were commanded to use this item, since simply decreasing quantity upon issuing an item's use
    // does not take into account what happens if the denigen dies before using said item, or if victory is acheived before then
    public int uses;

	// Use this for initialization
	void Start () {
	
	}

    public void Use(Denigen target)
    {
        //this method will use the name of the item in a switch
        //to determine the appropriate method to call to perform
        //the item's specific effect
        switch (name)
        {
            case "Lesser Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    if (target.hp <= (target.hpMax - 20)) { be.GetComponent<Effect>().damage = 20; }
                    else { be.GetComponent<Effect>().damage = (target.hpMax - target.hp); }
                }
                target.hp += 20;
                if (target.hp > target.hpMax) { target.hp = target.hpMax; }
                break;
            case "Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    if (target.hp <= (target.hpMax - 40)) { be.GetComponent<Effect>().damage = 40; }
                    else { be.GetComponent<Effect>().damage = (target.hpMax - target.hp); }
                }
                target.hp += 40;
                if (target.hp > target.hpMax) { target.hp = target.hpMax; }
                break;
            case "Gratuitous Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    if (target.hp <= (target.hpMax - 60)) { be.GetComponent<Effect>().damage = 60; }
                    else { be.GetComponent<Effect>().damage = (target.hpMax - target.hp); }
                }
                target.hp += 60;
                if (target.hp > target.hpMax) { target.hp = target.hpMax; }
                break;
            case "Terminal Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.GetComponent<Effect>().damage = (target.hpMax - target.hp);
                }
                target.hp = target.hpMax;
                break;
            default:
                print(name + " does not have a case!");
                break;
        }

        // Always decrease quantity by 1 since this is the consumable item class
        quantity--;
        // if quantity hits 0, remove it from the inventory
        if (quantity <= 0) { GameControl.control.consumables.Remove(this.gameObject); }
    }
}
