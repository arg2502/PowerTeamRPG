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
            case "Fireball":
                Fireball();
                break;
            case "Grand Fireball":
                GrandFireball();
                break;
            case "Firewall":
                Firewall();
                break;
            case "Splash Flame":
                SplashFlame();
                break;
            case "HellFire":
                HellFire();
                break;
        }

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack(atkChoice);

    }
    
    void Candleshot()
    {
        SingleAttack(40f, 0f, 100f, true);
    }

    void Fireball()
    {
        SingleAttack(55f, 5f, 95f, true);
    }

    void GrandFireball()
    {
        SingleAttack(75f, 10f, 90f, true);
    }

    public void SplashFlame()
    {
        SplashAttack(60f, 5f, 85f, true);
    }

    void Firewall()
    {
        TeamAttack(20f, 6f, 100f, true);
    }

    public void HellFire()
    {
        TeamAttack(40f, 3f, 100f, true);
    }
}
