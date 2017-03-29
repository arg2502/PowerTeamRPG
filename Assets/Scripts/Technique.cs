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
    protected string name;
    protected string description;
    protected int pm;
    protected int colPos;
    protected int rowPos;
    [NonSerialized]
    List<Technique> listNextTechnique; // tells the skill tree if this technique is part of a chain/branch
    [NonSerialized]
    List<Technique> prerequisites;
    //[NonSerialized]
    //Sprite treeImage; // image to display on the button in the skill tree
    protected int cost; // number of skill points required to unlock
    bool active; // if true, the hero has this technique
    [NonSerialized]
    ButtonSkillTree button;
    protected int damage;
    protected int critical;
    protected int accuracy;
    

    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public int Pm { get { return pm; } set { pm = value; } }
    public int ColPos { get { return colPos; } set { colPos = value; } }
    public int RowPos { get { return rowPos; } set { rowPos = value; } }
    public List<Technique> ListNextTechnique { get { return listNextTechnique; } set { listNextTechnique = value; } }
    public int Cost { get { return cost; } set { cost = value; } }
    public bool Active { get { return active; } set { active = value; } }
    //public Sprite TreeImage { get { return treeImage; } set { treeImage = value; } }    
    public List<Technique> Prerequisites { get { return prerequisites; } set { prerequisites = value; } }
    public ButtonSkillTree Button { get { return button; } set { button = value; } }

    public Technique() {}
    public Technique(string nm, string descrip, int cst, int powerMag, int dmg, int crit, int acc, int cp, int rp)
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
    //// Use this for initialization
    //void Start () {
	
    //}
	
    //// Update is called once per frame
    //void Update () {
	
    //}
}
