namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class BattleMenu : Menu
    {
        public Button attack, block, items, flee;
        BattleManager battleManager;
        public Image dimmer;

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            descriptionText = battleManager.DescriptionText;
            base.Init();
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { attack, block, items, flee };
        }
        protected override void AddListeners()
        {
            base.AddListeners();

            attack.onClick.AddListener(OnAttack);
            block.onClick.AddListener(OnBlock);
            items.onClick.AddListener(OnItems);
            flee.onClick.AddListener(OnFlee);
        }

        public override Button AssignRootButton()
        {
            return attack;
        }

        public override void TurnOnMenu()
        {
            rootButton = AssignRootButton();
            base.TurnOnMenu();

            dimmer.gameObject.SetActive(false);
            //battleManager.battleUI.transform.SetAsLastSibling(); // <- ugly line

            CheckForItems();
        }

        public override void Refocus()
        {
            base.Refocus();
            dimmer.gameObject.SetActive(false);
            battleManager.ShowAllShortCardsExceptCurrent();
            CheckForItems();
        }

        void OnAttack()
        {
            dimmer.gameObject.SetActive(true);
            uiManager.PushMenu(uiDatabase.AttackSub, this);
        }
        void OnBlock()
        {
            uiManager.HideAllMenus();
            battleManager.DetermineTargetType("Block");

        }
        void OnItems()
        {
            //print("Whoa, there. This function isn't done yet, sonny.");
            dimmer.gameObject.SetActive(true);
            battleManager.SetMenuState(MenuState.ITEMS);
            uiManager.PushMenu(uiDatabase.ListSub, this);
        }
        void OnFlee()
        {
            // flee successful
            // end the battle
            if (battleManager.CalcFlee())
            {
                uiManager.HideAllMenus();
                battleManager.StartFlee();
            }
            // flee failed
            // skip to Attack state
            else
            {
                //print("...flee failed...");
                uiManager.HideAllMenus();
                battleManager.FleeFailed();
            }
        }    
        
        void CheckForItems()
        {
            if (DoWeHaveItems())
                items.interactable = true;
            else
                items.interactable = false;
            SetButtonNavigation();
        }    

        bool DoWeHaveItems()
        {
            // first check, if it's 0, we obviously don't have any items
            if (gameControl.consumables.Count <= 0)
                return false;
            
            // next check, let's make sure we have some items that are available. If not, then as far as the menu's concerned, we don't have items            
            foreach(var obj in gameControl.consumables)
            {
                var item = obj.GetComponent<ConsumableItem>();
                if (item.Available)
                    return true;
            }

            // if we've reached this point, then that means we have not been able to find an item that's available
            return false;

        }
    }
}