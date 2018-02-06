namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class TeamInfoSub : SubMenu
    {
        public Button jethroButton, coleButton, eleanorButton, julietteButton;
        internal int currentHero;

        protected override void AddListeners()
        {
            base.AddListeners();

            jethroButton.onClick.AddListener(OnJethro);
            coleButton.onClick.AddListener(OnCole);
            eleanorButton.onClick.AddListener(OnEleanor);
            julietteButton.onClick.AddListener(OnJuliette);
        }
        protected override void AddButtons()
        {
            listOfButtons = new List<Button>() { jethroButton, coleButton, eleanorButton, julietteButton };
        }
        public override Button AssignRootButton()
        {
            return jethroButton;
        }

        private void OnJethro()
        {
            // open HeroInfoSub but with Jethro stats
            currentHero = 0;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        private void OnCole()
        {
            // open HeroInfoSub but with Cole stats
            currentHero = 1;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        private void OnEleanor()
        {
            // open HeroInfoSub but with Eleanor stats
            currentHero = 2;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        private void OnJuliette()
        {
            // open HeroInfoSub but with Juliette stats
            currentHero = 3;
            uiManager.PushMenu(uiDatabase.HeroInfoSub, this);
        }

        // TODO:
        // When this menu system gets integrated into the game,
        // create a function to only activate the buttons 
        // for the team members you currently have

    }
}