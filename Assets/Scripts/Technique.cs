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
    [NonSerialized]
    Sprite treeImage; // image to display on the button in the skill tree
    protected int cost; // number of skill points required to unlock
    bool active; // if true, the hero has this technique
    [NonSerialized]
    ButtonSkillTree button;
    protected int damage;
    protected int critical;
    protected int accuracy;
    protected int level;
    public TargetType targetType;
    public Dictionary<Technique, GameObject> treeLinesDictionary = new Dictionary<Technique, GameObject>();

    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public int Pm { get { return pm; } set { pm = value; } }
    public int ColPos { get { return colPos; } set { colPos = value; } }
    public int RowPos { get { return rowPos; } set { rowPos = value; } }
    public List<Technique> ListNextTechnique { get { return listNextTechnique; } set { listNextTechnique = value; } }    
    public int TechPointCost { get { return cost; } set { cost = value; } }
    public bool Active { get { return active; } set { active = value; } }
    public Sprite TreeImage { get { return treeImage; } set { treeImage = value; } }    
    public List<Technique> Prerequisites { get { return prerequisites; } set { prerequisites = value; } }
    public ButtonSkillTree Button { get { return button; } set { button = value; } }
    public int Damage { get { return damage; } }
    public int Critical { get { return critical; } }
    public int Accuaracy { get { return accuracy; } }    

    public Technique() {}
    public Technique(string[] list, Sprite icon = null)
    {
        for (int i = 1; i < list.Length; i++)
        {
            switch (i)
            {
                case 1:
                    name = list[i];
                    break;

                case 2:
                    description = list[i];
                    break;

                case 3:
                    int.TryParse(list[i], out cost);
                    break;

                case 4:
                    int.TryParse(list[i], out pm);
                    break;

                case 5:
                    int.TryParse(list[i], out damage);
                    break;

                case 6:
                    int.TryParse(list[i], out critical);
                    break;

                case 7:
                    int.TryParse(list[i], out accuracy);
                    break;

                case 8:
                    int.TryParse(list[i], out colPos);
                    break;

                case 9:
                    int.TryParse(list[i], out rowPos);
                    break;

                case 10:
                    int.TryParse(list[i], out level);
                    break;

                case 11:
                    int targetInt;
                    int.TryParse(list[i], out targetInt);
                    targetType = (TargetType)targetInt;
                    break;
            }
        }

        if (icon != null)
            treeImage = icon;
    }

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
}
