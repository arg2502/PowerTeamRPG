using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class QuestTracker {
    public Dictionary<string, Quest> activeQuests;    
    public Dictionary<string, Quest> finishedQuests;

    public QuestTracker()
    {
        activeQuests = new Dictionary<string, Quest>();
        finishedQuests = new Dictionary<string, Quest>();
    }

    public void AddNewQuest(string questID)
    {
        activeQuests.Add(questID, new Quest(questID));
    }

    public string GetCurrentSubQuestID(string questID)
    {
        var mainQuest = activeQuests[questID];
        var subQuestID = questID + (mainQuest.CurrentState+1).ToString();
        return subQuestID;
    }

    public string GetCurrentQuestID(string subquestID)
    {
        return Regex.Match(subquestID, @"[^\d]+").Value;
    }

    public bool ContainsKey(string subquestID)
    {
        // get the main quest ID from the subquestID, by only getting every character but the numbers
        var mainQuestID = GetCurrentQuestID(subquestID);
        if (!activeQuests.ContainsKey(mainQuestID))
            return false;

        var state = (activeQuests[mainQuestID].CurrentState + 1).ToString();
        var subQuestStr = mainQuestID + state;

        return subQuestStr == subquestID;
    }

    public void IncrementTalkToPeople(string subquestID)
    {
        activeQuests[GetCurrentQuestID(subquestID)].IncrementTalkToPeople();
    }

    public void IncrementKillEnemies(string subquestID)
    {
        activeQuests[GetCurrentQuestID(subquestID)].IncrementKillEnemies();
    }

    public void AddItemToGet(string subquestID, ScriptableItem newItem)
    {
        activeQuests[GetCurrentQuestID(subquestID)].AddItemToGet(newItem);
    }

    public void CompleteQuest(string questID)
    {
        finishedQuests.Add(questID, activeQuests[questID]);
        activeQuests.Remove(questID);
    }
}
