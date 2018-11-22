using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager {
    
    public ItemManager()
    {

    }

	public void ItemUse(Hero user, List<Denigen> targets)
	{
		// if the item is intended for living, but the target is dead, don't use the item -- skip the turn
		var itemIsForLiving = ItemForLiving(user.CurrentAttackName);
		if (itemIsForLiving && targets[0].IsDead)
			return;

		ScriptableConsumable item = ItemDatabase.GetItem("Consumable",
			                                            user.CurrentAttackName) as ScriptableConsumable;
		if (item != null) {
			for(int j = 0; j < GameControl.control.consumables.Count; j++){
				if (item.name == GameControl.control.consumables[j].name) {
					//item.Use(targets[0]);
					for (int i = 0; i < targets.Count; i++) {
					
						//Check if the item offers any status changes
						//Ignore if it's status change doesn't pertain to the target's status
						if (item.statusChange != ScriptableConsumable.Status.normal 
							&& (DenigenData.Status)item.statusChange == targets [i].StatusState) {
							// double check and make sure they're not dead before setting them back to normal
							if (targets [i].IsDead)
								return;
							targets [i].CalculatedDamage = 0;
							targets [i].HealedStatusEffect = targets[i].StatusState;
							targets [i].StatusChanged = true;
							targets [i].SetStatus (DenigenData.Status.normal);
						}
						//run through any stat boosts the item may offer
						foreach (Boosts b in item.statBoosts) {
							switch (b.statName) {
							case "HP":
								targets [i].SetHPHealingValue (b.boost);
								break;
							case "PM":
								targets [i].SetPMHealingValue (b.boost);
								break;
							case "ATK":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].AtkChange += b.boost;
								break;
							case "DEF":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].DefChange += b.boost;
								break;
							case "MGKATK":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].MgkAtkChange += b.boost;
								break;
							case "MGKDEF":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].MgkDefChange += b.boost;
								break;
							case "LUCK":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].LuckChange += b.boost;
								break;
							case "EVASION":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].EvasionChange += b.boost;
								break;
							case "SPD":
								Debug.Log ("Target's " + b.statName + " is boosted by " + b.boost);
								targets [i].SpdChange += b.boost;
								break;
							default:
								Debug.Log ("Error on item use by " + user.name + ": Attempted to boost stat named " + b.statName);
								break;
							}
						}
					}
					GameControl.control.consumables [j].quantity --;
					if (GameControl.control.consumables [j].quantity <= 0) {
						GameControl.control.consumables.Remove (GameControl.control.consumables [j]);
					}
					break;
				}
			}
		}
	}

    public bool ItemForLiving(string item)
    {
//		return true;
//		Debug.Log("Called ItemForLivingMethod for item named " + item);
		ScriptableConsumable _item = ItemDatabase.GetItem("Consumable", item) as ScriptableConsumable;
	
		if (_item == null) {
			//This is a band-aid solution because I have no Idea why the ItemForLiving method is called all the time
			//But it gets called for every command issued
			return true;
		} else if (_item.statusChange == ScriptableConsumable.Status.dead) {
			//if the status change pertains to dead denigens, then the item is not for the living
			return false;
		} else { //if it pertains to any other status, it is for the living
			return true;
		} 
    }
}
