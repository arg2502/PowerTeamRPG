using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ArmorItem : Item {

    //Attributes
    public enum Type { head, chest, hands, misc};
    public Type type;

	// Use this for initialization
	void Start () {
	
	}

    public void Use(HeroData hero)
    {
        //this method will use the name of the item in a switch
        //to determine the appropriate method to call to perform
        //the item's specific effect

        hero.hpMax += hpChange;
        hero.pmMax += pmChange;
        hero.atk += atkChange;
        hero.def += defChange;
        hero.mgkAtk += mgkAtkChange;
        hero.mgkDef += mgkDefChange;
        hero.luck += luckChange;
        hero.evasion += evadeChange;
        hero.spd += spdChange;

        switch (name)
        {
            case "Helmet of Fortitude":
                break;
            case "Iron Helm":
                break;
            case "Steel Helm":
                break;
            case "Iron Armor":
                break;
            case "Steel Armor":
                break;
            case "Steel Gauntlets":
                break;
            default:
                print(name + " does not have a case!");
                break;
        }

        // find the appropriate object to replace
        if (hero.equipment.Count > 0)
        {
            for (int i = 0; i < hero.equipment.Count; i++)
            {
                if (hero.equipment[i].GetComponent<ArmorItem>().type == type)
                {
                    hero.equipment[i].GetComponent<ArmorItem>().Remove(hero);
                    hero.equipment[i] = this.gameObject;
                }
            }
        }
        else { hero.equipment.Add(this.gameObject); }
        
        // Always decrease quantity by 1 to avoid multiple heroes sharing the same piece of armor
        //quantity--;
        uses++;
        // if quantity hits 0, remove it from the inventory
        //if (quantity <= 0) { GameControl.control.equipment.Remove(this.gameObject); }
    }

    public void Remove(HeroData hero)
    {

        hero.hpMax -= hpChange;
        hero.pmMax -= pmChange;
        hero.atk -= atkChange;
        hero.def -= defChange;
        hero.mgkAtk -= mgkAtkChange;
        hero.mgkDef -= mgkDefChange;
        hero.luck -= luckChange;
        hero.evasion -= evadeChange;
        hero.spd -= spdChange;

        // Does the equip function in reverse, so it subtracts instead of adding
        switch (name)
        {
            case "Helmet of Fortitude":
                break;
            case "Iron Helm":
                break;
            case "Steel Helm":
                break;
            case "Iron Armor":
                break;
            case "Steel Armor":
                break;
            case "Steel Gauntlets":
                break;
            default:
                print(name + " does not have a case!");
                break;
        }

        hero.equipment.Remove(this.gameObject);
        // make it possible to use the item again
        //quantity++;
        uses--;
        // if this item is not in the inventory anymore, add it back in
        if (quantity == 1) { GameControl.control.equipment.Add(this.gameObject); }
    }
}
