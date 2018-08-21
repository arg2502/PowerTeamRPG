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
                PivotKick();
                break;
            case "Scorpio Jolt":
                Scorpio();
                break;
        }

        base.Attack();
    }

    void PivotKick()
    {
        var pk = GameControl.skillTreeManager.FindTechnique(Data, "Pivot Kick") as Skill;

        StartEnemyAttack(pk);
    }

    void Scorpio()
    {
        var scor = GameControl.skillTreeManager.FindTechnique(Data, "Scorpio Jolt") as Spell;

        StartEnemyAttack(scor);
    }
}
