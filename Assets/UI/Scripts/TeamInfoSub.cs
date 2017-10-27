namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System;

    public class TeamInfoSub : Menu
    {
        public Button jethroButton, coleButton, eleanorButton, julietteButton;

        protected override void AddListeners()
        {
            jethroButton.onClick.AddListener(OnJethro);
            coleButton.onClick.AddListener(OnCole);
            eleanorButton.onClick.AddListener(OnEleanor);
            julietteButton.onClick.AddListener(OnJuliette);
        }
        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons.Add(jethroButton);
            listOfButtons.Add(coleButton);
            listOfButtons.Add(eleanorButton);
            listOfButtons.Add(julietteButton);
        }
        public override Button AssignFirstButton()
        {
            return jethroButton;
        }

        private void OnJethro()
        {
            // open HeroInfoSub but with Jethro stats
        }

        private void OnCole()
        {
            // open HeroInfoSub but with Cole stats
        }

        private void OnEleanor()
        {
            // open HeroInfoSub but with Eleanor stats
        }

        private void OnJuliette()
        {
            // open HeroInfoSub but with Juliette stats
        }
    }
}