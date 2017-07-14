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
        arcSlash = new Skill("Arc Slash", "A wide swing striking a row of opponents.", 2, 9, 50, 5, 100, 0, 1);
        siegeBreaker = new SiegeBreaker("Siege Breaker", "Boosts all damage output at lower health.", 0, 0, 0, 0, 0, 1, 2);
        frostEdge = new Spell("Frost Edge", "Increase chance of critical hit by 15% for 2 turns.", 1, 7, 0, 0, 100, 1, 3);
        mordstreich = new Skill("Mordstreich", "Jump into the air for a deadly sword thrust.", 1, 15, 75, 60, 60, 0, 3);
        riser = new Skill("Riser", "Jumping sword strike. Knocks opponent upward. 60% chance to decrease enemy's defense.", 1, 10, 75, 10, 95, 1, 4);
        duelistI = new Duelist("Duelist I", "Boost physical damage output by 5%.", 1, 0, 0, 0, 100, 3, 1, 1);
        duelistII = new Duelist("Duelist II", "Boost physical damage output by 10%.", 1, 0, 0, 0, 100, 3, 3, 2);
        duelistIII = new Duelist("Duelist III", "Boost physical damage output by 15%.", 1, 0, 0, 0, 100, 4, 3, 3);
        rally = new Skill("Rally", "Boost physical attack for the entire team.", 1, 5, 0, 0, 100, 3, 2);
        goldSoul = new Skill("Gold Soul", "Boost stats of entire team slightly.", 1, 20, 0, 0, 100, 0, 3);


        // set nexts to create branches
        helmsplitter.ListNextTechnique = new List<Technique>();
        helmsplitter.ListNextTechnique.Add(trinitySlice);
        helmsplitter.ListNextTechnique.Add(duelistI);

        trinitySlice.ListNextTechnique = new List<Technique>();
        trinitySlice.ListNextTechnique.Add(arcSlash);
        trinitySlice.ListNextTechnique.Add(siegeBreaker);

        siegeBreaker.ListNextTechnique = new List<Technique>();
        siegeBreaker.ListNextTechnique.Add(frostEdge);

        frostEdge.ListNextTechnique = new List<Technique>();
        frostEdge.ListNextTechnique.Add(mordstreich);
        frostEdge.ListNextTechnique.Add(riser);

        duelistI.ListNextTechnique = new List<Technique>();
        duelistI.ListNextTechnique.Add(rally);

        rally.ListNextTechnique = new List<Technique>();
        rally.ListNextTechnique.Add(duelistII);

        duelistII.ListNextTechnique = new List<Technique>();
        duelistII.ListNextTechnique.Add(duelistIII);
        duelistII.ListNextTechnique.Add(goldSoul);
        

        // prerequisites
        trinitySlice.Prerequisites = new List<Technique>();
        trinitySlice.Prerequisites.Add(helmsplitter);

        arcSlash.Prerequisites = new List<Technique>();
        arcSlash.Prerequisites.Add(trinitySlice);

        siegeBreaker.Prerequisites = new List<Technique>();
        siegeBreaker.Prerequisites.Add(trinitySlice);

        frostEdge.Prerequisites = new List<Technique>();
        frostEdge.Prerequisites.Add(siegeBreaker);

        mordstreich.Prerequisites = new List<Technique>();
        mordstreich.Prerequisites.Add(frostEdge);

        riser.Prerequisites = new List<Technique>();
        riser.Prerequisites.Add(frostEdge);

        duelistI.Prerequisites = new List<Technique>();
        duelistI.Prerequisites.Add(helmsplitter);

        rally.Prerequisites = new List<Technique>();
        rally.Prerequisites.Add(duelistI);

        duelistII.Prerequisites = new List<Technique>();
        duelistII.Prerequisites.Add(rally);

        duelistIII.Prerequisites = new List<Technique>();
        duelistIII.Prerequisites.Add(duelistII);

        goldSoul.Prerequisites = new List<Technique>();
        goldSoul.Prerequisites.Add(duelistII);
                      

        // trees
        basic = new MyTree();

        // set content array
        basic.listOfContent = new List<Technique>() { helmsplitter, trinitySlice, arcSlash, siegeBreaker, frostEdge, mordstreich, riser, duelistI, duelistII, duelistIII, rally, goldSoul };
        
        // set sizes of columns and rows
        basic.numOfColumn = 5;
        basic.numOfRow = 5;

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
