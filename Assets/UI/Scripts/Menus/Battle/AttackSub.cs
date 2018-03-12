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
        public Image dimmer;

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            base.Init();
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

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();
            dimmer.gameObject.SetActive(false);

            CheckTechniques(); 
        }

        public override void Refocus()
        {
            base.Refocus();
            uiManager.ShowAllMenus();
            dimmer.gameObject.SetActive(false);
        }
        void OnStrike()
        {
            //print("Whoa, there. This function isn't done yet, sonny.");
            uiManager.HideAllMenus();
            battleManager.DetermineTargetType("Strike");
            uiManager.PushMenu(uiDatabase.TargetMenu);
        }
        void OnSkills()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnSpells()
        {
            //print("Whoa, there. This function isn't done yet, sonny.");
            uiManager.PushMenu(uiDatabase.ListSub);            
        }
        
        /// <summary>
        /// Determines whether or not to disable the current denigens skills and/or spells buttons
        /// </summary>
        void CheckTechniques()
        {
            var hero = battleManager.CurrentHero;
            
            // sets buttons to false if lists are empty
            skills.interactable = hero.SkillsList.Count > 0;
            spells.interactable = hero.SpellsList.Count > 0;

            // reset buttons
            SetButtonNavigation();
        }
    }
}