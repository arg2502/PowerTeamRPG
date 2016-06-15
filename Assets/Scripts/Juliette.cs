using UnityEngine;
using System.Collections;

public class Juliette : Hero {

	// Use this for initialization
	void Start () {
        // stats - should total to 1.00f
        hpPer = 0.09f;
        pmPer = 0.07f;
        atkPer = 0.12f;
        defPer = 0.14f;
        mgkAtkPer = 0.12f;
        mgkDefPer = 0.08f;
        luckPer = 0.06f;
        evasionPer = 0.13f;
        spdPer = 0.19f;

        base.Start();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
