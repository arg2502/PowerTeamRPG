using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColeSkillTree : SkillTree {

    // techniques
    Spell hellfire;
    Spell splashflame;

	// Use this for initialization
	void Start () {
        // set hero to Cole
        hero = GameControl.control.heroList[1];

        // create techniques
        hellfire = new Spell();
        hellfire.Name = "HellFire";
        hellfire.Pm = 8;
        hellfire.Description = "An all-encompassing spell with considerable power and accuracy. \n Str 40, Crit 03, Acc 100";

        splashflame = new Spell();
        splashflame.Name = "Splash Flame";
        splashflame.Pm = 3;
        splashflame.Description = "An explosive fireball that deals light damage to enemies adjacent to the target. \n Str 60, Crit 05, Acc 85";

        numOfColumn = 2;
        numOfRow = 1;

        content2DArray = new List<List<Technique>>();
        content2DArray.Add(new List<Technique>() { hellfire });
        content2DArray.Add(new List<Technique>() { splashflame });

        base.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
