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
        ReadInfo("techniquesJethro1");

        // create all techniques
        helmsplitter = new Skill(FindTechnique("helmsplitter"), imageDatabase.helmsplitter);
        trinitySlice = new Skill(FindTechnique("trinitySlice"), imageDatabase.trinitySlice);
        arcSlash = new Skill(FindTechnique("arcSlash"), imageDatabase.arcSlash);
        siegeBreaker = new SiegeBreaker(FindTechnique("siegeBreaker"), imageDatabase.siegeBreaker);
        frostEdge = new Spell(FindTechnique("frostEdge"), imageDatabase.frostEdge);
        mordstreich = new Skill(FindTechnique("mordstreich"), imageDatabase.mordstreich);
        riser = new Skill(FindTechnique("riser"), imageDatabase.riser);
        duelistI = new Duelist(FindTechnique("duelist1"), 1, imageDatabase.duelistI);
        duelistII = new Duelist(FindTechnique("duelist2"), 2, imageDatabase.duelistII);
        duelistIII = new Duelist(FindTechnique("duelist3"), 3, imageDatabase.duelistIII);
        rally = new Skill(FindTechnique("rally"), imageDatabase.rally);
        goldSoul = new Skill(FindTechnique("goldSoul"), imageDatabase.goldSoul);

        // set nexts to create branches
        helmsplitter.ListNextTechnique = new List<Technique>() { trinitySlice, duelistI };
        trinitySlice.ListNextTechnique = new List<Technique>() { arcSlash, siegeBreaker };
        siegeBreaker.ListNextTechnique = new List<Technique>() { frostEdge };
        frostEdge.ListNextTechnique = new List<Technique>() { mordstreich, riser };
        duelistI.ListNextTechnique = new List<Technique>() { rally };
        rally.ListNextTechnique = new List<Technique>() { duelistII };
        duelistII.ListNextTechnique = new List<Technique>() { duelistIII, goldSoul };

        // prerequisites
        trinitySlice.Prerequisites = new List<Technique>() { helmsplitter };
        arcSlash.Prerequisites = new List<Technique>() { trinitySlice };
        siegeBreaker.Prerequisites = new List<Technique>() { trinitySlice };
        frostEdge.Prerequisites = new List<Technique>() { siegeBreaker };
        mordstreich.Prerequisites = new List<Technique>() { frostEdge };
        riser.Prerequisites = new List<Technique>() { frostEdge };
        duelistI.Prerequisites = new List<Technique>() { helmsplitter };
        rally.Prerequisites = new List<Technique>() { duelistI };
        duelistII.Prerequisites = new List<Technique>() { rally };
        duelistIII.Prerequisites = new List<Technique>() { duelistII };
        goldSoul.Prerequisites = new List<Technique>() { duelistII };

        // read in info for next tree
        ReadInfo("techniquesJethro2");

        // TREE 2 STATS        
        fog = new Spell(FindTechnique("fog"), imageDatabase.fog);
        frost = new Spell(FindTechnique("frost"), imageDatabase.frost);
        iceArmor = new Spell(FindTechnique("iceArmor"), imageDatabase.iceArmor);
        iceBarrier = new Spell(FindTechnique("iceBarrier"), imageDatabase.iceBarrier);
        resilience = new Resilience(FindTechnique("resilience"), imageDatabase.resilience);
        coldShoulder = new Spell(FindTechnique("coldShoulder"), imageDatabase.coldShoulder);
        unbreakable = new Unbreakable(FindTechnique("unbreakable"), imageDatabase.unbreakable);
        magicianI = new Magician(FindTechnique("magician1"), 1, imageDatabase.magicianI);
        magicianII = new Magician(FindTechnique("magician2"), 2, imageDatabase.magicianII);
        magicianIII = new Magician(FindTechnique("magician3"), 3, imageDatabase.magicianIII);
        iceSpear = new Spell(FindTechnique("iceSpear"), imageDatabase.iceSpear);
        frostBite = new Spell(FindTechnique("frostBite"), imageDatabase.frostBite);
        diamondPeak = new Spell(FindTechnique("diamondPeak"), imageDatabase.diamondPeak);


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
                                                ,duelistI,duelistII,duelistIII, rally, goldSoul, frost, iceArmor, iceBarrier, magicianI, magicianII, magicianIII,
			iceSpear, frostBite, diamondPeak, unbreakable, fog, resilience};
	}
    
}
