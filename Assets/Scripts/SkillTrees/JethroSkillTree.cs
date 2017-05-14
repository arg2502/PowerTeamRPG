using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JethroSkillTree : SkillTree {

    // all possible skills for Jethro

    // tree 1
    Skill helmsplitter;
    Skill trinitySlice;
    Skill arcSlash;
    Passive siegeBreaker;
    Spell frostEdge;
    Skill mordstreich;
    Skill riser;
    Passive duelistI;
    Passive duelistII;
    Passive duelistIII;
    Skill rally;
    Skill goldSoul;

    // tree 2
    Spell fog;
    Spell frost;
    Spell iceArmor;
    Spell iceBarrier;
    Passive resilience;
    Spell coldShoulder;
    Passive unbreakable;
    Passive magicianI;
    Passive magicianII;
    Passive magicianIII;
    Spell iceSpear;
    Spell frostBite;

    

    // Trees
    MyTree basic;

    // FILL TREES DOWN BELOW, THEN TEST

	// Use this for initialization
	void Start () {

        // set hero to jethro
        hero = GameControl.control.heroList[0];
        whichContent = new List<string> { "Basic" };
        // create all techniques
        helmsplitter = new Skill("Helmsplitter", "A powerful sword strike from above.", 1, 2, 75, 3, 90, 2, 0);
        trinitySlice = new Skill("Trinity Slice", "Rapidly slash an opponent three times.", 1, 6, 40, 5, 80, 1, 1);
        arcSlash = new Skill("Arc Slash", "DESCRIPTION", 2, 0, 0, 0, 0, 0, 1);
       
        riser = new Skill("Riser", "DESCRIPTION", 1, 0, 0, 0, 0, 1, 4);
        mordstreich = new Skill("Mordstreich", "DESCRIPTION", 1, 0, 0, 0, 0, 0, 3);
        


      
        // set nexts to create branches
        helmsplitter.ListNextTechnique = new List<Technique>();
        riser.ListNextTechnique = new List<Technique>();
        mordstreich.ListNextTechnique = new List<Technique>();
        helmsplitter.ListNextTechnique.Add(trinitySlice);
        riser.ListNextTechnique.Add(mordstreich);
        

        // prerequisites
        trinitySlice.Prerequisites = new List<Technique>();
        mordstreich.Prerequisites = new List<Technique>();
        trinitySlice.Prerequisites.Add(helmsplitter);
        mordstreich.Prerequisites.Add(riser);

       

        // trees
        basic = new MyTree();

        // set content array
        basic.listOfContent = new List<Technique>() { helmsplitter, trinitySlice };
        
        // set sizes of columns and rows
        basic.numOfColumn = 3;
        basic.numOfRow = 3;

        // set starting position
        basic.rootCol = 2;
        basic.rootRow = 0;
        
        //foreach(List<Technique> t in basic.listOfContent)
        //{
           // if (basic.numOfRow < t.Count) basic.numOfRow = t.Count;

            // add prerequisites to descriptions
            foreach(Technique tq in basic.listOfContent)
            {
                if(tq != null && tq.Prerequisites != null)
                {
                    tq.Description += "\n\nPrerequisites: ";
                    foreach(Technique tqn in tq.Prerequisites)
                    {                        
                        tq.Description += "\n" + tqn.Name;
                    }
                }
            }

        
        listOfTrees = new List<MyTree>();
        listOfTrees.Add(basic);

        base.Start();
	}
    
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
