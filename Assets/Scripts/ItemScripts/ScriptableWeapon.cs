using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon")]
public class ScriptableWeapon : ScriptableItem {

	//Weapons can be one of 3 types
	public enum Type { augment, bookmark, beads };

	//Weapons can affect any stats other than hp or pm
	public int atkChange;
	public int defChange;
	public int mgkAtkChange;
	public int mgkDefChange;
	public int luckChange;
	public int evadeChange;
	public int spdChange;

	//Also passives, whenever we get there
}
