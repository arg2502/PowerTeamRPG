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
            base.TurnOnMenu();

            dimmer.gameObject.SetActive(false);
            //battleManager.battleUI.transform.SetAsLastSibling(); // <- ugly line
        }

        public override void Refocus()
        {
            base.Refocus();
            dimmer.gameObject.SetActive(false);
        }

        void OnAttack()
        {
            dimmer.gameObject.SetActive(true);
            uiManager.PushMenu(uiDatabase.AttackSub);
        }
        void OnBlock()
        {
            uiManager.HideAllMenus();
            battleManager.DetermineTargetType("Block");

        }
        void OnItems()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnFlee()
        {
            // flee successful
            // end the battle
            if (battleManager.CalcFlee())
            {
                uiManager.HideAllMenus();
                battleManager.FleeBattle();
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
    }
}