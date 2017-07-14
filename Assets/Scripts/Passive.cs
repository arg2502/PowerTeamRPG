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
    public Passive(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp)
    {
        name = nm;
        cost = cst;
        pm = powerMag;
        colPos = cp;
        rowPos = rp;
        damage = dmg;
        critical = crit;
        accuracy = acc;
        description = name + "\n" + descrip + "\nCost: " + cost + "\n\nDMG: " + damage + "\nCRIT: " + critical + "\nACC: " + accuracy;
    }

    public abstract void Start();

    //every passive should override this
    //public abstract int Use() { return 0; }
    public abstract void Use(Denigen attackingDen, Denigen other);
}

// Call this during the CalcDamage method
[Serializable]
public abstract class CalcDamagePassive : Passive {

    public CalcDamagePassive(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp) { }
    //public abstract int Use() { return 0; }
    //public abstract void Use(Denigen attackingDen, Denigen other) { /*return 0;*/ }
}

// Call this during the TakeDamage method
[Serializable]
public abstract class TakeDamagePassive : Passive
{
    public TakeDamagePassive(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp) { }
    //public abstract int Use() { return 0; }
    //public abstract void Use(Denigen attackingDen, Denigen other) { /*return 0;*/ }
}

// call this for every denigen at the end of each turn
[Serializable]
public abstract class PerTurnPassive : Passive
{
    public PerTurnPassive(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp) { }
    //public abstract int Use() { return 0; }
    //public abstract void Use(Denigen attackingDen, Denigen other) { /*return 0;*/ }
}

// this passive just restores 1hp per turn
[Serializable]
public class LightRegeneration : PerTurnPassive {

    public LightRegeneration(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp) { }

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
[Serializable]
public class SiegeBreaker : CalcDamagePassive
{
    public SiegeBreaker(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp) { }

    public override void Start()
    {
                
    }
    public override void Use(Denigen attackingDen, Denigen other)
    {
        
    }
}
[Serializable]
public class Duelist : CalcDamagePassive
{
    public Duelist(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp, int level)
        :base(nm, descrip, cst, powerMag, dmg, crit, acc, cp, rp)
    {
        // the level will be passed in and all calculations can be based on that
        // Duelist I : level = 1
        // Duelist II : level = 2
        // Duelist III : level = 3
    }

    public override void Start()
    {

    }
    public override void Use(Denigen attackingDen, Denigen other)
    {

    }
}
