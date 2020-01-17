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
            RootButton = consumablesButton;
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.Consumables;
            uiManager.PushMenu(uiDatabase.InventoryMenu);
        }

        private void OnWeapons()
        {
            // open inventory with focus on weapons
            RootButton = weaponsButton;
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.Armor;
            uiManager.PushMenu(uiDatabase.InventoryMenu);
        }

        private void OnEquipment()
        {
            // open inventory with focus on equipment
            RootButton = equipmentButton;
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.Augments;
            uiManager.PushMenu(uiDatabase.InventoryMenu);        
        }

        private void OnKeyItems()
        {
            // open inventory with focus on key items
            RootButton = keyItemsButton;
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.KeyItems;
            uiManager.PushMenu(uiDatabase.InventoryMenu);        
        }        
    }
}