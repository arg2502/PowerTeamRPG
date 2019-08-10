using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {

    public QuestData data;
    int currentState = 0;
    public int currentTalkToPeople;
    public int currentKillEnemies;
    public List<QuestItem> currentListItemToGet;

    public int CurrentState { get { return currentState; } }
    public int RequiredTalkToPeople { get { return data.subQuestStates[currentState].talkToPeopleNum; } }
    public int RequiredKillEnemies { get { return data.subQuestStates[currentState].killEnemiesNum; } }
	public List<QuestItem> RequiredListItemToGet { get { return data.subQuestStates[currentState].listItemToGet; } }
    public int RewardGold { get { return data.subQuestStates[currentState].rewardGold; } }
    public List<QuestItem> RewardItems { get { return data.subQuestStates[currentState].rewardItems; } }

    public Quest(string questID)
    {
        data = Resources.Load<QuestData>("Data/Quests/" + questID);
        currentListItemToGet = new List<QuestItem>();
    }

    public void CheckProgress()
    {
        // check if we have satisfied all requirements
        if(currentTalkToPeople == RequiredTalkToPeople
            && currentKillEnemies == RequiredKillEnemies
            && GotCorrectItems())
        {
            NextState();
        }
    }

    bool GotCorrectItems()
    {
        if (RequiredListItemToGet == null) return true;
        // we want to see if we have all of the right items we need,
        // as well as the correct number of items
        int itemCounter = 0;
        for(int i = 0; i < RequiredListItemToGet.Count; i++)
        {
            for (int j = 0; j < currentListItemToGet.Count; j++)
            {
                if (currentListItemToGet[j].item == RequiredListItemToGet[i].item
                    && currentListItemToGet[j].quantity >= RequiredListItemToGet[i].quantity)
                {
                    itemCounter++;
                }
            }
        }

        return (itemCounter >= RequiredListItemToGet.Count);
    }

    public void NextState()
    {
        Reward();
        currentState++;
        currentTalkToPeople = 0;
        currentKillEnemies = 0;
        currentListItemToGet.Clear();

        if (currentState >= data.subQuestStates.Count)
        {
            // complete quest
            GameControl.questTracker.CompleteQuest(data.questID);
        }        
    }

    public void IncrementTalkToPeople() { currentTalkToPeople++; CheckProgress(); }
    public void IncrementKillEnemies() { currentKillEnemies++; CheckProgress(); }
    public void AddItemToGet(ScriptableItem newItem)
    {
        bool alreadyAdded = false;
        for(int i = 0; i < currentListItemToGet.Count; i++)
        {
            if(currentListItemToGet[i].item == newItem)
            {
                currentListItemToGet[i].quantity++;
                alreadyAdded = true;
                break;
            }
        }
        if (!alreadyAdded)
        {
            var newQuestItem = new QuestItem();
            newQuestItem.item = newItem;
            newQuestItem.quantity = 1;
            currentListItemToGet.Add(newQuestItem);
        }
        CheckProgress();
    }

    void Reward()
    {
        GameControl.control.AddGold(RewardGold);
        foreach(var item in RewardItems)
        {
            GameControl.control.AddItem(item.item, item.quantity);
        }
    }
}
