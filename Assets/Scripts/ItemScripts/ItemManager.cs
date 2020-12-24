using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager {
    
    public static void ItemUse(DenigenData user, InventoryItem invItem)
    {
        var itemIsForLiving = ItemForLiving(invItem.name);
        if (itemIsForLiving && user.IsDead)
            return;

        // just consumable for now
        // eventually we'll need to divide them up based on Item type
        // Or just call different functions initially based on item type
        var item = ItemDatabase.GetItem(invItem) as ScriptableConsumable;

        if (item != null)
        {
            for (int j = 0; j < GameControl.control.consumables.Count; j++)
            {
                if (item.name == GameControl.control.consumables[j].name)
                {
                    //Check if the item offers any status changes
                    //Ignore if it's status change doesn't pertain to the target's status
                    if (item.statusChange != ScriptableConsumable.Status.normal
                        && (DenigenData.Status)item.statusChange == user.statusState)
                    {
                        // double check and make sure they're not dead before setting them back to normal
                        if (user.IsDead)
                            return;
                        user.statusState = DenigenData.Status.normal;
                    }
                    //run through any stat boosts the item may offer
                    foreach (Boosts b in item.statBoosts)
                    {
                        switch (b.statName)
                        {
                            case "HP":
                                user.hp += b.boost;
                                if (user.hp > user.hpMax) user.hp = user.hpMax;
                                break;
                            case "PM":
                                user.pm += b.boost;
                                if (user.pm > user.pmMax) user.pm = user.pmMax;
                                break;
                            default:
                                Debug.Log("Error on item use by " + user.name + ": Attempted to boost stat named " + b.statName);
                                break;
                        }
                    }
                    GameControl.control.consumables[j].quantity--;
                    if (GameControl.control.consumables[j].quantity <= 0)
                    {
                        GameControl.control.consumables.Remove(GameControl.control.consumables[j]);
                    }
                    break;
                }
            }
        }
    }
    public static void BattleItemUse(Hero user, List<Denigen> targets)
    {
        // if the item is intended for living, but the target is dead, don't use the item -- skip the turn
        var itemIsForLiving = ItemForLiving(user.CurrentAttackName);
        if (itemIsForLiving && targets[0].IsDead)
            return;

        ScriptableConsumable item = ItemDatabase.GetItem("Consumable",
                                                        user.CurrentAttackName) as ScriptableConsumable;
        if (item != null) {
            for (int j = 0; j < GameControl.control.consumables.Count; j++) {
                if (item.name == GameControl.control.consumables[j].name) {
                    //item.Use(targets[0]);
                    for (int i = 0; i < targets.Count; i++) {

                        //Check if the item offers any status changes
                        //Ignore if it's status change doesn't pertain to the target's status
                        if (item.statusChange != ScriptableConsumable.Status.normal
                            && (DenigenData.Status)item.statusChange == targets[i].StatusState) {
                            // double check and make sure they're not dead before setting them back to normal
                            if (targets[i].IsDead)
                                return;
                            targets[i].CalculatedDamage = 0;
                            targets[i].HealedStatusEffect = targets[i].StatusState;
                            targets[i].StatusChanged = true;
                            //targets [i].SetStatus (DenigenData.Status.normal);
                            targets[i].MarkAsStatusChanged(DenigenData.Status.normal);
                        }
                        //run through any stat boosts the item may offer
                        foreach (Boosts b in item.statBoosts) {
                            switch (b.statName) {
                                case "HP":
                                    targets[i].SetHPHealingValue(b.boost);
                                    break;
                                case "PM":
                                    targets[i].SetPMHealingValue(b.boost);
                                    break;
                                case "ATK":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].AtkChange += b.boost;
                                    break;
                                case "DEF":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].DefChange += b.boost;
                                    break;
                                case "MGKATK":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].MgkAtkChange += b.boost;
                                    break;
                                case "MGKDEF":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].MgkDefChange += b.boost;
                                    break;
                                case "LUCK":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].LuckChange += b.boost;
                                    break;
                                case "EVASION":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].EvasionChange += b.boost;
                                    break;
                                case "SPD":
                                    Debug.Log("Target's " + b.statName + " is boosted by " + b.boost);
                                    targets[i].SpdChange += b.boost;
                                    break;
                                default:
                                    Debug.Log("Error on item use by " + user.name + ": Attempted to boost stat named " + b.statName);
                                    break;
                            }
                        }
                    }
                    GameControl.control.consumables[j].quantity--;
                    if (GameControl.control.consumables[j].quantity <= 0) {
                        GameControl.control.consumables.Remove(GameControl.control.consumables[j]);
                    }
                    break;
                }
            }
        }
    }

    public static bool ItemForLiving(string item)
    {
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

    public static bool IsBattleOnly(InventoryItem _item)
    {
        ScriptableConsumable item = ItemDatabase.GetItem("Consumable", _item.name) as ScriptableConsumable;

        foreach (var b in item.statBoosts)
        {
            switch (b.statName)
            {
                case "HP":
                case "PM":
                    continue;
                default: return true;
            }
        }
        return false;
    }

    public static void EquipItem(DenigenData user, InventoryItem invItem)
    {
        ScriptableItem item = null;
        List<InventoryItem> itemList = null;
        if (invItem.type == "augment")
        {
            item = ItemDatabase.GetItem(invItem) as ScriptableAugment;
            itemList = GameControl.control.augments;
        }
        else if (invItem.type == "armor")
        {
            item = ItemDatabase.GetItem(invItem) as ScriptableArmor;
            itemList = GameControl.control.armor;
        }

        if(item != null)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].name == item.name)
                {
                    foreach (Boosts b in item.statBoosts)
                    {
                        BoostUser(user, b);
                    }

                    if (invItem.type == "augment")
                    {
                        GameControl.control.augments[i].uses++;
                        user.augments.Add(invItem);
                    }
                    else if (invItem.type == "armor")
                    {
                        GameControl.control.armor[i].uses++;
                        user.armor.Add(invItem);
                    }
                }
            }
        }
    }

    public static void RemoveItem(DenigenData user, InventoryItem invItem)
    {
        if (invItem.type == "augment")
        {
            var item = ItemDatabase.GetItem(invItem) as ScriptableAugment;

            if (item != null)
            {
                for (int i = 0; i < GameControl.control.augments.Count; i++)
                {
                    if (GameControl.control.augments[i].name == item.name)
                    {
                        foreach (Boosts b in item.statBoosts)
                        {
                            BoostUser(user, b, false);
                        }

                        GameControl.control.augments[i].uses--;
                        user.augments.Remove(invItem);
                    }
                }
            }
        }
        else if (invItem.type == "armor")
        {
            var item = ItemDatabase.GetItem(invItem) as ScriptableArmor;

            if (item != null)
            {
                for(int i = 0; i < GameControl.control.armor.Count; i++)
                {
                    if(GameControl.control.armor[i].name == item.name)
                    {
                        foreach(Boosts b in item.statBoosts)
                        {
                            BoostUser(user, b, false);
                        }

                        GameControl.control.armor[i].uses--;
                        user.armor.Remove(invItem);
                    }
                }
            }
        }

    }

    private static void BoostUser(DenigenData user, Boosts b, bool equip = true)
    {
        var boost = b.boost * ((equip) ? 1 : -1);
        switch (b.statName)
        {
            case "HP":
                user.hpMax = AddBoost(user.hpMax, boost);             
                break;
            case "PM":
                user.pmMax = AddBoost(user.pmMax, boost);
                break;
            case "ATK":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.atk = AddBoost(user.atk, boost);
                break;
            case "DEF":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.def = AddBoost(user.def, boost);
                break;
            case "MGKATK":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.mgkAtk = AddBoost(user.mgkAtk, boost);
                break;
            case "MGKDEF":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.mgkDef = AddBoost(user.mgkDef, boost);
                break;
            case "LUCK":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.luck = AddBoost(user.luck, boost);
                break;
            case "EVASION":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.evasion = AddBoost(user.evasion, boost);
                break;
            case "SPD":
                Debug.Log("Target's " + b.statName + " is boosted by " + boost);
                user.spd = AddBoost(user.spd, boost);
                break;
            default:
                Debug.Log("Error on item use by " + user.name + ": Attempted to boost stat named " + b.statName);
                break;
        }
    }

    private static int AddBoost(int stat, int boost)
    {
        return (stat + boost < 0) ? 0 : stat + boost;
    }


}
