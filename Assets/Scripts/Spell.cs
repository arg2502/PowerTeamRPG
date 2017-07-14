using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class Spell : Technique {
    public Spell() { }
    public Spell(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
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
}
