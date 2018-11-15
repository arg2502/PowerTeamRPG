using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    TEST_NPC speaker;
    string currentSpeakerName;
    Sprite currentSpeakerSprite;
    string currentDialogueText;

    TextAsset currentTextAsset;
    List<string> speakerNames;
    List<Sprite> speakerEmotions;
    List<string> dialogueConversation;

    int conversationIterator = 0;

    UI.DialogueMenu dialogueMenu;

    private void Start()
    {
        speaker = GetComponent<TEST_NPC>();
    }

    public void StartDialogue(TextAsset textAsset)
    {
        // if this is our first time talking to an NPC,
        // OR the text they have to say is different from the last time we've talked,
        // set the current text asset and decipher dialogue
        if (currentTextAsset == null || currentTextAsset != textAsset) // NEEDS TESTING WITH MULTIPLE DIALOGUES
        {
            currentTextAsset = textAsset;
            DecipherConversation();
        }

        // push the dialogue menu
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.DialogueMenu);
        dialogueMenu = GameControl.UIManager.FindMenu(GameControl.UIManager.uiDatabase.DialogueMenu).GetComponent<UI.DialogueMenu>();
        dialogueMenu.SetContinue(PrintConversation);

        // Send the dialogue information to the menu to be printed
        PrintConversation();
    }

    /// <summary>
    /// Fill out the lists for the conversation based on the currently stored Text Asset, passed in from the NPC.
    /// </summary>
    void DecipherConversation()
    {
        speakerNames = new List<string>();
        speakerEmotions = new List<Sprite>();
        dialogueConversation = new List<string>();

        // split whole doc into row
        // each row makes up a sentence or section of the dialogue
        // could be possible to have someone different speak on each row
        var rows = currentTextAsset.text.Split('\n');
        
        // Loop through the lines (skipping the first line, as that contains var names like "name", "emotion", etc.
        // Add the appropriate variables to the lists
        for(int i = 1; i < rows.Length; i++)
        {
            // split rows further by dividing them by 'tab' space, since TextAsset was originally a .tsv
            var lines = rows[i].Split('\t');

            // the first column should be the name of the speaker
            // if blank, use the NPC's name
            if (string.IsNullOrEmpty(lines[0]))
                speakerNames.Add(speaker.npcName);
            else
                speakerNames.Add(lines[0]);

            // Next, we had emotional response.
            // TODO: come up with a way to know which character's sprite to add.
            // FOR NOW: just add the NPC's image
            if (string.IsNullOrEmpty(lines[1]))
                speakerEmotions.Add(speaker.neutralSpr);
            else
            {
                switch (lines[1])
                {
                    case "HAPPY":
                        speakerEmotions.Add(speaker.happySpr);
                        break;
                    case "NEUTRAL":
                        speakerEmotions.Add(speaker.neutralSpr);
                        break;
                }
            }

            // Finally, adding the dialogue. This should never be null so no need to check
            dialogueConversation.Add(lines[2]);
        }
    }

    void PrintConversation()
    {
        // if the iterator is greater than the amount of dialogue that needs to be said,
        // End the conversation
        if (conversationIterator >= dialogueConversation.Count)
        {
            conversationIterator = 0;
            speaker.EndDialogue();

            // pop dialogue menu
            GameControl.UIManager.HideAllMenus();

            return;
        }
        currentSpeakerName = speakerNames[conversationIterator];
        currentSpeakerSprite = speakerEmotions[conversationIterator];
        currentDialogueText = dialogueConversation[conversationIterator];
        
        dialogueMenu.SetText(currentSpeakerName, currentDialogueText, currentSpeakerSprite);

        conversationIterator++;

    }    
}
