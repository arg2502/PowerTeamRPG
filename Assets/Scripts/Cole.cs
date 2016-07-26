using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {

	// Use this for initialization
	void Start () {
        // stats - should total to 1.00f
        hpPer = 0.11f;
        pmPer = 0.16f;
        atkPer = 0.09f;
        defPer = 0.07f;
        mgkAtkPer = 0.17f;
        mgkDefPer = 0.13f;
        luckPer = 0.10f;
        evasionPer = 0.08f;
        spdPer = 0.09f;

        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
