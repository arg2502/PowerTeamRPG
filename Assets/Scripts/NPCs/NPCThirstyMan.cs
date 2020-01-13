using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCThirstyMan : StationaryNPCControl
{
    //public enum ThirstyState { THIRSTY, SATISFIED, DEAD }
    public enum ItemState { MIN, MOD, MAX, BLEACH}
    //ThirstyState thirstyState;
    ItemState currentItemState;
    public List<ThirstyManResponse> thirstyManResponses;

    public List<ScriptableConsumable> minItems;
    public List<ScriptableConsumable> modItems;
    public List<ScriptableConsumable> maxItems;
    public List<ScriptableConsumable> bleachItems;

    const int MIN_GOLD = 50;
    const int MOD_GOLD = 250;
    const int MAX_GOLD = 1000;
    const int BLEACH_GOLD = 5000;

    private void Start()
    {
        base.Start();
        // destroy if we've already finished the Thirsty Man quest
        if (GameControl.questTracker.completedQuests.ContainsKey("thirstyMan"))
            Destroy(gameObject);
    }

    public void OpenConsumables()
    {
        GameControl.UIManager.PushThirstyManMenu(this);
        GameControl.questTracker.AddNewQuest("thirstyMan");
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
        if (bleachItems.Find((i) => i.name == item.name))
            currentItemState = ItemState.BLEACH;
        else if (maxItems.Find((i) => i.name == item.name))
            currentItemState = ItemState.MAX;
        else if (modItems.Find((i) => i.name == item.name))
            currentItemState = ItemState.MOD;
        else if (minItems.Find((i) => i.name == item.name))
            currentItemState = ItemState.MIN;

        if (currentItemState != ItemState.BLEACH)
        {
            //thirstyState = ThirstyState.SATISFIED;
            var anim = GetComponent<Animator>();
            anim.SetBool("isSatisfied", true);
        }
        else
        {
            //thirstyState = ThirstyState.DEAD;
            GetComponentInChildren<NPCDialogue>().canTalk = false;
        }

        GameControl.questTracker.CompleteQuest("thirstyMan");
        GameControl.control.RemoveItem(item);
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
