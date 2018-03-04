namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class BattleMenu : Menu
    {
        public Button attack, block, items, flee;

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

        void OnAttack()
        {
            //print("Whoa, there. This function isn't done yet, sonny.");
            uiManager.PushMenu(uiDatabase.AttackSub);
        }
        void OnBlock()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnItems()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnFlee()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        
        
    }
}