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
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.Consumables;
            uiManager.PushMenu(uiDatabase.InventoryMenu);
        }

        private void OnWeapons()
        {
            // open inventory with focus on weapons
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.Weapons;
            //if (tempControl.UIManager.dictionary_existingMenus.ContainsKey(uiDatabase.InventoryMenu.GetComponent<InventoryMenu>()))
              //  tempControl.UIManager.dictionary_existingMenus[uiDatabase.InventoryMenu.GetComponent<InventoryMenu>()].GetComponent<InventoryMenu>().CreateButtons();
            uiManager.PushMenu(uiDatabase.InventoryMenu);
        }

        private void OnEquipment()
        {
            // open inventory with focus on equipment
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.Equipment;
            uiManager.PushMenu(uiDatabase.InventoryMenu);        
        }

        private void OnKeyItems()
        {
            // open inventory with focus on key items
            GameControl.control.whichInventoryEnum = GameControl.WhichInventory.KeyItems;
            uiManager.PushMenu(uiDatabase.InventoryMenu);        
        }        
    }
}