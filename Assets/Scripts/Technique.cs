using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class Technique {

    // This class will hold all of the necessary info to be a technique.
    // Deneigens will have technique lists for their spells and skills
    // This will be used to access the discription of the technique as
    // well as comminicating to the battle menu how much PM a tech takes

    // Attributes
    protected string key;
    protected string name;
    protected string description;
    protected int pm;
    protected int colPos;
    protected int rowPos;
    [NonSerialized]
    List<Technique> listNextTechnique; // tells the skill tree if this technique is part of a chain/branch
    [NonSerialized]
    List<Technique> prerequisites;
    [NonSerialized]
    Sprite treeImage; // image to display on the button in the skill tree
    protected int cost; // number of skill points required to unlock
    bool active; // if true, the hero has this technique
    protected int damage;
    protected int critical;
    protected int accuracy;
    protected int level;
    public TargetType targetType;
    //[NonSerialized]
    //public Dictionary<Technique, GameObject> treeLinesDictionary = new Dictionary<Technique, GameObject>();

    public string Key { get { return key; } }
    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    //public int Pm { get { return pm; } set { pm = value; } }
    public int GetPmCost(Denigen attacker)
    {
        var cost = pm * attacker.GetPmMult();
        if (attacker.StatusState == DenigenData.Status.cursed)
            cost *= 2;
        return (int)cost;
    }
    public int ColPos { get { return colPos; } set { colPos = value; } }
    public int RowPos { get { return rowPos; } set { rowPos = value; } }
    public List<Technique> ListNextTechnique { get { return listNextTechnique; } set { listNextTechnique = value; } }    
    public int TechPointCost { get { return cost; } set { cost = value; } }
    public bool Active { get { return active; } set { active = value; } }
    public Sprite TreeImage { get { return treeImage; } set { treeImage = value; } }    
    public List<Technique> Prerequisites { get { return prerequisites; } set { prerequisites = value; } }
    public int Damage { get { return damage; } }
    public int Critical { get { return critical; } }
    public int Accuaracy { get { return accuracy; } set { accuracy = value; } }

    const int LIST_SIZE = 12;

    public Technique() {}
    public Technique(string[] list, Sprite icon = null)
    {
        if (list.Length < LIST_SIZE)
        {
            Debug.LogError("TECHNIQUE STATS IS TOO SMALL");
            return;
        }

        key = list[0];
        name = list[1];
        description = list[2];
        int.TryParse(list[3], out cost);
        int.TryParse(list[4], out pm);
        int.TryParse(list[5], out damage);
        int.TryParse(list[6], out critical);
        int.TryParse(list[7], out accuracy);
        int.TryParse(list[8], out colPos);
        int.TryParse(list[9], out rowPos);
        int.TryParse(list[10], out level);
        int targetInt;
        int.TryParse(list[11], out targetInt);
        targetType = (TargetType)targetInt;

        if (icon != null)
            treeImage = icon;
    }

	protected bool IsTechniqueIce(string techName){
		switch (techName) {
			case "Frost Edge":
			case "Fog":
			case "Frost":
			case "Frost Bite":
			case "Ice Spear":
			case "Diamond Peak":
				return true;
			default:
				return false;
		}
	}

	protected bool IsTechniqueFire(string techName){
		switch (techName) {
			case "Candleshot":
			case "Fireball":
			case "Grand Fireball":
			case "Splash Flame":
			case "Firewall":
			case "Hellfire":
			case "Cole Fusion":
				return true;
			default:
				return false;
		}
	}
}
