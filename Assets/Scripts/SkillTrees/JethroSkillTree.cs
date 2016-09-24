using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JethroSkillTree : SkillTree {

    // all possible skills for Jethro
    Skill helmsplitter;
    Skill trinitySlice;
    Skill riser;
    Skill mordstreich;
    Skill arcSlash;
        
    // divided into columns
    List<Technique> col1;
    List<Technique> col2;
    List<Technique> col3;
        

	// Use this for initialization
	void Start () {

        // set hero to jethro
        hero = GameControl.control.heroList[0];

        // create all techniques
        helmsplitter = new Skill();
        helmsplitter.Name = "Helmsplitter";
        helmsplitter.Description = "A powerful sword strike from above. \n Str 75, Crit 03, Acc 90";
        helmsplitter.Pm = 2;

        trinitySlice = new Skill();
        trinitySlice.Name = "Trinity Slice";
        trinitySlice.Description = "";
        trinitySlice.Pm = 0;

        riser = new Skill();
        riser.Name = "Riser";
        riser.Description = "";
        riser.Pm = 0;

        mordstreich = new Skill();
        mordstreich.Name = "Mordstreich";
        mordstreich.Description = "";
        mordstreich.Pm = 0;

        arcSlash = new Skill();
        arcSlash.Name = "Arc Slash";
        arcSlash.Description = "";
        arcSlash.Pm = 0;

        // max num of col
        numOfColumn = 3;

        // max num of row
        numOfRow = 2;

        // set content array
        content2DArray = new List<List<Technique>>();
        content2DArray.Add(new List<Technique>(){helmsplitter, trinitySlice});
        content2DArray.Add(new List<Technique>() { arcSlash });
        content2DArray.Add(new List<Technique>() { riser, mordstreich });

        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
