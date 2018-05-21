using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Crabgrass : Enemy {

    void Start()
    {
        // TEMPORARY
        Skill stomp = new Skill();
        stomp.Name = "Stomp";
        stomp.Pm = 0;
        stomp.Description = "Stomp stomp stomp";
        SkillsList.Add(stomp);

        Spell heal = new Spell();
        heal.Name = "Heal";
        heal.Pm = 0;
        heal.Description = "Healing McHealface";
        SpellsList.Add(heal);
    }

    void Stomp()
    {
        // I'm guessing it's just a simple single random attack?
        ChooseRandomTarget();
        SingleAttack(75, 25, 90, false); // TEMPORARY
    }

    void Heal()
    {
        ChooseSelfTarget();
        SingleHeal(15, 0, 100); // TEMPORARY
    }

    public override string ChooseAttack()
    {
        targets.Clear();

        float rng = Random.value;

        // if high health, then only choose stomp
        if(healthState == Health.high)
        {
            return "Heal";//"Stomp";
        }
        // if average health, mainly stomp with small chance of heal
        else if(healthState == Health.average)
        {
            if (rng < 0.33f) return "Heal";
            else return "Stomp";
        }
        // if low health, higher chance of healing
        else
        {
            if (rng < 0.66f) return "Heal";
            else return "Stomp";
        }
    }

    public override void Attack()
    {
        switch(CurrentAttackName)
        {
            case "Stomp":
                Stomp();
                break;
            case "Heal":
                Heal();
                break;
        }

        base.Attack();
    }

}
