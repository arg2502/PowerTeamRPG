using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable")]
public class ScriptableConsumable : ScriptableItem {

	//Consumable items usually have some sort of change to stats or status
	public string statusChange;
}
