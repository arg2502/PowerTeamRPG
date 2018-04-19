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
                Tears();
                break;
            case "Gaze of Morttimer":
                Gaze();
                break;
            case "Antiheal":
                Antiheal();
                break;
        }

        base.Attack();
    }

    void Purge()
    {
        SingleStatusCure();
    }

    void Tears()
    {
        // NOT FINAL -- RANDOMLY CHOSEN
        SingleHeal(20, 0, 100);
    }

    void Gaze()
    {
        // NOT FINAL -- RANDOMLY CHOSEN
        TeamHeal(20, 0, 100);
    }

    void Antiheal()
    {
        SingleStatusAttack(DenigenData.Status.bleeding);
        print(targets[0].DenigenName + " is now bleeding");
    }

}
