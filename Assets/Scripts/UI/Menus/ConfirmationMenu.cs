namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ConfirmationMenu : Menu
    {
        public Text specificText;
        public Button yesButton, noButton;
        public Action yesAction, noAction;
        
        public override void TurnOnMenu()
        {
            RootButton = AssignRootButton();
            base.TurnOnMenu();
        }
        public override Button AssignRootButton()
        {
            return yesButton;
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { yesButton, noButton };
        }
        protected override void AddListeners()
        {
            base.AddListeners();

            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();

            yesButton.onClick.AddListener(CloseMenu);
            noButton.onClick.AddListener(CloseMenu);

            yesButton.onClick.AddListener(yesAction.Invoke);

            if (noAction != null)
                noButton.onClick.AddListener(noAction.Invoke);

        }

        void CloseMenu()
        {
            uiManager.PopMenu();
        }
    }
}