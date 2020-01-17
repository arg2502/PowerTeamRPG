using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Augment Item", menuName = "Items/Augment")]
public class ScriptableAugment : ScriptableItem {

	//Weapons can be one of 3 types
    public new enum Type { enchant, grimark, chapletti };
    public Type type;

	//Weapons can affect any stats other than hp or pm
	//Also passives, whenever we get there
}
