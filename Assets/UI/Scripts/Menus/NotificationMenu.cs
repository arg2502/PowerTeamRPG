namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class NotificationMenu : Menu
    {
        public Text messageText;
        public Button okButton;

        public override void TurnOnMenu()
        {
            RootButton = AssignRootButton();
            base.TurnOnMenu();
        }
        public override Button AssignRootButton()
        {
            return okButton;
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { okButton };
        }
        protected override void AddListeners()
        {
            base.AddListeners();

            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(CloseMenu);
        }

        void CloseMenu()
        {
            uiManager.PopMenu();
        }
    }
}