using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Items/Armor")]
public class ScriptableArmor : ScriptableItem {

	//Armor can be one of 4 types
	public enum Type { head, chest, gloves, boots };
	
	//Armor can affect any stats other than hp or pm
	//Also passives, whenever we get there
}
