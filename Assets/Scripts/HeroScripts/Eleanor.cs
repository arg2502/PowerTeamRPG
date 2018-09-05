using UnityEngine;
using System.Collections;

public class Eleanor : Hero {
    
    public override void Attack()
    {
        switch(CurrentAttackName)
        {
            case "Purge":
                Purge();
                break;
            case "Morttimer's Tears":
            case "Gaze of Morttimer":
                StartAttack(CurrentAttackName);
                break;
            case "Antiheal":
                Antiheal();
                break;
            case "Weep":
                Weep();
                break;
        }

        base.Attack();
    }

    void Purge()
    {
        SingleStatusCure();
    }

    //void Tears()
    //{
    //    // NOT FINAL -- RANDOMLY CHOSEN
    //    SingleHeal(60, 0, 100);
    //}

    //void Gaze()
    //{
    //    // NOT FINAL -- RANDOMLY CHOSEN
    //    TeamHeal(50, 0, 100);
    //}

    void Antiheal()
    {
        SingleStatusAttack(DenigenData.Status.bleeding);
        //print(targets[0].DenigenName + " is now bleeding");
    }

    void Weep()
    {
        var tech = GameControl.skillTreeManager.FindTechnique(Data, CurrentAttackName);

        foreach (var target in targets)
        {
            target.CalculatedDamage = 0;

            // set the magic defense change to a percentage of current MgkDef based off of damage
            // ex: dmg = 0.1; MgkDef = 10; result: Change = -1; new MgkDef = 9;
            // next: dmg = 0.1; MgkDef = 9; result: Change = -0.9; new MgkDef = 8.1 (round to 8)
            print("change before: " + target.MgkDefChange + ", mgk def: " + target.MgkDef);
            target.MgkDefChange -= (int)(tech.Damage / 100f * target.MgkDef);
            print("change after: " + target.MgkDefChange + ", mgk def: " + target.MgkDef);
        }
    }
}
