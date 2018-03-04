﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {
    
	// Use this for initialization
	void Awake () {

        //startingLevel = 1;

        // stats - should total to 1.00f
        //hpPer = 0.24f;
        //pmPer = 0.08f;
        //atkPer = 0.145f;
        //defPer = 0.125f;
        //mgkAtkPer = 0.10f;
        //mgkDefPer = 0.09f;
        //luckPer = 0.13f;
        //evasionPer = 0.09f;
        //spdPer = 0.10f;

        //growthSpeed = 0.85f;
                
        base.Awake();

        // test of passive
        //LightRegeneration lr = new LightRegeneration();
        //lr.Start();
        //passivesList.Add(lr);
    }
	
    //public override void SelectTarget(List<Denigen> targetsFromCursors)
    //{
    //    //clear any previously selected targets from other turns
    //    if (targets != null)
    //    {
    //        targets.Clear();
    //    }

        //this will use a switch statement to determine the type of
        //targeting required, and then pass off to a more specific method
        //switch (attack)
        //{
        //    case "Block":
        //        SelectSelfTarget(attack);
        //        break;
        //    case "Helmsplitter":
        //    case "Strike":
        //        SelectSingleTarget();
        //        break;
        //    default:
        //        // if there is no case for this action, then it must be treated as an item
        //        SelectSingleTeamTarget(attack);
        //        break;
        //}
    //}

	public override void Attack (string atkChoice)
	{
		// attacks specific to the character
		switch (atkChoice) {
			case "Helmsplitter":
				if (!targets [0].IsDead)
					Helmsplitter ();
				break;
		}

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack(atkChoice);


    }
    public void Helmsplitter()
	{
		float damage;
		damage = CalcDamage("Helmsplitter", 0.5f, 0.03f, 0.9f, false);
		targets[0].TakeDamage(this, damage, false);
	}
}
