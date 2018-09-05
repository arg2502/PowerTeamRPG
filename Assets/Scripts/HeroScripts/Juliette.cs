using UnityEngine;
using System.Collections;

public class Juliette : Hero {

	// Use this for initialization
	new void Awake () {
        //startingLevel = 4;
        // stats - should total to 1.00f
        //hpPer = 0.19f;
        //pmPer = 0.07f;
        //atkPer = 0.12f;
        //defPer = 0.14f;
        //mgkAtkPer = 0.12f;
        //mgkDefPer = 0.08f;
        //luckPer = 0.06f;
        //evasionPer = 0.13f;
        //spdPer = 0.19f;

        base.Awake();	
	}

    public override void Attack()
    {
        switch(CurrentAttackName)
        {
            case "Pivot Kick":
            case "Scorpio Jolt":
                StartAttack(CurrentAttackName);
                break;
            case "Taunt":
                Taunt();
                break;
        }

        base.Attack();
    }

    //void PivotKick()
    //{
    //    var pk = GameControl.skillTreeManager.FindTechnique(Data, "Pivot Kick") as Skill;

    //    StartEnemyAttack(pk);
    //}

    //void Scorpio()
    //{
    //    var scor = GameControl.skillTreeManager.FindTechnique(Data, "Scorpio Jolt") as Spell;

    //    StartEnemyAttack(scor);
    //}

    void Taunt()
    {
        // at this point in the attack, we have to assume that our target will have a single target attack they can perform
        // if we happen to have an enemy that will not have a single target attack, further checking will have to be done during targeting

        Enemy target = targets[0] as Enemy;
        target.CalculatedDamage = 0;
        target.TauntAttack();
    }
}
