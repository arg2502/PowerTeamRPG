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
            case "Antiheal":
                Antiheal();
                break;
            case "Weep":
                Weep();
                break;
            case "Staff Strike":
                StartAttack(CurrentAttackName);
                break;
        }

        base.Attack();
    }

    void Purge()
    {
        SingleStatusCure();
    }

    void Antiheal()
    {
        SingleStatusAttack(DenigenData.Status.bleeding);
    }

    void Weep()
    {
        StatEffect("MGKDEF");
   //     var tech = GameControl.skillTreeManager.FindTechnique(Data, CurrentAttackName);

   //     foreach (var target in targets)
   //     {
   //         target.CalculatedDamage = 0;

   //         // set the magic defense change to a percentage of current MgkDef based off of damage
   //         // ex: dmg = 0.1; MgkDef = 10; result: Change = -1; new MgkDef = 9;
   //         // next: dmg = 0.1; MgkDef = 9; result: Change = -0.9; new MgkDef = 8.1 (round to 8)
			//target.StatChanged = "MGKDEF";
			//target.statChangeInt = -(int)(tech.Damage / 100f * target.MgkDef);
			//target.MgkDefChange += target.statChangeInt;
   //     }
    }
}
