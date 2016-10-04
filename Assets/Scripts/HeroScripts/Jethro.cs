using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {
    
	// Use this for initialization
	void Start () {
        
        // stats - should total to 1.00f
        hpPer = 0.24f;
        pmPer = 0.08f;
        atkPer = 0.145f;
        defPer = 0.125f;
        mgkAtkPer = 0.10f;
        mgkDefPer = 0.09f;
        luckPer = 0.13f;
        evasionPer = 0.09f;
        spdPer = 0.10f;

        growthSpeed = 0.85f;

        base.Start();
        /*for (int i = 0; i < 3; i++)
        {
            level = i + 1;
            base.LevelUp();
        }*/

        // test of passive
        LightRegeneration lr = new LightRegeneration();
        lr.Start();
        passivesList.Add(lr);
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public override void SelectTarget(string attack)
    {
        //clear any previously selected targets from other turns
        if (targets != null)
        {
            targets.Clear();
        }

        //this will use a switch statement to determine the type of
        //targeting required, and then pass off to a more specific method
        switch (attack)
        {
            case "Block":
                SelectSelfTarget(attack);
                break;
            case "Helmsplitter":
            case "Strike":
                SelectSingleTarget();
                break;
            default:
                SelectSingleTarget();
                break;
        }
    }

	public override void Attack (string atkChoice)
	{
		base.Attack (atkChoice);


		// attacks specific to the character
		switch (atkChoice) {
			case "Helmsplitter":
				if (targets [0].StatusState != Status.dead)
					Helmsplitter ();
				break;
			default:
				break;

		}
	}
	public void Helmsplitter()
	{
		float damage;
		damage = CalcDamage("Helmsplitter", 0.75f, 0.03f, 0.9f, false);
		targets[0].TakeDamage(this, damage, false);
	}
}
