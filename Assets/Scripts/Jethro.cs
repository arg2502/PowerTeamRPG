using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {
    
	// Use this for initialization
	void Start () {
        // TEMPORARY LISTS
        skillsList = new List<string>() { "Good", "God", "Really Good", "SuperGod"};
        spellsList = new List<string>() { "Godspell", "Firepuff", "Wimsy Ass", "Ecpliseddd", "You're goddamn right" };

        // stats - should total to 1.00f
        hpPer = 0.14f;
        pmPer = 0.08f;
        atkPer = 0.145f;
        defPer = 0.125f;
        mgkAtkPer = 0.10f;
        mgkDefPer = 0.09f;
        luckPer = 0.13f;
        evasionPer = 0.09f;
        spdPer = 0.10f;

        base.Start();
        /*for (int i = 0; i < 3; i++)
        {
            level = i + 1;
            base.LevelUp();
        }*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
