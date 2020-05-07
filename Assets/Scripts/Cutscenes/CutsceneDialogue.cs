using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialogue : Dialogue {

    public TextAsset textAsset;
    Cutscene cutscene;
    bool hasStarted = false;
    int pausedOnIterator = -1;

    public List<NPCCutsceneDialogue> npcCharacters;

    private void Start()
    {
        cutscene = GetComponent<Cutscene>();
    }

    public void StartDialogue()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartDialogueTextAsset(textAsset);
        }
        else
            base.StartDialogue();
    }

    protected override void PrintConversation()
    {
        //print("cutscene print");        
        if (conversationIterator - 1 != pausedOnIterator &&             
            (conversationIterator >= currentConversation.dialogueConversation.Count ||
            string.Equals(currentConversation.actionName[conversationIterator - 1].ToLower(), "pause")))
        {
            pausedOnIterator = conversationIterator - 1;
            GameControl.UIManager.PopMenu();
            cutscene.Resume();
            return;
        }

        base.PrintConversation();
    }

    protected override float GetTalkingSpeed(string _name)
    {
        if (npcCharacters.Count > 0)
        {
            var npc = npcCharacters.Find(n => string.Equals(n.npcName, _name));

            if (npc != null)
                return npc.talkingSpeed;
        }

        return base.GetTalkingSpeed(_name);
    }

    protected override Sprite GetHeroSprite(string heroName, string emotion)
    {        
        if (npcCharacters.Count > 0)
        {
            var npc = npcCharacters.Find(n => string.Equals(n.npcName, heroName));

            if (npc != null)
            {
                return npc.GetSprite(emotion);
            }
        }

        return base.GetHeroSprite(heroName, emotion);        
    }
}

[System.Serializable]
public class NPCCutsceneDialogue
{
    public string npcName;
    [Range(0.0f, 100.0f)]
    public float talkingSpeed;
    public Sprite neutralPortrait;
    public Sprite happyPortrait;
    public Sprite sadPortrait;
    public Sprite angryPortrait;

    public Sprite GetSprite(string emotion)
    {
        switch (emotion)
        {
            case "NEUTRAL":
                return neutralPortrait;
            case "HAPPY":
                return happyPortrait;
            case "SAD":
                return sadPortrait;
            case "ANGRY":
                return angryPortrait;
            default:
                Debug.LogError("Could not find emotion -- returning neutral as default");
                return neutralPortrait;

        }
    }
}
