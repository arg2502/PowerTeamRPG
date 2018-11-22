using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon")]
public class ScriptableWeapon : ScriptableItem {

	//Weapons can be one of 3 types
	public enum Type { augment, bookmark, beads };

	//Weapons can affect any stats other than hp or pm
	//Also passives, whenever we get there
}
