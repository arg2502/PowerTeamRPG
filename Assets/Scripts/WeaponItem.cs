using UnityEngine;
using System.Collections;

public class WeaponItem : Item {

    //Attributes
    public enum Type { sword, tome, dagger, staff };

    // Use this for initialization
    void Start()
    {

    }

    public void Use()
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
