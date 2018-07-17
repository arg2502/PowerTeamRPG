using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Crabgrass : Enemy {

    Skill stomp;
    Spell heal;

    void Start()
    {
        stomp = skillTree.stomp;
        heal = skillTree.heal;

        GameControl.skillTreeManager.AddTechnique(Data, stomp);
        GameControl.skillTreeManager.AddTechnique(Data, heal);

        defaultAttack = stomp;
    }

    void Stomp()
    {
        // I'm guessing it's just a simple single random attack?
        ChooseRandomTarget();
        SingleAttack(stomp);
    }

    void Heal()
    {
        ChooseSelfTarget();
        SingleHeal(heal);
    }

    public override Technique ChooseAttack()
    {
        targets.Clear();

        float rng = Random.value;

        // if high health, then only choose stomp
        if(healthState == Health.high)
        {
            return stomp;
        }
        // if average health, mainly stomp with small chance of heal
        else if(healthState == Health.average)
        {
            if (rng < 0.33f) return heal;
            else return stomp;
        }
        // if low health, higher chance of healing
        else
        {
            if (rng < 0.8f) return heal;
            else return stomp;
        }
    }

    public override void Attack()
    {
        if (string.Equals(CurrentAttackName, stomp.Name))
            Stomp();
        else if (string.Equals(CurrentAttackName, heal.Name))
            Heal();

        base.Attack();
    }

}
