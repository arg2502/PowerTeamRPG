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

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            RootButton = AssignRootButton();
            SetSelectedObjectToRoot();
        }

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
            uiManager.PushMenu(uiDatabase.SaveMenu, this);
        }

        void OnLoad()
        {
            // load the game here
            //GameControl.control.Load();
            uiManager.PushMenu(uiDatabase.LoadMenu, this);
        }
    }
}
