using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Items/Armor")]
public class ScriptableArmor : ScriptableItem {

	//Armor can be one of 4 types
	public enum Type { head, chest, gloves, boots };
	
	//Armor can affect any stats other than hp or pm
	public int atkChange;
	public int defChange;
	public int mgkAtkChange;
	public int mgkDefChange;
	public int luckChange;
	public int evadeChange;
	public int spdChange;

	//Also passives, whenever we get there
}
