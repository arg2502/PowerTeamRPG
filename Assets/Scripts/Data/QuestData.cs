using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 1)]
public class QuestData : ScriptableObject {
    public string questID;
    public string questName;
    public List<SubQuestData> subQuestStates;
    
    [System.Serializable]
    public struct SubQuestData
    {
        //public string subQuestID;
        public int talkToPeopleNum;
        public int killEnemiesNum;
        [SerializeField]
        public List<QuestItem> listItemToGet;
        [Header("Rewards")]
        public int rewardGold;
        public List<QuestItem> rewardItems;
    }

}
