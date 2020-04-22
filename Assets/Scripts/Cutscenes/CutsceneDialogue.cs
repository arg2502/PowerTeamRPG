using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialogue : Dialogue {

    public TextAsset textAsset;
    Cutscene cutscene;
    bool hasStarted = false;
    int pausedOnIterator = -1;

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
}
