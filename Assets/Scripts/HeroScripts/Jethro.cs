using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jethro : Hero {
    
    public override void Attack ()
	{
		// attacks specific to the character
		switch (CurrentAttackName) {
			case "Helmsplitter":
                StartAttack(CurrentAttackName);
				break;
		}

        // check parent function to take care of reducing pm
        // also check if the attack is a general hero attack (Strike, Block) or an item use
        base.Attack();
    }
}
