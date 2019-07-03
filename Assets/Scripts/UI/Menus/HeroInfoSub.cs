namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class HeroInfoSub : SubMenu
    {
        public Button skillTree, statPoints;
        public GameObject buttonsGroup;
        TeamInfoSub teamInfoSub;

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            teamInfoSub = uiManager.FindMenu(uiDatabase.TeamInfoSub) as TeamInfoSub;

            var yPos = teamInfoSub.currentButton.transform.position.y;
            buttonsGroup.transform.position = new Vector3(buttonsGroup.transform.position.x, yPos, buttonsGroup.transform.position.z);

            SetParentDescriptions();
        }

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

        public override void Refocus()
        {
            base.Refocus();
        }

        void SetParentDescriptions()
        {
            HeroStatDescription parentButton;

            switch (teamInfoSub.currentHero)
            {
                case 0:
                    parentButton = teamInfoSub.jethroButton.GetComponent<HeroStatDescription>();
                    break;
                case 1:
                    parentButton = teamInfoSub.coleButton.GetComponent<HeroStatDescription>();
                    break;
                case 2:
                    parentButton = teamInfoSub.eleanorButton.GetComponent<HeroStatDescription>();
                    break;
                default:
                    parentButton = teamInfoSub.joulietteButton.GetComponent<HeroStatDescription>();
                    break;

            }

            skillTree.GetComponent<Description>().parentButton = parentButton;
            skillTree.GetComponent<Description>().SetDescription();
            statPoints.GetComponent<Description>().parentButton = parentButton;
            statPoints.GetComponent<Description>().SetDescription();
        }

        void OnSkillTree()
        {
            // open skill tree for this character
            uiManager.PushMenu(uiDatabase.SkillTreeMenu, this);
            var skillTree = uiManager.CurrentMenu.GetComponent<SkillTreeMenu>();
            skillTree.SetHero(teamInfoSub.currentHero);
        }
        void OnStatPoints()
        {
            // open stat points for this character
            uiManager.PushMenu(uiDatabase.StatPointsMenu, this);
            var statPoints = uiManager.CurrentMenu.GetComponent<StatPointsMenu>();
            statPoints.SetInitialHero(teamInfoSub.currentHero);
        }

    }
}