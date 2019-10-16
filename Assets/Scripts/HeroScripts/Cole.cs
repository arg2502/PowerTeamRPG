using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {

    public override void Attack()
    {
        // attacks specific to the character
        switch (CurrentAttackName)
        {
            case "Anathema":
                Anathema();
                break;
            case "Candleshot":
                StartAttack(CurrentAttackName);
                break;
        }

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack();

    }

    void Anathema()
    {
        SingleStatusAttack(DenigenData.Status.cursed);
    }
}
