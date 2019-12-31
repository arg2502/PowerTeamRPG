using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCThirstyMan : StationaryNPCControl
{
    public enum ItemState { MIN, MOD, MAX, BLEACH}
    ItemState currentItemState;
    public List<ThirstyManResponse> thirstyManResponses;

    const int MIN_GOLD = 50;
    const int MOD_GOLD = 250;
    const int MAX_GOLD = 1000;
    const int BLEACH_GOLD = 5000;

    public void OpenConsumables()
    {
        GameControl.UIManager.PushThirstyManMenu(this);
    }

    public void PayMinimum()
    {
        GameControl.control.AddGold(MIN_GOLD, true);
    }

    public void PayModerate()
    {
        GameControl.control.AddGold(MOD_GOLD, true);
    }

    public void PayMaximum()
    {
        GameControl.control.AddGold(MAX_GOLD, true);
    }

    public void PayBleach()
    {
        GameControl.control.AddGold(BLEACH_GOLD, true);
    }
	
    public void DrinkItem(InventoryItem item)
    {
        if (item.name.ToLower().Contains("bleach"))
        {
            currentItemState = ItemState.BLEACH;
        }
        else
        {
            var scriptableItem = ItemDatabase.GetItem(item);
            if (scriptableItem.value <= 100)
                currentItemState = ItemState.MIN;
            else if (scriptableItem.value > 100 && scriptableItem.value < 300)
                currentItemState = ItemState.MOD;
            else if (scriptableItem.value > 300)
                currentItemState = ItemState.MAX;
        }

        var dialogue = thirstyManResponses.Find((r) => r.itemState == currentItemState).dialogue;
        GameControl.UIManager.PopMenu();
        GetComponentInChildren<NPCDialogue>().StartDialogue(dialogue);
    }
}

[System.Serializable]
public class ThirstyManResponse
{
    public NPCThirstyMan.ItemState itemState;
    public TextAsset dialogue;
}
