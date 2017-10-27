namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections;
    
    public class PauseMenu : Menu
    {
        public Button exitMenuButton, teamInfoButton, inventoryButton, saveButton, loadButton;

        protected override void AddListeners()
        {
            exitMenuButton.onClick.AddListener(OnExit);
            teamInfoButton.onClick.AddListener(OnTeamInfo);
            inventoryButton.onClick.AddListener(OnInventory);
            saveButton.onClick.AddListener(OnSave);
            loadButton.onClick.AddListener(OnLoad);
        }
        protected override void AddButtons()
        {
            base.AddButtons();

            listOfButtons.Add(exitMenuButton);
            listOfButtons.Add(teamInfoButton);
            listOfButtons.Add(inventoryButton);
            listOfButtons.Add(saveButton);
            listOfButtons.Add(loadButton);
        }

        public override Button AssignFirstButton() { return exitMenuButton; }

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
            //uiManager.PushMenu(uiDatabase.InventorySub);
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
