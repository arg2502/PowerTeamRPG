﻿using UnityEngine;
using System.Collections;

public class Eleanor : Hero {

	// Use this for initialization
	void Start () {
        // stats - should total to 1.00f
        hpPer = 0.07f;
        pmPer = 0.18f;
        atkPer = 0.05f;
        defPer = 0.12f;
        mgkAtkPer = 0.15f;
        mgkDefPer = 0.13f;
        luckPer = 0.03f;
        evasionPer = 0.13f;
        spdPer = 0.14f;

        base.Start();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
