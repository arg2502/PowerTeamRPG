﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {
    
    public override void Attack ()
	{
		// attacks specific to the character
		switch (CurrentAttackName) {
			case "Helmsplitter":
            case "Trinity Slice":
            case "Arc Slash":
                StartAttack(CurrentAttackName);
				break;
            case "Riser":
                Riser();
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
            StatEffect("DEF", 10f);
    }
}
