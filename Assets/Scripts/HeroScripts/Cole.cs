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

    public override void Attack()
    {
        // attacks specific to the character
        switch (CurrentAttackName)
        {
            case "Candleshot":
            case "Fireball":
            case "Grand Fireball":
            case "Firewall":
            case "Splash Flame":
            case "Hellfire":
                StartAttack(CurrentAttackName);
                break;
        }

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack();

    }

    
    //void Candleshot()
    //{
    //    //SingleAttack(40f, 0f, 100f, true);
    //    var candle = GameControl.skillTreeManager.FindTechnique(Data, "Candleshot");
    //    StartEnemyAttack(candle);
    //}

    //void Fireball()
    //{
    //    //SingleAttack(55f, 5f, 95f, true);
    //    var fireball = GameControl.skillTreeManager.FindTechnique(Data, "Fireball");
    //    StartEnemyAttack(fireball);
    //}

    //void GrandFireball()
    //{
    //    //SingleAttack(75f, 10f, 90f, true);
    //    var grand = GameControl.skillTreeManager.FindTechnique(Data, "Grand Fireball");
    //    StartEnemyAttack(grand);
    //}

    //public void SplashFlame()
    //{
    //    //SplashAttack(60f, 5f, 85f, true);
    //    var 
    //}

    //void Firewall()
    //{
    //    TeamAttack(20f, 6f, 100f, true);
    //}

    //public void HellFire()
    //{
    //    TeamAttack(40f, 3f, 100f, true);
    //}
}
