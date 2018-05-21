namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class TeamInfoSub : SubMenu
    {
        public Button jethroButton, coleButton, eleanorButton, joulietteButton;
        internal int currentHero;
        internal Button currentButton; // to know the Y position

        protected override void AddListeners()
        {
            base.AddListeners();

            jethroButton.onClick.AddListener(OnJethro);
            coleButton.onClick.AddListener(OnCole);
            eleanorButton.onClick.AddListener(OnEleanor);
            joulietteButton.onClick.AddListener(OnJouliette);
        }
        protected override void AddButtons()
        {
            listOfButtons = new List<Button>() { jethroButton, coleButton, eleanorButton, joulietteButton };
        }
        public override Button AssignRootButton()
        {
            return jethroButton;
        }

        private void OnJethro()
        {
            // open HeroInfoSub but with Jethro stats
            currentHero = 0;
            currentButton = jethroButton;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        private void OnCole()
        {
            // open HeroInfoSub but with Cole stats
            currentHero = 1;
            currentButton = coleButton;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        private void OnEleanor()
        {
            // open HeroInfoSub but with Eleanor stats
            currentHero = 2;
            currentButton = eleanorButton;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        private void OnJouliette()
        {
            // open HeroInfoSub but with Juliette stats
            currentHero = 3;
            currentButton = joulietteButton;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        // TODO:
        // When this menu system gets integrated into the game,
        // create a function to only activate the buttons 
        // for the team members you currently have

    }
}