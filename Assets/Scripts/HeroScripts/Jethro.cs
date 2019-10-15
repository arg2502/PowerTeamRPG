using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {

    int frostEdgeCounter = 0;
    int frostEdgeDelta = 0;

    public override void CheckForResetStats()
    {
        base.CheckForResetStats();
        if (frostEdgeCounter > 0)
        {
            frostEdgeCounter--;
            if (frostEdgeCounter <= 0)
                ReverseFrostEdge();
        }
    }

    public override void Attack ()
	{
		// attacks specific to the character
		switch (CurrentAttackName) {
			case "Riser":
                Riser();
                break;
            case "Frost Edge":
                FrostEdge();
                break;
            case "Strike":
            case "Block":
                break;
            default:
                StartAttack(CurrentAttackName);
                break;
		}

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack();
    }

    void Riser()
    {
        StartAttack(CurrentAttackName);

        // 60% chance of lowering defense
        var value = Random.value;
        if (value <= 0.6f)
            StatEffect("DEF", -10f);
    }

    void FrostEdge()
    {
        var percentage = 15f;
        var originalLuckChange = LuckChange;
        StatEffect("LUCK", percentage);
        var newLuckChange = LuckChange;
        frostEdgeDelta = newLuckChange - originalLuckChange;
        frostEdgeCounter = 2;
        print("luck change: " + LuckChange);
    }
    void ReverseFrostEdge()
    {
        RemoveStatEffectChange(this, "LUCK", frostEdgeDelta);
        frostEdgeDelta = 0;
        print("remove change: " + LuckChange);
    }
}
