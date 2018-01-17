namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class HeroInfoSub : SubMenu
    {
        public Button skillTree, statPoints;
        
        protected override void AddListeners()
        {
            base.AddListeners();
            skillTree.onClick.AddListener(OnSkillTree);
            statPoints.onClick.AddListener(OnStatPoints);
        }
        protected override void AddButtons()
        {
            listOfButtons = new List<Button>() { skillTree, statPoints };            
        }
        public override Button AssignRootButton()
        {
            return skillTree;
        }
        void OnSkillTree()
        {
            // open skill tree for this character
            uiManager.PushMenu(uiDatabase.SkillTreeMenu);
        }
        void OnStatPoints()
        {
            // open stat points for this character
        }

    }
}