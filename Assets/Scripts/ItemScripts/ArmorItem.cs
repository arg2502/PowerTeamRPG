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
    }

    public void Equip()
    {
        // functions similarly to the use method, but this method will add
        // passives or boost stats when called. It may require passing in
        // the denigen who is equipping the item as an argument
    }

    public void Remove()
    {
        // Does the equip function in reverse, so it subtracts instead of
        // adding, still may require arg
    }
}
