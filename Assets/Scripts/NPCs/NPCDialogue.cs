using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour {
    
    // each item in the list is a dialogue exchange you have with an NPC or object
    // if they only say one thing each time you talk, the list will have a count of 1.
    public List<TextAsset> dialogueList;
    [TextArea]
    public List<string> customDialogueList;
    
    int numOfTimesTalked;

    public string npcName;
    public Sprite neutralSpr;
    public Sprite happySpr;
    public Sprite sadSpr;
    public Sprite angrySpr;
    [Range(0.0f, 100.0f)]
    public float talkingSpeed = 70f;

    Dialogue dialogue;

    characterControl.CharacterState prevState;

    public bool canTalk = true;

    public List<QuestDialogue> questDialogues;
    public List<QuestDialogue> completedQuestDialogues;

    [System.Serializable]
    public struct QuestDialogue
    {
        public string questID;
        public TextAsset questDialogue;
    }
    

    // Use this for initialization
    protected void Start () {
        dialogue = GetComponent<Dialogue>();

        if (dialogue == null)
            dialogue = gameObject.AddComponent<Dialogue>();
	}

    /// <summary>
    /// Send the appropriate conversation over to the Dialogue class to handle
    /// </summary>
    public virtual void StartDialogue(TextAsset ta = null)
    {
        // Start the actual Dialogue last, as this will determine whether we should end the dialogue as well
        // (Putting this line first caused issues where the dialogue would try to start again soon after
        // ending, but then the iterator would not be 0 yet and the conversation would end, BUT THEN 
        // the character was set to Talking and was stuck forever. This way, if there is a false start,
        // the character is set to Talking first, and then the dialogues starts & ends
        // not really a fix, more like hiding the bug)

        if (GetComponentInParent<OverworldObject>())
            GetComponentInParent<OverworldObject>().HideInteractionNotification();

        // forcing a passed in text asset
        // 11/21/19 -- adding this now for beggar. No idea if this will break, but hey..let's see what happens
        if (ta != null)
        {
            dialogue.StartDialogueTextAsset(ta);
        }
        else
        {
            // check if there is any dialogue for any active quests
            var questDialogue = ActiveQuest();

            // if there are no active quest dialogues, check for any completed quests
            if (questDialogue == null)
                questDialogue = CompletedQuest();

            if (questDialogue != null)
                dialogue.StartDialogueTextAsset(questDialogue);
            else if (dialogueList.Count > 0)
                dialogue.StartDialogueTextAsset(dialogueList[numOfTimesTalked]);
            else
                dialogue.StartDialogueCustom(customDialogueList[numOfTimesTalked]);
        }
    }

    public void EndDialogue(Dialogue.Conversation currentConversation)
    {
        if (numOfTimesTalked < dialogueList.Count - 1 || numOfTimesTalked < customDialogueList.Count - 1)
            numOfTimesTalked++;

        // call any functions that need to occur after the dialogue has ended here
        if(!string.IsNullOrEmpty(currentConversation.GetLastAction()))
        {
            GetComponentInParent<OverworldObject>().Invoke(currentConversation.GetLastAction(), 0f);            
        }

        // At the end of the dialogue, set the NPC's walking method back to normal
        // ...might not be the best place for this, as the NPCPathwalkControl function that's called
        // at the start of the conversation is called in characterControl, but that's because that 
        // function needs a characterControl variable
        // plus this location kinda makes sense...

        else if (GetComponentInParent<OverworldObject>())
            GetComponentInParent<OverworldObject>().BackToNormal();
    }

    TextAsset ActiveQuest()
    {
        // check if this NPC has anything to say about any currently active quests        
        for(int i = 0; i < questDialogues.Count; i++)
        {
            if(GameControl.questTracker.ContainsActiveKey(questDialogues[i].questID)
                && !GetComponentInParent<OverworldObject>().QuestAlreadyTalked.Contains(questDialogues[i].questID))
            {
                GetComponentInParent<OverworldObject>().CurrentQuestID = questDialogues[i].questID;
                return questDialogues[i].questDialogue;
            }
        }
        return null;
    }

    TextAsset CompletedQuest()
    {
        // check if this NPC has anything to say about any completed quests        
        for (int i = 0; i < completedQuestDialogues.Count; i++)
        {
            if (GameControl.questTracker.ContainsCompletedKey(completedQuestDialogues[i].questID))
            {
                return completedQuestDialogues[i].questDialogue;
            }
        }
        return null;
    }
}
