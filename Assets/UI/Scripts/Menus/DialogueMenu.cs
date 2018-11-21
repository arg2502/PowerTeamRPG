namespace UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DialogueMenu : Menu
    {
        public Text speakerText;
        public Text dialogueText;
        public Image portraitImage;
        public Button continueButton; // invisible
        public RectMask2D textMask;

        string dialogueStr; // the full string that dialogueText will print out
        float typingSpeed = 0.01f;
        Action nextDialogue; // function that occurs when Continue button is pressed, set in Dialogue.cs
        bool readyForNextDialogue = true;
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
        public void SetText(string speaker, string dialogue, Sprite portrait)
        {
            speakerText.text = speaker;
            dialogueStr = dialogue;
            portraitImage.sprite = portrait;

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
            for(int i = 0; i < dialogueStr.Length; i++)
            {
                dialogueText.text += dialogueStr[i];
                CheckIfTextOutOfBounds();
                yield return new WaitForSeconds(typingSpeed);
            }
            EndTyping();
        }

        void CheckIfTextOutOfBounds()
        {
            if(dialogueText.preferredHeight > currentYBoundary)
            {
                //print("OUT OF BOUNDS");                
                var yPos = dialogueText.preferredHeight - currentYBoundary;
                dialogueText.transform.Translate(0, yPos, 0);//localPosition += new Vector3(0, yPos);
                currentYBoundary += yPos;
                print("yBound: " + currentYBoundary + ", preferred height: " + dialogueText.preferredHeight);
            }
        }

        /// <summary>
        /// Stops typing coroutine, sets text to full text, and sets ready for next line of dialogue
        /// </summary>
        void EndTyping()
        {
            StopAllCoroutines();
            dialogueText.text = dialogueStr;
            CheckIfTextOutOfBounds();
            readyForNextDialogue = true;
        }
    }
}