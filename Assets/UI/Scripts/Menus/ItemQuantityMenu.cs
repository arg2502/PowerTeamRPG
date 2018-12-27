namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemQuantityMenu : Menu
    {
        public Text quantity;
        public Text price;
        public Text totalPrice;

        public Image desArrow;
        public Image ascArrow;

        public Button button;

        ScriptableItem itemToBuy;

        int quantityNum;
        
        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { button };
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            button.onClick.AddListener(OnClick);
        }

        public override Button AssignRootButton()
        {
            return button;
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            quantityNum = 1;
        }

        void OnClick()
        {
            //BuyItem();
            uiManager.PushConfirmationMenu("Purchase " + quantityNum.ToString() + " " + itemToBuy.name + "?", BuyItem);
        }

        public void SetItem(ScriptableItem _item)
        {
            itemToBuy = _item;
            Refresh();
        }

        void BuyItem()
        {   
            GameControl.control.AddItem(itemToBuy, quantityNum);
            uiManager.PopMenu();
            uiManager.PopMenu();
        }

        private void Update()
        {
            base.Update();
            quantity.text = quantityNum.ToString();
            price.text = itemToBuy.value.ToString();
            totalPrice.text = (quantityNum * itemToBuy.value).ToString();

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                quantityNum++;
                //UpdateArrowStates();
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (quantityNum > 1)
                    quantityNum--;
                //UpdateArrowStates();
            }
        }
    }
}