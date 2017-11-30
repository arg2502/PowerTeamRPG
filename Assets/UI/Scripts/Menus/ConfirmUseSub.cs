namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class ConfirmUseSub : Menu
    {
        public Button jethro, cole, eleanor, juliette;

        public override void Init()
        {
            base.Init();
            
        }
        protected override void AddButtons()
        {
            base.AddButtons();

            listOfButtons = new List<Button>() { jethro, cole, eleanor, juliette };
        }
        public override Button AssignRootButton()
        {
            return jethro;
        }
    }
}