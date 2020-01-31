using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Items/Armor")]
public class ScriptableArmor : ScriptableItem {

	//Armor can be one of 4 types
	public new enum SubType { head, chest, gloves, boots };
    public SubType subtype;

    public override string GetSubType()
    {
        if (subtype == SubType.head)
            return "head";
        else if (subtype == SubType.chest)
            return "chest";
        else if (subtype == SubType.gloves)
            return "gloves";
        else
            return "boots";
        
    }
	
	//Armor can affect any stats other than hp or pm
	//Also passives, whenever we get there
}
