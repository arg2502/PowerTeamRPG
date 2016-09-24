using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// This script will contain all of the base classes of passives
[Serializable]
public class Passive : Technique {
    //Attributes
    //protected int returnNum; // every passive should return a number
    protected string name; // every passive should have a name
    protected string description; // every passive should let the player know what it does

    public string Name { get { return name; } }
    public string Description { get { return description; } }

    public virtual void Start() { }

    //every passive should override this
    //public virtual int Use() { return 0; }
    public virtual void Use() { /*return 0;*/ }
}

// Call this during the CalcDamage method
public class CalcDamagePassive : Passive {

    //public virtual int Use() { return 0; }
    public virtual void Use() { /*return 0;*/ }
}

// Call this during the TakeDamage method
public class TakeDamagePassive : Passive {

    //public virtual int Use() { return 0; }
    public virtual void Use() { /*return 0;*/ }
}

// call this for every denigen at the end of each turn
public class PerTurnPassive : Passive {

    //public virtual int Use() { return 0; }
    public virtual void Use() { /*return 0;*/ }
}

public class LightRegeneration : PerTurnPassive {
    
    public override void Start()
    {
        name = "Light Regeneration";
        description = "1hp is restored at the end of every turn.";
    }

    public override void Use()
    {
        // Code goes here
    }
}
