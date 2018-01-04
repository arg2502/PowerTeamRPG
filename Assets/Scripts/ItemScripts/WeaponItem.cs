using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class WeaponItem : Item {

    //Attributes
    public enum Type { sword, tome, dagger, staff };

    // Use this for initialization
    void Start()
    {

    }

    public void Use(HeroData hero)
    {
        //this method will use the name of the item in a switch
        //to determine the appropriate method to call to perform
        //the item's specific effect
        if (quantity <= 0 ) return;

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
            case "Spare Sword":
                break;
            case "Tome of Practical Spells":
                break;
            default:
                print(name + " does not have a case!");
                break;
        }
        if (hero.weapon != null) { hero.weapon.GetComponent<WeaponItem>().Remove(hero); }
        hero.weapon = this.gameObject;
        AddHeroAndSortList(hero);

        // Always decrease quantity by 1 to avoid multiple heroes sharing the same sword
        //quantity--;
        uses++;
        // if quantity hits 0, remove it from the inventory
        //if (quantity <= 0) { GameControl.control.weapons.Remove(this.gameObject); }
    }

    public void Remove(HeroData hero)
    {
        // functions similarly to the use method, but this method will remove
        // passives or boost stats when called. It may require passing in
        // the denigen who is equipping the item as an argument

        hero.hpMax -= hpChange;
        hero.pmMax -= pmChange;
        hero.atk -= atkChange;
        hero.def -= defChange;
        hero.mgkAtk -= mgkAtkChange;
        hero.mgkDef -= mgkDefChange;
        hero.luck -= luckChange;
        hero.evasion -= evadeChange;
        hero.spd -= spdChange;

        switch (name)
        {
            case "Spare Sword":
                break;
            case "Tome of Practical Spells":
                break;
            default:
                print(name + " does not have a case!");
                break;
        }

        hero.weapon = null;
        listOfHeroes.Remove(hero);
        // make it possible to use the item again
        //quantity++;
        uses--;
        // if this item is not in the inventory anymore, add it back in
        //if (quantity == 1) { GameControl.control.weapons.Add(this.gameObject); }
    }
}
