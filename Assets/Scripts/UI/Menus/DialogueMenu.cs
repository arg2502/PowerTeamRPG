namespace UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class DialogueMenu : Menu
    {
        public Text speakerText;
        public Text dialogueText;
        public Image portraitImage;
        public Button continueButton; // invisible
        public RectMask2D textMask;

        string dialogueStr; // the full string that dialogueText will print out
        float typingSpeed = 0.02f;
        Action nextDialogue; // function that occurs when Continue button is pressed, set in Dialogue.cs

        // testing out event triggers
        bool readyForNextDialogue = true;
        public bool ReadyForNextDialogue
        {
            get { return readyForNextDialogue; }
            set
            {
                readyForNextDialogue = value;

                if(readyForNextDialogue && OnNextDialogue != null)
                {
                    OnNextDialogue.Invoke();
                }
            }
        }
        public UnityEvent OnNextDialogue;


        float currentYBoundary;
        Vector3 origTextPos;

        public override void TurnOnMenu()
        {
            RootButton = AssignRootButton();
            currentYBoundary = textMask.GetComponent<RectTransform>().sizeDelta.y;
            origTextPos = dialogueText.transform.localPosition;
            base.TurnOnMenu();
        }
        public override Button AssignRootButton()
        {
            return continueButton;
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { continueButton };
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            continueButton.onClick.RemoveAllListeners();

            if (nextDialogue != null)
                continueButton.onClick.AddListener(Continue);
        }

        public override void Close()
        {
            dialogueText.transform.localPosition = origTextPos;
            base.Close();
        }

        void Continue()
        {
            // text has finished writing by this point, go to next dialogue
            if (readyForNextDialogue)
            {
                dialogueText.transform.localPosition = origTextPos; // reset to starting pos
                nextDialogue.Invoke();
            }
            // Otherwise, the text is still typing. Press select to skip to the end of the typing
            else
            {
                EndTyping();
            }
        }

        /// <summary>
        /// Set what function occurs when the continue button is pressed. Used outside of this class.
        /// </summary>
        /// <param name="continueAction"></param>
        public void SetContinue(Action continueAction)
        {
            nextDialogue = continueAction;
            AddListeners();
        }

        /// <summary>
        /// Setting the speaker text, dialogue text, and portrait image for each part of conversation.
        /// Set in Dialogue.cs
        /// </summary>
        /// <param name="speaker"></param>
        /// <param name="dialogue"></param>
        /// <param name="portrait"></param>
        public void SetText(string speaker, string dialogue, Sprite portrait, float speed)//, bool isResponse)
        {
            typingSpeed = speed;
            speakerText.text = speaker;
            dialogueStr = dialogue;
            if (portrait != null)
            {
                portraitImage.gameObject.SetActive(true);
                portraitImage.sprite = portrait;
            }
            else
                portraitImage.gameObject.SetActive(false);
            StartCoroutine(TypeDialogue());
        }

        /// <summary>
        /// Coroutine that types out the dialogue one letter at a time based on the typing speed
        /// </summary>
        /// <returns></returns>
        IEnumerator TypeDialogue()
        {
            readyForNextDialogue = false;
            dialogueText.text = "";
            string transparentTagStart = "<color=#00000000>";
            string transparentColorTagEnd = "</color>";
            bool insideRichText = false;
            int insideTagLayer = 0; // layers of tags -- ex: <b><color=white>words wrodwewtyty</color></b> --would be 2 layers of tags
            for(int i = 0; i < dialogueStr.Length; i++)
            {
                // check for any rich text tags, we don't wanna place the transparent color tag within that   
                if (dialogueStr[i] == '<')
                {
                    insideRichText = true;
                    if (dialogueStr[i + 1] != '/')
                        insideTagLayer++;
                    continue;
                }
                else if (dialogueStr[i] == '>')
                {
                    insideRichText = false;
                    continue;
                }
                else if (dialogueStr[i] == '/')
                {
                    insideTagLayer--;
                    if (insideTagLayer < 0) insideTagLayer = 0;
                    continue;
                }
               
                // check for any special characters ( "{" for speed )
                // we want to wait the desired number of seconds as well as remove the special characters from the string
                if(dialogueStr[i] == '{')
                {
                    // find end of tag
                    int tagStart = i;
                    int tagEnd = dialogueStr.IndexOf('}', i + 1);
                    int strLength = (tagEnd - tagStart);
                    string secondsStr = dialogueStr.Substring(tagStart + 1, strLength - 1);
                    
                    float secondsToWait;
                    if (!float.TryParse(secondsStr, out secondsToWait))
                        secondsToWait = 1;

                    dialogueStr = dialogueStr.Remove(i, strLength + 1); // {#.##}
                    yield return new WaitForSecondsRealtime(secondsToWait);
                }

                if (!insideRichText)
                {
                    dialogueText.text += dialogueStr[i];

                    if (insideTagLayer == 0)
                        dialogueText.text = 
                            dialogueStr.Substring(0, i+1) +
                            transparentTagStart + 
                            dialogueStr.Substring(i) + 
                            transparentColorTagEnd;
                    else
                    {
                        // find where the tag ends
                        int indexOfEndTagStart = dialogueStr.IndexOf('<', i);
                        int indexOfEndTagEnd = indexOfEndTagStart;
                        int tempStart = indexOfEndTagStart;
                        
                        for(int j = 0; j < insideTagLayer; j++)
                        {
                            indexOfEndTagEnd = dialogueStr.IndexOf('>', tempStart + 1);
                            tempStart = indexOfEndTagEnd;                            
                        }

                        var lengthUntilEndTagStart = indexOfEndTagStart - i;
                        var lengthOfTag = indexOfEndTagEnd - indexOfEndTagStart;
                        dialogueText.text = 
                            dialogueStr.Substring(0, i)                           // currently visible
                            + transparentTagStart                                                // start transparent
                            + dialogueStr.Substring(i, lengthUntilEndTagStart)                   // hidden string until end tag
                            + transparentColorTagEnd                                                  // close transparent tag
                            + dialogueStr.Substring(i + lengthUntilEndTagStart, lengthOfTag + 1) // skip closing tag
                            + transparentTagStart                                                // open transparent again
                            + dialogueStr.Substring(indexOfEndTagEnd + 1)                        // rest of string
                            + transparentColorTagEnd;                                                 // close transparent tag
                    }
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            EndTyping();
        }

        void CheckIfTextOutOfBounds()
        {
            if(dialogueText.preferredHeight > currentYBoundary)
            {           
                var yPos = dialogueText.preferredHeight - currentYBoundary;
                dialogueText.transform.localPosition += new Vector3(0, yPos);
                currentYBoundary += yPos;
            }
        }

        /// <summary>
        /// Stops typing coroutine, sets text to full text, and sets ready for next line of dialogue
        /// </summary>
        void EndTyping()
        {
            StopAllCoroutines();
            dialogueText.text = dialogueStr;
            ReadyForNextDialogue = true; // invokes event
        }
    }
}