namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PauseMenu : Menu
    {
        public Button exitMenuButton, teamInfoButton, inventoryButton, saveButton, loadButton;
        protected override void AddListeners()
        {
            base.AddListeners();

            exitMenuButton.onClick.AddListener(OnExit);
            teamInfoButton.onClick.AddListener(OnTeamInfo);
            inventoryButton.onClick.AddListener(OnInventory);
            saveButton.onClick.AddListener(OnSave);
            loadButton.onClick.AddListener(OnLoad);
        }
        protected override void AddButtons()
        {
            listOfButtons = new List<Button>() { exitMenuButton, teamInfoButton, inventoryButton, saveButton, loadButton };
        }

        public override Button AssignRootButton() { return exitMenuButton; }

        void OnExit()
        {
            uiManager.PopMenu();
        }

        void OnTeamInfo()
        {            
            uiManager.PushMenu(uiDatabase.TeamInfoSub, this);
        }

        void OnInventory()
        {
            uiManager.PushMenu(uiDatabase.InventorySub, this);
        }

        void OnSave()
        {
            // save the game here
        }

        void OnLoad()
        {
            // load the game here
        }
    }
}
