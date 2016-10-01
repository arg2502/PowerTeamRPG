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
    int pm;
    Technique next; // tells the skill tree if this technique is part of a chain/branch
    int cost; // number of skill points required to unlock

    public string Name { get { return name; } set { name = value; } }
    public string Description { get { return description; } set { description = value; } }
    public int Pm { get { return pm; } set { pm = value; } }
    public Technique Next { get { return next; } set { next = value; } }
    public int Cost { get { return cost; } set { cost = value; } }
    

    //// Use this for initialization
    //void Start () {
	
    //}
	
    //// Update is called once per frame
    //void Update () {
	
    //}
}
