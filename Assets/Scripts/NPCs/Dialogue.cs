using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    TEST_NPC speaker;
    string currentSpeakerName;
    Sprite currentSpeakerSprite;
    string currentDialogueText;

    List<string> speakerNames;
    List<Sprite> speakerEmotions;
    List<string> dialogueConversation;

    int conversationIterator = 0;

    private void Start()
    {
        speaker = GetComponent<TEST_NPC>();
    }

    public void StartDialogue(TextAsset textAsset)
    {
        if (dialogueConversation == null)
            DecipherConversation(textAsset);

        else if (conversationIterator >= dialogueConversation.Count)
        {
            print("END");
            conversationIterator = 0;
            speaker.EndDialogue();
            return;
        }
        PrintConversation();
    }

    /// <summary>
    /// Fill out the lists for the conversation based on the passed in file
    /// </summary>
    /// <param name="textAsset"></param>
    void DecipherConversation(TextAsset textAsset)
    {
        speakerNames = new List<string>();
        speakerEmotions = new List<Sprite>();
        dialogueConversation = new List<string>();

        // split whole doc into row
        // each row makes up a sentence or section of the dialogue
        // could be possible to have someone different speak on each row
        var rows = textAsset.text.Split('\n');
        
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
        currentSpeakerName = speakerNames[conversationIterator];
        currentSpeakerSprite = speakerEmotions[conversationIterator];
        currentDialogueText = dialogueConversation[conversationIterator];

        print(currentSpeakerName + ": " + currentDialogueText);

        conversationIterator++;

    }    
}
