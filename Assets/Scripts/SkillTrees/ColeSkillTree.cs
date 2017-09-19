﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColeSkillTree : SkillTree {

    // techniques
    Spell candleshot;
    Spell fireball;
    Caster caster1;
    Spell cauterize;
    Spell slowBurn;
    Spell grandFireball;
    Caster caster2;
    Spell splashFlame;
    Caster caster3;
    Spell firewall;
    Spell hellfire;
    Spell coleFusion;

    // tree 2
    Spell anathema;
    Skill bucket;
    Spell twilight;
    Spell hollow;
    Spell resist;
    Spell study;
    Spell resiviction;
    Spell eclipse;
    Spell bonecrush;
    Spell reaperGaze;

    // trees
    MyTree basic;
    MyTree basic2;

	// Use this for initialization
	void Start () {
        // set hero to Cole
        hero = GameControl.control.heroList[1];

        whichContent = new List<string>() { "Tree1", "Tree2" };

        // read in technique info
        ReadInfo("techniquesCole1.tsv");

        // create techniques
        candleshot = new Spell(FindTechnique("candleshot"));
        fireball = new Spell(FindTechnique("fireball"));
        caster1 = new Caster(FindTechnique("caster1"));
        cauterize = new Spell(FindTechnique("cauterize"));
        slowBurn = new Spell(FindTechnique("slowBurn"));
        grandFireball = new Spell(FindTechnique("grandFireball"));
        caster2 = new Caster(FindTechnique("caster2"));
        splashFlame = new Spell(FindTechnique("splashFlame"));
        caster3 = new Caster(FindTechnique("caster3"));
        firewall = new Spell(FindTechnique("firewall"));
        hellfire = new Spell(FindTechnique("hellfire"));
        coleFusion = new Spell(FindTechnique("coleFusion"));

        // prerequisites
        fireball.Prerequisites = new List<Technique>() { candleshot };
        caster1.Prerequisites = new List<Technique>() { fireball };
        cauterize.Prerequisites = new List<Technique>() { caster1 };
        slowBurn.Prerequisites = new List<Technique>() { cauterize };
        grandFireball.Prerequisites = new List<Technique> { caster1 };
        caster2.Prerequisites = new List<Technique>() { grandFireball };
        splashFlame.Prerequisites = new List<Technique>() { caster2 };
        caster3.Prerequisites = new List<Technique>() { splashFlame };
        firewall.Prerequisites = new List<Technique>() { caster1 };
        hellfire.Prerequisites = new List<Technique>() { firewall };
        coleFusion.Prerequisites = new List<Technique>() { slowBurn, caster3, hellfire };

        // tree 2
        ReadInfo("techniquesCole2.tsv");

        anathema = new Spell(FindTechnique("anathema"));
        bucket = new Skill(FindTechnique("bucketSplash"));
        twilight = new Spell(FindTechnique("twilightCascade"));
        hollow = new Spell(FindTechnique("hollow"));
        resist = new Spell(FindTechnique("resistEnchantment"));
        study = new Spell(FindTechnique("study"));
        resiviction = new Spell(FindTechnique("resiviction"));
        eclipse = new Spell(FindTechnique("eclipse"));
        bonecrush = new Spell(FindTechnique("bonecrush"));
        reaperGaze = new Spell(FindTechnique("reaperGaze"));

        // prereq
        twilight.Prerequisites = new List<Technique>() { anathema };
        hollow.Prerequisites = new List<Technique>() { twilight };
        resist.Prerequisites = new List<Technique>() { hollow };
        study.Prerequisites = new List<Technique>() { resist };
        resiviction.Prerequisites = new List<Technique>() { study };
        eclipse.Prerequisites = new List<Technique>() { twilight };
        bonecrush.Prerequisites = new List<Technique>() { eclipse };
        reaperGaze.Prerequisites = new List<Technique>() { bonecrush };


        // initiate trees
        basic = new MyTree();
        basic2 = new MyTree();

        basic.numOfColumn = 4;
        basic.numOfRow = 9;
        basic2.numOfColumn = 4;
        basic2.numOfRow = 6;

        basic.listOfContent = new List<Technique>() { candleshot, fireball, caster1, cauterize, slowBurn, grandFireball, caster2, splashFlame, caster3, firewall, hellfire, coleFusion };
        basic2.listOfContent = new List<Technique>() { anathema, bucket, twilight, hollow, resist, study, resiviction, eclipse, bonecrush, reaperGaze };

        basic.rootCol = 2;
        basic.rootRow = 0;
        basic2.rootCol = 2;
        basic2.rootRow = 0;

        AddPrerequisites(basic.listOfContent);
        AddPrerequisites(basic2.listOfContent);

        listOfTrees = new List<MyTree>() { basic, basic2 };
        

        base.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
