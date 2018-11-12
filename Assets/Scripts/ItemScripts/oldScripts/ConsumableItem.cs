using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ConsumableItem : Item {

    // keep track of how many other heroes plan on using this item in battle
    // Ex: 
    // * You have 3 Restoratives
    // * Cole selects Restorative in battle
    // * inUse++;
    // * Jethro wants to use Restorative
    // * You didn't use Cole's Restorative so you technically still have 3,
    // * But we want to show (3 - inUse), so Jethro only sees 2
    // This is purely for battle target phase, so it will reset to 0 when the item is used
    public int inUse; 
    public bool Available
    {
        get
        {
            return
                (quantity - inUse > 0);
        }
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
                target.SetHPHealingValue(20);
                break;
            case "Restorative":
                target.SetHPHealingValue(40);
                break;
            case "Gratuitous Restorative":
                target.SetHPHealingValue(60);
                break;
            case "Terminal Restorative":
                var totalHeal = target.HpMax - target.Hp;
                target.SetHPHealingValue(totalHeal);
                break;

            case "Lesser Elixir":
                target.SetPMHealingValue(20);
                break;
            case "Elixir":
                target.SetPMHealingValue(40);
                break;
            case "Gratuitous Elixir":
                target.SetPMHealingValue(60);
                break;
            case "Terminal Elixir":
                var totalPMHeal = target.PmMax - target.Pm;
                target.SetHPHealingValue(totalPMHeal);
                break;

            default:
                print(name + " does not have a case!");
                break;
        }

        // Always decrease quantity by 1 since this is the consumable item class
        quantity--;
        print("new quantity: " + quantity);
        // if quantity hits 0, remove it from the inventory
        if (quantity <= 0) { GameControl.control.consumables.Remove(this.gameObject); }

        // reset inUse
        inUse = 0;
    }
}
