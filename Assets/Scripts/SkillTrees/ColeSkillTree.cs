using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColeSkillTree : SkillTree {

    // techniques
    Spell hellfire;
    Spell splashflame;

    MyTree basic;

	// Use this for initialization
	void Start () {
        // set hero to Cole
        hero = GameControl.control.heroList[1];

        // create techniques
        hellfire = new Spell();
        hellfire.Name = "HellFire";
        hellfire.Pm = 8;
		hellfire.Description = "An all-encompassing spell with considerable power and accuracy. \nCost " + hellfire.Pm + "pm, Str 40, Crit 03, Acc 100";

        splashflame = new Spell();
        splashflame.Name = "Splash Flame";
        splashflame.Pm = 3;
		splashflame.Description = "An explosive fireball that deals light damage to enemies adjacent to the target. \nCost " + splashflame.Pm + "pm, Str 60, Crit 05, Acc 85";

        basic.numOfColumn = 2;
        basic.numOfRow = 1;

        basic.listOfContent = new List<Technique>() { hellfire, splashflame };

        base.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
