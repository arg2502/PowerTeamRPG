using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Augment Item", menuName = "Items/Augment")]
public class ScriptableAugment : ScriptableItem {

	//Weapons can be one of 3 types
    public new enum SubType { enchant, grimark, chapletti };
    public SubType subtype;

    public override string GetSubType()
    {
        if (subtype == SubType.enchant)
            return "enchant";
        else if (subtype == SubType.grimark)
            return "grimark";
        else
            return "chapletti";

    }

    //Weapons can affect any stats other than hp or pm
    //Also passives, whenever we get there
}
