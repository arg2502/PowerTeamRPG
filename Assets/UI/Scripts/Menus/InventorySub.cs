namespace UI
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using System;

    public class InventorySub : SubMenu
    {
        public Button consumablesButton, weaponsButton, equipmentButton, keyItemsButton;
        
        protected override void AddButtons()
        {
            listOfButtons = new List<Button>() { consumablesButton, weaponsButton, equipmentButton, keyItemsButton };
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            consumablesButton.onClick.AddListener(OnConsumables);
            weaponsButton.onClick.AddListener(OnWeapons);
            equipmentButton.onClick.AddListener(OnEquipment);
            keyItemsButton.onClick.AddListener(OnKeyItems);
        }

        public override Button AssignRootButton()
        {
            return consumablesButton;
        }

        private void OnConsumables()
        {
            // open inventory with focus on consumables
            uiManager.PushMenu(uiDatabase.InventoryMenu);
        }

        private void OnWeapons()
        {
            // open inventory with focus on weapons
            uiManager.PushMenu(uiDatabase.InventoryMenu);
        }

        private void OnEquipment()
        {
            // open inventory with focus on equipment
            uiManager.PushMenu(uiDatabase.InventoryMenu);        
        }

        private void OnKeyItems()
        {
            // open inventory with focus on key items
            uiManager.PushMenu(uiDatabase.InventoryMenu);        
        }        
    }
}