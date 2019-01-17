using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    NPCDialogue speaker;
    string currentSpeakerName;
    Sprite currentSpeakerSprite;
    string currentDialogueText;

    TextAsset currentTextAsset;

    int conversationIterator = 0;

    UI.DialogueMenu dialogueMenu;

    // Keeps hold of everything the dialogue conversation needs:
    // names of whose speaker, sprite emotions, and the actual dialogue
    // Also keeps track of any possible responses the player needs to make
    public struct Conversation
    {
        public List<string> speakerNames;
        public List<Sprite> speakerEmotions;
        public List<string> dialogueConversation;
        public List<Response> responses;
        public string actionName;

        public string GetSpeakerName(int increment)
        {
            return speakerNames[increment];
        }

        public Sprite GetSpeakerEmotion(int increment)
        {
            return speakerEmotions[increment];
        }

        public string GetDialogueConversation(int increment)
        {
            return dialogueConversation[increment];
        }

        public bool IsThereResponse()
        {
            return responses != null && responses.Count > 0;
        }        
    }
    Conversation currentConversation;

    // Used when the player has to opportunity to make a choice in the dialogue
    // Stores what the player can say to respond, and the conversation that takes place after that response
    public struct Response
    {
        public string playerResponse;
        public Conversation conversation;
    }

    private void Start()
    {
        speaker = GetComponent<NPCDialogue>();
    }

    public void StartDialogue(TextAsset textAsset)
    {
        // if this is our first time talking to an NPC,
        // OR the text they have to say is different from the last time we've talked,
        // set the current text asset and decipher dialogue
        //if (currentTextAsset == null || currentTextAsset != textAsset)
        //{
            currentTextAsset = textAsset;
            currentConversation = DecipherConversation(currentTextAsset.text);
        //}

        // push the dialogue menu
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.DialogueMenu);
        dialogueMenu = GameControl.UIManager.FindMenu(GameControl.UIManager.uiDatabase.DialogueMenu).GetComponent<UI.DialogueMenu>();
        dialogueMenu.SetContinue(PrintConversation);
        dialogueMenu.OnNextDialogue.RemoveAllListeners();
        dialogueMenu.OnNextDialogue.AddListener(CheckForResponseMenu); // add event listener

        // Send the dialogue information to the menu to be printed
        PrintConversation();
    }

    /// <summary>
    /// Fill out the lists for the conversation based on the currently stored Text Asset, passed in from the NPC.
    /// </summary>
    Conversation DecipherConversation(string currentText, int startAtRow = 1, int endAtRow = -1)
    {
        Conversation newConversation = new Conversation();
        newConversation.speakerNames = new List<string>();
        newConversation.speakerEmotions = new List<Sprite>();
        newConversation.dialogueConversation = new List<string>();

        // split whole doc into row
        // each row makes up a sentence or section of the dialogue
        // could be possible to have someone different speak on each row
        var rows = currentTextAsset.text.Split('\n');

        if (endAtRow < 0)
            endAtRow = rows.Length;
        
        // Loop through the lines (skipping the first line, as that contains var names like "name", "emotion", etc.
        // Add the appropriate variables to the lists
        for(int i = startAtRow; i < endAtRow; i++)
        {
            // split rows further by dividing them by 'tab' space, since TextAsset was originally a .tsv
            var lines = rows[i].Split('\t');

            // the first column should be the name of the speaker
            // if blank, use the NPC's name
            if (string.IsNullOrEmpty(lines[0]))
                newConversation.speakerNames.Add(speaker.npcName);
            else
                newConversation.speakerNames.Add(lines[0]);

            // Next, we had emotional response.
            // TODO: come up with a way to know which character's sprite to add.
            // FOR NOW: just add the NPC's image
            if (string.IsNullOrEmpty(lines[1]))
                newConversation.speakerEmotions.Add(speaker.neutralSpr);
            else
            {
                switch (lines[1])
                {
                    case "HAPPY":
                        newConversation.speakerEmotions.Add(speaker.happySpr);
                        break;
                    case "NEUTRAL":
                        newConversation.speakerEmotions.Add(speaker.neutralSpr);
                        break;
					case "SAD":
						newConversation.speakerEmotions.Add(speaker.sadSpr);
						break;
					case "ANGRY":
						newConversation.speakerEmotions.Add(speaker.angrySpr);
						break;
                }
            }

            // Finally, adding the dialogue. This should never be null so no need to check
            newConversation.dialogueConversation.Add(lines[2]);

            // Now we'll check if the dialogue line requires a response
            // If this column is empty, just ignore. We're done with this row
            if (lines.Length < 4 || string.IsNullOrEmpty(lines[3]))// || !lines[3].Contains("["))
                continue;
            
            // If we've reached this point then it means we either have responses to choose,
            // or there's a function that needs to be performed at the end of this conversation
            // Let's see if it's just a function. If it is, then we just set the current
            // conversation action and continue
            // To check, see if our special character "[" is there. If there is, then there's a response.
            // If not, then it's just the action
            if(!lines[3].Contains("["))
            {
                var newAction = Regex.Match(lines[3], @"\w+").Value;
                newConversation.actionName = newAction;
                continue;
            }

            var lineCounter = i + 1; // start on the next line

            newConversation.responses = new List<Response>();

            // Otherwise, we need to figure out the responses and store their corresponding dialogue elsewhere
            // First separate the responses (separating character is _)
            var responses = lines[3].Split('_');
            
            // now we need to make sure we split the actual text response with 
            // the marker that determines how many lines the dialogue that corresponds with the response is
            for(int j = 0; j < responses.Length; j++)
            {
                var newResponse = new Response();

                // responses[j] = text[number, function]

                var bracketIndex = responses[j].IndexOf('[');
                var actualResponse = responses[j].Substring(0, bracketIndex);
                newResponse.playerResponse = actualResponse;

                var commaIndex = responses[j].IndexOf(',');

                string numOfLinesStr = "";
                string functionStr = "";
                print("comma index: " + commaIndex);
                if (commaIndex >= 0)
                {
                    numOfLinesStr = responses[j].Substring(bracketIndex, commaIndex);
                    functionStr = responses[j].Substring(commaIndex);
                }
                // if there's no comma, find out if it's a number, if it is, then set num of lines
                else if (Regex.Match(responses[j], @"\d+").Success)
                    numOfLinesStr = responses[j].Substring(bracketIndex);
                // otherwise, it's text for a function
                else
                    functionStr = responses[j].Substring(bracketIndex);
                // numOfLinesStr = [number,
                // functionStr = , function]
                print("lines str: " + numOfLinesStr);
                print("function str: " + functionStr);

                // get the numeric values from numOfLines
                // Regex -- Regular Expression
                // \d+ should return any numeric values found within the string
                var resultStr = Regex.Match(numOfLinesStr, @"\d+").Value;

                // parse string to int
                int numOfLines;
                int.TryParse(resultStr, out numOfLines);
                print("num of lines: " + numOfLines);
                
                // create a new conversation that will be printed if this response it chosen
                // search through the current text file, except instead of starting at the top and going till the end,
                // start at the row in the text file where the response dialogue takes place,
                // and end after the appropriate amount of lines listed (numOfLines)
                newResponse.conversation = DecipherConversation(currentTextAsset.text, lineCounter, lineCounter + numOfLines);
                
                // set the conversation's action if we have one to set
                if(!string.IsNullOrEmpty(functionStr))
                {
                    // get any alphanumeric values from functionStr to get function name
                    // Regex -- Regular Expression
                    // \w+ should return any alphanumeric values found within the string
                    var newAction = Regex.Match(functionStr, @"\w+").Value;
                    newResponse.conversation.actionName = newAction;

                }

                // increase the line counter for the next response
                lineCounter += numOfLines;
                
                // add the new response obj to our new conversation
                newConversation.responses.Add(newResponse);
            }

            // once we've determined there is a response, we don't want to go any further
            break;
        }

        return newConversation;
    }

    void PrintConversation()
    {
        // if the iterator is greater than the amount of dialogue that needs to be said,
        if (conversationIterator >= currentConversation.dialogueConversation.Count)
        {
            //// check to see if there are any responses to give
            //if (currentConversation.IsThereResponse())
            //{
            //    GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.DialogueResponseMenu);
            //}

            //// End the conversation if there is no options for responses
            //else
            //{
                conversationIterator = 0;
                speaker.EndDialogue(currentConversation);

                // pop dialogue menu
                GameControl.UIManager.PopMenu();

                return;
            //}
        }
        currentSpeakerName = currentConversation.GetSpeakerName(conversationIterator);
        currentSpeakerSprite = currentConversation.GetSpeakerEmotion(conversationIterator);
        currentDialogueText = currentConversation.GetDialogueConversation(conversationIterator);
        
        dialogueMenu.SetText(currentSpeakerName, currentDialogueText, currentSpeakerSprite);

        conversationIterator++;

    }    

    void CheckForResponseMenu()
    {
        // check to see if there are any responses to give
        if (conversationIterator >= currentConversation.dialogueConversation.Count
            && currentConversation.IsThereResponse())
        {
            GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.DialogueResponseMenu);
            var responseMenu = GameControl.UIManager.FindMenu(GameControl.UIManager.uiDatabase.DialogueResponseMenu).GetComponent<UI.DialogueResponseMenu>();
            responseMenu.SetResponses(currentConversation.responses);                
            responseMenu.OnStartNextConversation.AddListener(NextConversation);
        }
    }
    
    void NextConversation(Conversation nextConvo)
    {
        print("next conversation");
        //if (conversationIterator <= 0) return;
        //responseMenu.OnStartNextConversation -= NextConversation;
        currentConversation = nextConvo;
        conversationIterator = 0;        
        PrintConversation();
    }
}
