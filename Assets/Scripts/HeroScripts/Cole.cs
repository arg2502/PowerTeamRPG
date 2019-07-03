using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cole : Hero {

    public override void Attack()
    {
        // attacks specific to the character
        switch (CurrentAttackName)
        {
            case "Candleshot":
            case "Fireball":
            case "Grand Fireball":
            case "Firewall":
            case "Splash Flame":
            case "Hellfire":
                StartAttack(CurrentAttackName);
                break;
            case "Anathema":
                Anathema();
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
