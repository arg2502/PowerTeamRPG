using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JethroSkillTree : SkillTree {

    // all possible skills for Jethro
    Skill helmsplitter;
    Skill trinitySlice;
    Skill arcSlash;
    Skill riser;
    Skill mordstreich;
    Skill superMordstreich; // temp
      
	// Use this for initialization
	void Start () {

        // set hero to jethro
        hero = GameControl.control.heroList[0];

        // create all techniques
        helmsplitter = new Skill();
        helmsplitter.Name = "Helmsplitter";
        helmsplitter.Cost = 1;
        helmsplitter.Description = helmsplitter.Name + "\nA powerful sword strike from above. \nCost " + helmsplitter.Cost + " \nStr 75\nCrit 03\nAcc 90";
        helmsplitter.Pm = 2;
        

        trinitySlice = new Skill();
        trinitySlice.Name = "Trinity Slice";
        trinitySlice.Cost = 1;
        trinitySlice.Description = trinitySlice.Name + "\n. \nCost " + trinitySlice.Cost + " \nStr \nCrit \nAcc ";
        trinitySlice.Pm = 0;

        arcSlash = new Skill();
        arcSlash.Name = "Arc Slash";
        arcSlash.Cost = 2;
        arcSlash.Description = arcSlash.Name + "\n. \nCost " + arcSlash.Cost + " \nStr \nCrit \nAcc ";
        arcSlash.Pm = 0;

        riser = new Skill();
        riser.Name = "Riser";
        riser.Cost = 1;
        riser.Description = riser.Name + "\n. \nCost " + riser.Cost + " \nStr \nCrit \nAcc ";
        riser.Pm = 0;

        mordstreich = new Skill();
        mordstreich.Name = "Mordstreich";
        mordstreich.Cost = 2;
        mordstreich.Description = mordstreich.Name + "\n. \nCost " + mordstreich.Cost + " \nStr \nCrit \nAcc ";
        mordstreich.Pm = 0;

        superMordstreich = new Skill();
        superMordstreich.Name = "Super Mordstreich";
        superMordstreich.Cost = 1;
        superMordstreich.Description = superMordstreich.Name + "\n\nCost " + superMordstreich.Cost + "\nStr \nCrit \nAcc ";
        superMordstreich.Pm = 0;
        
        

        // set nexts to create branches
        helmsplitter.Next = trinitySlice;
        riser.Next = mordstreich;
        mordstreich.Next = superMordstreich;

        // prerequisites
        superMordstreich.Prerequisites = new List<Technique>();
        superMordstreich.Prerequisites.Add(mordstreich);
        superMordstreich.Prerequisites.Add(arcSlash);
        superMordstreich.Prerequisites.Add(trinitySlice);

        // max num of col
        //numOfColumn = 3;

        // max num of row
        //numOfRow = 2;

        // descriptions
        if(superMordstreich.Prerequisites != null)
        {
            superMordstreich.Description += "\n\nPrerequisites: ";
            foreach(Technique t in superMordstreich.Prerequisites)
            {
                superMordstreich.Description += "\n" + t.Name;
            }
        }

        // set content array
        content2DArray = new List<List<Technique>>();
        content2DArray.Add(new List<Technique>(){helmsplitter, trinitySlice});
        content2DArray.Add(new List<Technique>() { arcSlash });
        content2DArray.Add(new List<Technique>() { riser, mordstreich, superMordstreich });

        // set sizes of columns and rows
        numOfColumn = content2DArray.Count;

        foreach(List<Technique> t in content2DArray)
        {
            if (numOfRow < t.Count) numOfRow = t.Count;
        }

        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
