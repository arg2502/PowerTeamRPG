using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Mudpuppy : Enemy {

    Skill bite;
    Skill frenzy;
    
    protected override void AssignAttack()    
    {
        bite = skillTree.bite;
        frenzy = skillTree.frenzy;

        SkillTreeManager.AddTechnique(Data, bite);
        SkillTreeManager.AddTechnique(Data, frenzy);

        defaultAttack = bite;
	}

    //Attack method for bite -- straight forward physical attack
    //IEnumerator Bite()
    void Bite()
    {
        // code for choosing the target of this attack
        // because mudpuppy is an early enemy, let's have it attack the hero with the most remaining hp
        //ChooseRandomTarget();
        ChooseHighestHPTarget();
        SingleAttack(bite);
    }

    void Frenzy()
    {
		ChooseSelfTarget ();
		StatChanged = "ATK";
		statChangeInt = (int)(Atk * 0.1f);
		AtkChange += statChangeInt;
        targets[0].CalculatedDamage = 0;
        //AtkChange += (int)(Atk * 0.1f);
        calcDamageText.Add(name + " used frenzy!");

    }

    public override Technique ChooseAttack()
    {
        // clear the targets list
        targets.Clear();

        // Use rng to provide variety to decision making
        float rng = Random.value; //returns a random number between 0 and 1, apparently RandomRange is depricated

        //Use health states to change mudpuppy's behavior throughout the battle
        if (healthState == Health.high)
        {
            if (rng < 0.5f) { return frenzy; } //boost atk
            else { return bite; } //attack
        }
        else if (healthState == Health.average)
        {
            if (rng < 0.25f) { return frenzy; } //boost atk
            else { return bite; } //attack
        }
        else
        {
            return bite;
        }
    }

    public override void Attack()
    {
        if (string.Equals(CurrentAttackName, bite.Name))
            Bite();
        else if (string.Equals(CurrentAttackName, frenzy.Name))
            Frenzy();

        base.Attack();
    }    
}
