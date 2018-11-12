using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable")]
public class ScriptableConsumable : ScriptableItem {

	//Consumable items usually have some sort of change to stats or status
	public string statusChange;
	public int hpChange;
	public int hpMaxChange;
	public int pmChange;
	public int pmMaxChange;
	public int atkChange;
	public int defChange;
	public int mgkAtkChange;
	public int mgkDefChange;
	public int luckChange;
	public int evadeChange;
	public int spdChange;
}
