using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable")]
public class ScriptableConsumable : ScriptableItem {

	//Unlike equipment or armor, consumable items can have multiple types of targets
	public enum TargetType
	{
		NULL,
		ENEMY_SINGLE,
		ENEMY_SPLASH,
		ENEMY_TEAM,
		HERO_SINGLE,
		HERO_SPLASH,
		HERO_TEAM,
		HERO_SELF
	}

	// status effect
	public enum Status { 
		normal, 
		bleeding, 
		infected, 
		cursed, 
		blinded, 
		petrified, 
		dead, 
		overkill 
	};

	public TargetType targetType;

	//This variable tells us what condition the item cures
	public Status statusChange;
}
