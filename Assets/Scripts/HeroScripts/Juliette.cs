using UnityEngine;
using System.Collections;

public class Juliette : Hero {

    public override void Attack()
    {
        switch(CurrentAttackName)
        {
            case "Taunt":
                Taunt();
                break;
            default:
                StartAttack(CurrentAttackName);
                break;
        }
        base.Attack();
    }

    void Taunt()
    {
        // at this point in the attack, we have to assume that our target will have a single target attack they can perform
        // if we happen to have an enemy that will not have a single target attack, further checking will have to be done during targeting

        Enemy target = targets[0] as Enemy;
        target.CalculatedDamage = 0;
        target.TauntAttack();
    }
}
