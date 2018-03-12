using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {

	// Use this for initialization
	new void Awake () {
        
        //startingLevel = 2; // SET ELSEWHERE ??

        // stats - should total to 1.00f
        //hpPer = 0.21f;
        //pmPer = 0.16f;
        //atkPer = 0.09f;
        //defPer = 0.07f;
        //mgkAtkPer = 0.17f;
        //mgkDefPer = 0.13f;
        //luckPer = 0.10f;
        //evasionPer = 0.08f;
        //spdPer = 0.09f;

        //growthSpeed = 0.95f;

        base.Awake();
	}

    public override void Attack(string atkChoice)
    {
        // attacks specific to the character
        switch (atkChoice)
        {
            case "Candleshot":
                Candleshot();
                break;
            case "HellFire":
                HellFire();
                break;
            case "Splash Flame":
                SplashFlame();
                break;
        }

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack(atkChoice);

    }
    
    void Candleshot()
    {
        var damage = CalcDamage(0.4f, 0f, 100f, true);
        targets[0].TakeDamage(this, damage, true);
    }

    public void HellFire()
    {
        float damage;
        damage = CalcDamage(0.40f, 0.03f, 1.0f, true);
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].TakeDamage(this, damage, true);
        }
    }

    public void SplashFlame()
    {
        float damage;
        damage = CalcDamage(0.60f, 0.05f, 0.85f, true);
        //full damage to the main target
        targets[0].TakeDamage(this, damage, true);

        // half damage to the surrounding targets
        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].TakeDamage(this, damage/2.0f, true);
        }
    }

	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
