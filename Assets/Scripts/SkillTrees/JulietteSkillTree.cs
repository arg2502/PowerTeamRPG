using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class JulietteSkillTree : SkillTree
{

    // all possible skills for Jethro

    // tree 1
    Skill pivotKick;
    Spell scorpio;
    Skill tempest;
    Spell conduct;
    Conductor conductor;
    Skill climax;
    Skill drill;
    Skill doubleCut;
    Skill headDash;
    Skill armorRush;
    Spell doubleDash;

    // tree 2
    Skill taunt;
    Skill boast;
    Rushdown rushdown1;
    Rushdown rushdown2;
    Rushdown rushdown3;
    Skill heckle;
    Untouchable untouchable;
    Skill misdirection;
    Skill flickerDodge;
    Skill flickerCounter;
    Skill polarize;
    Skill flickerAssault;

    // Trees
    MyTree tree1;
    MyTree tree2;

    // FILL TREES DOWN BELOW, THEN TEST

    // Use this for initialization
    public JulietteSkillTree()
    {

        // set hero to jethro
        hero = GameControl.control.heroList[2];
        whichContent = new List<string> { "Tree1", "Tree2" };

        // read in info
        ReadInfo("techniquesJuliette1.tsv");

        // create all techniques
        pivotKick = new Skill(FindTechnique("pivotKick"));
        scorpio = new Spell(FindTechnique("scorpio"));
        tempest = new Skill(FindTechnique("tempest"));
        conduct = new Spell(FindTechnique("conduct"));
        conductor = new Conductor(FindTechnique("conductor"));
        climax = new Skill(FindTechnique("climax"));
        drill = new Skill(FindTechnique("drill"));
        doubleCut = new Skill(FindTechnique("doubleCut"));
        headDash = new Skill(FindTechnique("headDash"));
        armorRush = new Skill(FindTechnique("armorRush"));
        doubleDash = new Spell(FindTechnique("doubleDash"));

        // next
        pivotKick.ListNextTechnique = new List<Technique>() { scorpio, climax, headDash };
        scorpio.ListNextTechnique = new List<Technique>() { tempest };
        tempest.ListNextTechnique = new List<Technique>() { conduct };
        conduct.ListNextTechnique = new List<Technique>() { conductor };
        climax.ListNextTechnique = new List<Technique>() { drill };
        drill.ListNextTechnique = new List<Technique>() { doubleCut };
        headDash.ListNextTechnique = new List<Technique>() { armorRush };
        armorRush.ListNextTechnique = new List<Technique>() { doubleDash };

        // prerequisites
        scorpio.Prerequisites = new List<Technique>() { pivotKick };
        tempest.Prerequisites = new List<Technique>() { scorpio };
        conduct.Prerequisites = new List<Technique>() { tempest };
        conductor.Prerequisites = new List<Technique>() { conduct };
        climax.Prerequisites = new List<Technique>() { pivotKick };
        drill.Prerequisites = new List<Technique>() { climax };
        doubleCut.Prerequisites = new List<Technique>() { drill };
        headDash.Prerequisites = new List<Technique>() { pivotKick };
        armorRush.Prerequisites = new List<Technique>() { headDash };
        doubleDash.Prerequisites = new List<Technique>() { armorRush };

        // read in info for next tree
        ReadInfo("techniquesJuliette2.tsv");

        // TREE 2 STATS        
        taunt = new Skill(FindTechnique("taunt"));
        boast = new Skill(FindTechnique("boast"));
        rushdown1 = new Rushdown(FindTechnique("rushdown1"));
        rushdown2 = new Rushdown(FindTechnique("rushdown2"));
        rushdown3 = new Rushdown(FindTechnique("rushdown3"));
        heckle = new Skill(FindTechnique("heckle"));
        untouchable = new Untouchable(FindTechnique("untouchable"));
        misdirection = new Skill(FindTechnique("misdirection"));
        flickerDodge = new Skill(FindTechnique("flickerDodge"));
        flickerCounter = new Skill(FindTechnique("flickerCounter"));
        polarize = new Skill(FindTechnique("polarize"));
        flickerAssault = new Skill(FindTechnique("flickerAssault"));

        // next
        taunt.ListNextTechnique = new List<Technique>() { boast };
        boast.ListNextTechnique = new List<Technique>() { rushdown1, flickerDodge };
        rushdown1.ListNextTechnique = new List<Technique>() { rushdown2, heckle };
        rushdown2.ListNextTechnique = new List<Technique>() { rushdown3 };
        heckle.ListNextTechnique = new List<Technique>() { untouchable };
        untouchable.ListNextTechnique = new List<Technique>() { misdirection };
        flickerDodge.ListNextTechnique = new List<Technique>() { flickerCounter };
        flickerCounter.ListNextTechnique = new List<Technique>() { polarize };
        polarize.ListNextTechnique = new List<Technique>() { flickerAssault };

        // prerequisites
        boast.Prerequisites = new List<Technique>() { taunt };
        rushdown1.Prerequisites = new List<Technique>() { boast };
        rushdown2.Prerequisites = new List<Technique>() { rushdown1 };
        rushdown3.Prerequisites = new List<Technique>() { rushdown2 };
        heckle.Prerequisites = new List<Technique>() { rushdown1 };
        untouchable.Prerequisites = new List<Technique>() { heckle };
        misdirection.Prerequisites = new List<Technique>() { untouchable };
        flickerDodge.Prerequisites = new List<Technique>() { boast };
        flickerCounter.Prerequisites = new List<Technique>() { flickerDodge };
        polarize.Prerequisites = new List<Technique>() { flickerCounter };
        flickerAssault.Prerequisites = new List<Technique>() { polarize };


        // trees
        tree1 = new MyTree();
        tree2 = new MyTree();

        // set content array
        tree1.listOfContent = new List<Technique>() { pivotKick, scorpio, tempest, conduct, conductor, climax, drill, doubleCut, headDash, armorRush, doubleDash };
        tree2.listOfContent = new List<Technique>() { taunt, boast, rushdown1, rushdown2, rushdown3, heckle, untouchable, misdirection, flickerDodge, flickerCounter, polarize, flickerAssault };

        // set sizes of columns and rows
        tree1.numOfColumn = 4;
        tree1.numOfRow = 5;
        tree2.numOfColumn = 6;
        tree2.numOfRow = 5;

        // set starting position
        tree1.rootCol = 2;
        tree1.rootRow = 0;
        tree2.rootCol = 3;
        tree2.rootRow = 0;
        
        listOfTrees = new List<MyTree>();
        listOfTrees.Add(tree1);
        listOfTrees.Add(tree2);
        
    }    
}
