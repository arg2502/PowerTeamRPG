using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ConsumableItem : Item {
    

	// Use this for initialization
	void Start () {
	
	}

    // out of battle use method
    public void Use(DenigenData target)
    {
        target.hp += hpChange;
        target.pm += pmChange;

        switch (name)
        {
            case "Lesser Restorative":
            case "Restorative":
            case "Gratuitous Restorative":
                if (target.hp > target.hpMax) { target.hp = target.hpMax; }
                break;
            case "Terminal Restorative":
                target.hp = target.hpMax;
                break;
            case "Lesser Elixir":
            case "Elixir":
            case "Gratuitous Elixir":
                if (target.pm > target.pmMax) { target.pm = target.pmMax; }
                break;
            case "Terminal Elixir":
                target.pm = target.pmMax;
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
                    be.name = "HealEffect";
                    if (target.Hp <= (target.HpMax - 20)) { be.GetComponent<Effect>().damage = 20 + "hp"; }
                    else { be.GetComponent<Effect>().damage = (target.HpMax - target.Hp) + "hp"; }
                }
                target.Hp += 20;
                if (target.Hp > target.HpMax) { target.Hp = target.HpMax; }
                break;
            case "Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    if (target.Hp <= (target.HpMax - 40)) { be.GetComponent<Effect>().damage = 40 + "hp"; }
                    else { be.GetComponent<Effect>().damage = (target.HpMax - target.Hp) + "hp"; }
                }
                target.Hp += 40;
                if (target.Hp > target.HpMax) { target.Hp = target.HpMax; }
                break;
            case "Gratuitous Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    if (target.Hp <= (target.HpMax - 60)) { be.GetComponent<Effect>().damage = 60 + "hp"; }
                    else { be.GetComponent<Effect>().damage = (target.HpMax - target.Hp) + "hp"; }
                }
                target.Hp += 60;
                if (target.Hp > target.HpMax) { target.Hp = target.HpMax; }
                break;
            case "Terminal Restorative":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    be.GetComponent<Effect>().damage = (target.HpMax - target.Hp) + "hp";
                }
                target.Hp = target.HpMax;
                break;
            case "Lesser Elixir":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    if (target.Pm <= (target.PmMax - 20)) { be.GetComponent<Effect>().damage = 20 + "pm"; }
                    else { be.GetComponent<Effect>().damage = (target.PmMax - target.Pm) + "pm"; }
                }
                target.Pm += 20;
                if (target.Pm > target.PmMax) { target.Pm = target.PmMax; }
                break;
            case "Elixir":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    if (target.Pm <= (target.PmMax - 40)) { be.GetComponent<Effect>().damage = 40 + "pm"; }
                    else { be.GetComponent<Effect>().damage = (target.PmMax - target.Pm) + "pm"; }
                }
                target.Pm += 40;
                if (target.Pm > target.PmMax) { target.Pm = target.PmMax; }
                break;
            case "Gratuitous Elixir":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    if (target.Pm <= (target.PmMax - 60)) { be.GetComponent<Effect>().damage = 60 + "pm"; }
                    else { be.GetComponent<Effect>().damage = (target.PmMax - target.Pm) + "pm"; }
                }
                target.Pm += 60;
                if (target.Pm > target.PmMax) { target.Pm = target.PmMax; }
                break;
            case "Terminal Elixir":
                // if you are in battle, make a healing effect object
                if (GameObject.FindObjectOfType<BattleMenu>() != null)
                {
                    GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), target.transform.position, Quaternion.identity);
                    be.name = "HealEffect";
                    be.GetComponent<Effect>().damage = (target.PmMax - target.PmMax) + "pm";
                }
                target.Pm = target.PmMax;
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
