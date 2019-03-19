using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class E_Goikko : Enemy {

    Skill tongueWhip;
    Spell poison;

    // Use this for initialization -- goikko is basically mudpuppy, other than stats
    protected override void AssignAttack()
    {
        tongueWhip = skillTree.whip;
        poison = skillTree.poison;

        GameControl.skillTreeManager.AddTechnique(Data, tongueWhip);
        GameControl.skillTreeManager.AddTechnique(Data, poison);

        defaultAttack = tongueWhip;
	}
    
    void TongueWhip()
    {       
        SingleAttack(tongueWhip);
    }
    void Poison()
    {
        SingleStatusAttack(DenigenData.Status.infected);
    }
    public override Technique ChooseAttack()
    {
        //CLEAR the targets list
        targets.Clear();

        // choose target first, THEN determine type of attack
        ChooseRandomTarget();
        bool targetAlreadyPoisoned = targets[0].StatusState == DenigenData.Status.infected;

        // Use rng to provide variety to decision making
        float rng = Random.value; //returns a random number between 0 and 1, apparently RandomRange is depricated

        //Use health states to change goikko's behavior throughout the battle
        //return poison;
        if (healthState == Health.high)
        {
            if (!targetAlreadyPoisoned && rng < 0.5f) { return poison; } //boost atk
            else { return tongueWhip; } //attack
        }
        else if (healthState == Health.average)
        {
            if (!targetAlreadyPoisoned && rng < 0.25f) { return poison; } //boost atk
            else { return tongueWhip; } //attack
        }
        else
        {
            return tongueWhip;
        }
    }

    public override void Attack()
    {
        if (string.Equals(CurrentAttackName, tongueWhip.Name))
            TongueWhip();
        else if (string.Equals(CurrentAttackName, poison.Name))
            Poison();

        base.Attack();
    }    
}
