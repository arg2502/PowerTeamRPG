using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a serializable class that stores the stat an item boosts as well as how many points it boosts said stat
//It is used by the ScriptableItem class as an easy way to edit the properties of an item
[System.Serializable]
public class Boosts {
	public string statName;
	public int boost;
}
