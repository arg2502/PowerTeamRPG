namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class AttackSub : Menu
    {
        public Button strike, skills, spells;
        BattleManager battleManager;

        public override void Init()
        {
            base.Init();
            battleManager = FindObjectOfType<BattleManager>();
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { strike, skills, spells };
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            strike.onClick.AddListener(OnStrike);
            skills.onClick.AddListener(OnSkills);
            spells.onClick.AddListener(OnSpells);
        }
        public override Button AssignRootButton()
        {
            return strike;
        }
        public override void Refocus()
        {
            base.Refocus();
            uiManager.ShowAllMenus();
        }
        void OnStrike()
        {
            //print("Whoa, there. This function isn't done yet, sonny.");
            uiManager.HideAllMenus();
            battleManager.DetermineTargetType("Strike");
            PushTargetMenu();
        }
        void OnSkills()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnSpells()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }

        void PushTargetMenu()
        {
            if (battleManager.IsTargetEnemy)
                PushEnemyTargetMenu();
            else
                PushHeroTargetMenu();
        }

        void PushHeroTargetMenu()
        {
            uiManager.PushMenu(uiDatabase.HeroTargetMenu);
        }
        void PushEnemyTargetMenu()
        {
            uiManager.PushMenu(uiDatabase.EnemyTargetMenu);
        }
    }
}