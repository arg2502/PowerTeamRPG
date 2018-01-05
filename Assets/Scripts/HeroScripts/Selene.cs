using UnityEngine;
using System.Collections;

public class Selene : Hero {

	// Use this for initialization
	void Start () {
        // stats - should total to 1.00f
        hpPer = 0.21f;
        pmPer = 0.10f;
        atkPer = 0.11f;
        defPer = 0.06f;
        mgkAtkPer = 0.09f;
        mgkDefPer = 0.06f;
        luckPer = 0.16f;
        evasionPer = 0.18f;
        spdPer = 0.13f;

        base.Awake();	
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
