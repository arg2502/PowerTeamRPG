using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class QuestTracker {
    public static Dictionary<string, Quest> activeQuests;    
    public static Dictionary<string, Quest> completedQuests;

    public static void Init()
    {
        activeQuests = new Dictionary<string, Quest>();
        completedQuests = new Dictionary<string, Quest>();
        AddNewQuest("solomvale", skipShow: true);
    }

    public static void AddNewQuest(string questID, bool skipShow = false)
    {
        if (!activeQuests.ContainsKey(questID))
            activeQuests.Add(questID, new Quest(questID));
        if (!skipShow && activeQuests[questID].data.questType != QuestType.MISC)
            UIManager.ShowQuestStart(activeQuests[questID].data.questName);
    }

    public static string GetCurrentSubQuestID(string questID)
    {
        var mainQuest = activeQuests[questID];
        var subQuestID = questID + (mainQuest.CurrentState+1).ToString();
        return subQuestID;
    }

    public static string GetCurrentQuestID(string subquestID)
    {
        return Regex.Match(subquestID, @"[^\d]+").Value;
    }

    public static bool ContainsActiveKey(string subquestID)
    {
        // get the main quest ID from the subquestID, by only getting every character but the numbers
        var mainQuestID = GetCurrentQuestID(subquestID);
        if (!activeQuests.ContainsKey(mainQuestID))
            return false;

        var state = (activeQuests[mainQuestID].CurrentState + 1).ToString();
        var subQuestStr = mainQuestID + state;

        return subQuestStr == subquestID;
    }

    public static bool ContainsCompletedKey(string questID)
    {
        return completedQuests.ContainsKey(questID);
    }

    public static void IncrementTalkToPeople(string subquestID)
    {
        activeQuests[GetCurrentQuestID(subquestID)].IncrementTalkToPeople();
    }

    public static void IncrementKillEnemies(string subquestID)
    {
        activeQuests[GetCurrentQuestID(subquestID)].IncrementKillEnemies();
    }

    public static void AddItemToGet(string subquestID, ScriptableItem newItem)
    {
        activeQuests[GetCurrentQuestID(subquestID)].AddItemToGet(newItem);
    }

    /// <summary>
    /// Manually increment a quest to the next goal. This should mainly be used for specific circumstances, like cutscenes
    /// </summary>
    /// <param name="subquestID"></param>
    public static void NextSubquest(string subquestID)
    {
        activeQuests[GetCurrentQuestID(subquestID)].CheckProgress();
    }

    public static void CompleteQuest(string questID)
    {
        completedQuests.Add(questID, activeQuests[questID]);
        activeQuests.Remove(questID);
    }
}
