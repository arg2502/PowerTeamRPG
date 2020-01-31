using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableItem : ScriptableObject {

	//Override unity's default name variable
	public new string name;

	//Variables universal to all items
	public Sprite sprite;
    [TextArea]
	public string description;
	public int value;
	public Boosts[] statBoosts;
	//There will be little to no functionality in the item classes
	//Instead, Items will act as data containers, with the menus 
	//holding all of the functionality for the items

    public string Type
    {
        get
        {
            if (this is ScriptableConsumable)
                return "consumable";
            else if (this is ScriptableArmor)
                return "armor";
            else if (this is ScriptableAugment)
                return "augment";
            else
                return "key";
        }
    }

    public virtual string GetSubType() { return ""; }
}
