namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class BeggarMenu : Menu
    {
        public Text quantity;
        public Image desArrow;
        public Image ascArrow;
        public Button button;
        public NPCBeggar beggar;

        int quantityNum;
        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { button };
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        public override Button AssignRootButton()
        {
            return button;
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            quantityNum = gameControl.totalGold > 0 ? 1 : 0;
        }

        void OnClick()
        {            
            gameControl.totalGold -= quantityNum;
            beggar.SetGold(quantityNum);
//            uiManager.PopMenu();
        }

        private new void Update()
        {
            base.Update();
            quantity.text = quantityNum.ToString();

            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
            {
                // if buying, only increase until we can't afford it
                // if selling, only increase until we run out
                if (quantityNum < gameControl.totalGold)
                    quantityNum++;
                else
                    quantityNum = 1;
            }
            else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
            {
                if (quantityNum > 1)
                    quantityNum--;
                else
                    quantityNum = gameControl.totalGold;
            }

        }
    }
}