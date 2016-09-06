using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {

	// Use this for initialization
	void Start () {
        // TEMPORARY LISTS
        skillsList = new List<string>() {  };
        spellsList = new List<string>() { "HellFire", "Splash Flame" };

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

        growthSpeed = 0.95f;

        base.Start();
	}

    public override void Attack(string atkChoice)
    {
        base.Attack(atkChoice);

        // attacks specific to the character
        switch (atkChoice)
        {
            case "HellFire":
                HellFire();
                break;
            case "Splash Flame":
                SplashFlame();
                break;
            default:
                break;

        }
    }

    //select the target for your attack
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
                break;
            case "Strike":
                SelectSingleTarget();
                break;
            case "Splash Flame":
                SelectSplashTarget();
                break;
            case "HellFire":
                SelectAllTargets();
                break;
            default:
                SelectSingleTarget();
                break;
        }
    }

    public void HellFire()
    {
        float damage;
        damage = CalcDamage("HellFire", 0.40f, 0.03f, 1.0f, false);
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].TakeDamage(damage, true);
        }
    }

    public void SplashFlame()
    {
        float damage;
        damage = CalcDamage("Splash Flame", 0.60f, 0.05f, 0.85f, false);
        //full damage to the main target
        targets[0].TakeDamage(damage, true);

        // half damage to the surrounding targets
        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].TakeDamage(damage/2.0f, true);
        }
    }

	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
