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
        string dialogueStr; // the full string that dialogueText will print out
        public Image portraitImage;
        float typingSpeed = 0.01f;
        public Button continueButton; // invisible
        Action nextDialogue; // function that occurs when Continue button is pressed, set in Dialogue.cs
        bool readyForNextDialogue = true;

        public override void TurnOnMenu()
        {
            RootButton = AssignRootButton();
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
                yield return new WaitForSeconds(typingSpeed);
            }
            EndTyping();
        }

        /// <summary>
        /// Stops typing coroutine, sets text to full text, and sets ready for next line of dialogue
        /// </summary>
        void EndTyping()
        {
            StopAllCoroutines();
            dialogueText.text = dialogueStr;
            readyForNextDialogue = true;
        }
    }
}