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
        }

        Action Continue;
        //{
        //    print("continue has been pressed!");
        //}

        public void SetContinue(Action continueAction)
        {
            Continue = continueAction;
            continueButton.onClick.AddListener(Continue.Invoke);
        }

        public void SetText(string speaker, string dialogue, Sprite portrait)
        {
            speakerText.text = speaker;
            dialogueStr = dialogue;
            dialogueText.text = dialogueStr; // TEMP
            portraitImage.sprite = portrait;
        }
    }
}