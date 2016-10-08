using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// This script will contain all of the base classes of passives
[Serializable]
public abstract class Passive : Technique {
    //Attributes
    //protected int returnNum; // every passive should return a number
    //protected string name; // every passive should have a name
    //protected string description; // every passive should let the player know what it does

    //public string Name { get { return name; } }
    //public string Description { get { return description; } }

    public abstract void Start();

    //every passive should override this
    //public abstract int Use() { return 0; }
    public abstract void Use(Denigen attackingDen, Denigen other);
}

// Call this during the CalcDamage method
[Serializable]
public abstract class CalcDamagePassive : Passive {

    //public abstract int Use() { return 0; }
    //public abstract void Use(Denigen attackingDen, Denigen other) { /*return 0;*/ }
}

// Call this during the TakeDamage method
[Serializable]
public abstract class TakeDamagePassive : Passive
{

    //public abstract int Use() { return 0; }
    //public abstract void Use(Denigen attackingDen, Denigen other) { /*return 0;*/ }
}

// call this for every denigen at the end of each turn
[Serializable]
public abstract class PerTurnPassive : Passive
{

    //public abstract int Use() { return 0; }
    //public abstract void Use(Denigen attackingDen, Denigen other) { /*return 0;*/ }
}

// this passive just restores 1hp per turn
[Serializable]
public class LightRegeneration : PerTurnPassive {
    
    public override void Start()
    {
        name = "Light Regeneration";
        description = "1hp is restored at the end of every turn.";
    }

    public override void Use(Denigen attackingDen, Denigen other)
    {
        if (attackingDen.hp < attackingDen.hpMax)
        {
            attackingDen.hp += 1;
            GameObject be = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HealEffect"), attackingDen.transform.position, Quaternion.identity);
            be.GetComponent<Effect>().damage = 1 + "hp";
        }
        
    }
}
