using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
    Spell diamondPeak;

    

    // Trees
    MyTree basic;
    MyTree magic;

    // FILL TREES DOWN BELOW, THEN TEST

	// Use this for initialization
	void Start () {

        // set hero to jethro
        hero = GameControl.control.heroList[0];
        whichContent = new List<string> { "Basic", "Magic" };

        // read in info
        ReadInfo("techniquesJethro1.csv");

        // create all techniques
        helmsplitter = new Skill(FindTechnique("helmsplitter"));
        trinitySlice = new Skill(FindTechnique("trinitySlice"));
        arcSlash = new Skill(FindTechnique("arcSlash"));
        siegeBreaker = new SiegeBreaker(FindTechnique("siegeBreaker"));
        frostEdge = new Spell(FindTechnique("frostEdge"));
        mordstreich = new Skill(FindTechnique("mordstreich"));
        riser = new Skill(FindTechnique("riser"));
        duelistI = new Duelist(FindTechnique("duelist1"));
        duelistII = new Duelist(FindTechnique("duelist2"));
        duelistIII = new Duelist(FindTechnique("duelist3"));
        rally = new Skill(FindTechnique("rally"));
        goldSoul = new Skill(FindTechnique("goldSoul"));

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


        // read in info for next tree
        ReadInfo("techniquesJethro2.csv");

        // TREE 2 STATS
        //fog = new Spell("Fog", "Lowers enemy accuracy. Deals trifling damage over time", 1, 2, 0, 0, 100, 1, 0);
        //frost = new Spell("Frost", "Weak single hit attack that substantially reduces enemy speed.", 1, 4, 50, 5, 100, 1, 1);
        //iceArmor = new Spell("Ice Armor", "Gives Jethro armor reducing damage by ¾ for 2 turns.", 1, 5, 0, 0, 100, 1, 2);
        //iceBarrier = new Spell("Ice Barrier", "Reduces damage to team by half for 2 turns.", 1, 10, 0, 0, 100, 0, 3);
        //resilience = new Resilience("Resilience", "Makes status effects 1-2 rounds shorter.", 1, 0, 0, 0, 100, 0, 4);
        //coldShoulder = new Spell("Cold Shoulder", "?", 1, 0, 0, 0, 100, 0, 5); // NOT IN PROPOSALS - MAY NOT BE A SPELL
        //unbreakable = new Unbreakable("Unbreakable", "Cannot be killed by a critical hit, leaving him with 1 hp.", 1, 0, 0, 0, 100, 0, 6);
        //magicianI = new Magician("Magician I", "Boost magic damage output by 5%.", 1, 0, 0, 0, 100, 2, 3, 1);
        //magicianII = new Magician("Magician II", "Boost magic damage output by 10%.", 1, 0, 0, 0, 100, 3, 4, 2);
        //magicianIII = new Magician("Magician III", "Boost magic damage output by 15%.", 1, 0, 0, 0, 100, 3, 5, 3);
        //iceSpear = new Spell("Ice Spear", "Powerful single hit with a high critical chance.", 1, 8, 75, 15, 90, 2, 4);
        //frostBite = new Spell("Frost Bite", "A primal magic attack of Crestian heritage. Strike your opponent with an intense ice attack that has a 60% chance of leaving your opponent petrified.", 1, 25, 100, 15, 90, 2, 5);
        //diamondPeak = new Spell("Diamond Peak", "Conjure a rending pillar of ice, which deals heavy damage and inflicts bleeding on all opponents.", 1, 20, 90, 30, 100, 1, 7);
        //fog = AssignTechnique("fog") as Spell;
        //frost = AssignTechnique("frost") as Spell;
        //iceArmor = AssignTechnique("iceArmor") as Spell;
        //iceBarrier = AssignTechnique("iceBarrier") as Spell;
        //resilience = AssignTechnique("resilience") as Resilience;
        //coldShoulder = AssignTechnique("coldShoulder") as Spell;
        //unbreakable = AssignTechnique("unbreakable") as Unbreakable;
        //magicianI = AssignTechnique("magician1") as Magician;
        //magicianII = AssignTechnique("magician2") as Magician;
        //magicianIII = AssignTechnique("magician3") as Magician;
        //iceSpear = AssignTechnique("iceSpear") as Spell;
        //frostBite = AssignTechnique("frostBite") as Spell;
        //diamondPeak = AssignTechnique("diamondPeak") as Spell;


        // set nexts to create branches ------TODO


        // prerequisites ------TODO



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
