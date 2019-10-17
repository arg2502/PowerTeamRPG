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
	public JethroSkillTree() {

        // set hero to jethro
        hero = GameControl.control.heroList[0];
        whichContent = new List<string> { "Basic", "Magic" };

        // read in info
        ReadInfo("techniquesJethro1.tsv");

        // create all techniques
        helmsplitter = new Skill(FindTechnique("helmsplitter"));
        trinitySlice = new Skill(FindTechnique("trinitySlice"));
        arcSlash = new Skill(FindTechnique("arcSlash"));
        siegeBreaker = new SiegeBreaker(FindTechnique("siegeBreaker"));
        frostEdge = new Spell(FindTechnique("frostEdge"));
        mordstreich = new Skill(FindTechnique("mordstreich"));
        riser = new Skill(FindTechnique("riser"));
        duelistI = new Duelist(FindTechnique("duelist1"), 1);
        duelistII = new Duelist(FindTechnique("duelist2"), 2);
        duelistIII = new Duelist(FindTechnique("duelist3"), 3);
        rally = new Skill(FindTechnique("rally"));
        goldSoul = new Skill(FindTechnique("goldSoul"));

        // set nexts to create branches
        helmsplitter.ListNextTechnique = new List<Technique>() { trinitySlice, duelistI };
        trinitySlice.ListNextTechnique = new List<Technique>() { arcSlash, siegeBreaker };
        siegeBreaker.ListNextTechnique = new List<Technique>() { frostEdge };
        frostEdge.ListNextTechnique = new List<Technique>() { mordstreich, riser };
        duelistI.ListNextTechnique = new List<Technique>() { rally };
        rally.ListNextTechnique = new List<Technique>() { duelistII };
        duelistII.ListNextTechnique = new List<Technique>() { duelistIII, goldSoul };

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
        ReadInfo("techniquesJethro2.tsv");

        // TREE 2 STATS        
        fog = new Spell(FindTechnique("fog"));
        frost = new Spell(FindTechnique("frost"));
        iceArmor = new Spell(FindTechnique("iceArmor"));
        iceBarrier = new Spell(FindTechnique("iceBarrier"));
        resilience = new Resilience(FindTechnique("resilience"));
        coldShoulder = new Spell(FindTechnique("coldShoulder"));
        unbreakable = new Unbreakable(FindTechnique("unbreakable"));
        magicianI = new Magician(FindTechnique("magician1"), 1);
        magicianII = new Magician(FindTechnique("magician2"), 2);
        magicianIII = new Magician(FindTechnique("magician3"), 3);
        iceSpear = new Spell(FindTechnique("iceSpear"));
        frostBite = new Spell(FindTechnique("frostBite"));
        diamondPeak = new Spell(FindTechnique("diamondPeak"));


        // set nexts to create branches
        fog.ListNextTechnique = new List<Technique>() { frost };
        frost.ListNextTechnique = new List<Technique>() { iceArmor };
        iceArmor.ListNextTechnique = new List<Technique>() { iceBarrier, magicianI };
        iceBarrier.ListNextTechnique = new List<Technique>() { resilience };
        resilience.ListNextTechnique = new List<Technique>() { coldShoulder };
        coldShoulder.ListNextTechnique = new List<Technique>() { unbreakable };
        unbreakable.ListNextTechnique = new List<Technique>() { diamondPeak };
        magicianI.ListNextTechnique = new List<Technique>() { iceSpear, magicianII };
        iceSpear.ListNextTechnique = new List<Technique>() { frostBite };
        magicianII.ListNextTechnique = new List<Technique>() { magicianIII };

        // prerequisites
        frost.Prerequisites = new List<Technique>() { fog };
        iceArmor.Prerequisites = new List<Technique>() { frost };
        iceBarrier.Prerequisites = new List<Technique>() { iceArmor };
        resilience.Prerequisites = new List<Technique>() { iceBarrier };
        coldShoulder.Prerequisites = new List<Technique>() { resilience };
        unbreakable.Prerequisites = new List<Technique>() { coldShoulder };
        diamondPeak.Prerequisites = new List<Technique>() { unbreakable };
        magicianI.Prerequisites = new List<Technique>() { iceArmor };
        iceSpear.Prerequisites = new List<Technique>() { magicianI };
        frostBite.Prerequisites = new List<Technique>() { iceSpear };
        magicianII.Prerequisites = new List<Technique>() { magicianI };
        magicianIII.Prerequisites = new List<Technique>() { magicianII };


        // trees
        basic = new MyTree();
        magic = new MyTree();

        // set content array
        basic.listOfContent = new List<Technique>() { helmsplitter, trinitySlice, arcSlash, siegeBreaker, frostEdge, mordstreich, riser, duelistI, duelistII, duelistIII, rally, goldSoul };
        magic.listOfContent = new List<Technique>() { fog, frost, iceArmor, iceBarrier, resilience, coldShoulder, unbreakable, magicianI, magicianII, magicianIII, iceSpear, frostBite, diamondPeak };
        

        // set sizes of columns and rows
        basic.numOfColumn = 5;
        basic.numOfRow = 5;
        magic.numOfColumn = 5;
        magic.numOfRow = 8;

        // set starting position
        basic.rootCol = 2;
        basic.rootRow = 0;
        magic.rootCol = 2;
        magic.rootRow = 0;

        
        listOfTrees = new List<MyTree>();
        listOfTrees.Add(basic);
        listOfTrees.Add(magic);

        startingTechs = new List<Technique>() { helmsplitter, trinitySlice, arcSlash, riser, mordstreich, siegeBreaker, frostEdge
                                                ,duelistI,duelistII,duelistIII, rally, goldSoul, frost, iceArmor, iceBarrier, magicianI, magicianII, magicianIII};
	}
    
}
