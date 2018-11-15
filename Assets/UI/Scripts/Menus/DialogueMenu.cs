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
        public Button continueButton; // invisible
        Action Continue; // function that occurs when Continue button is pressed, set in Dialogue.cs

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

            if (Continue != null)
                continueButton.onClick.AddListener(Continue.Invoke);
        }
        
        /// <summary>
        /// Set what function occurs when the continue button is pressed. Used outside of this class.
        /// </summary>
        /// <param name="continueAction"></param>
        public void SetContinue(Action continueAction)
        {
            Continue = continueAction;
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
            dialogueText.text = dialogueStr; // TEMP
            portraitImage.sprite = portrait;
        }
    }
}