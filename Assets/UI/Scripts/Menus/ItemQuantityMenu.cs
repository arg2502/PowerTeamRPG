namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class ItemQuantityMenu : Menu
    {
        public Text quantity;
        public Text price;
        public Text totalPrice;

        public Image desArrow;
        public Image ascArrow;

        public Button button;

        ScriptableItem itemToBuy;
        InventoryItem itemToSell;

        int quantityNum;
        int priceValue;
        
        enum PurchaseState { BUY, SELL }
        PurchaseState currentPurchaseState;

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

            quantityNum = 1;

            if (currentPurchaseState == PurchaseState.BUY && itemToBuy != null)
                priceValue = itemToBuy.value;
            else if (currentPurchaseState == PurchaseState.SELL && itemToSell != null)
            {
                var typeName = gameControl.CurrentInventory;
                priceValue = ItemDatabase.GetItem(typeName, itemToSell.name).value;
            }            
        }

        void OnClick()
        {
            if (currentPurchaseState == PurchaseState.BUY)
                uiManager.PushConfirmationMenu("Purchase " + quantityNum.ToString() + " " + itemToBuy.name + "?", BuyItem);
            else
                uiManager.PushConfirmationMenu("Sell " + quantityNum.ToString() + " " + itemToSell.name + "?", SellItem);
        }

        public void SetItem(ScriptableItem _item)
        {
            itemToBuy = _item;
            currentPurchaseState = PurchaseState.BUY;

            Refresh();
        }

        public void SetItem(InventoryItem _item)
        {
            itemToSell = _item;
            currentPurchaseState = PurchaseState.SELL;

            Refresh();
        }

        void BuyItem()
        {   
            GameControl.control.AddItem(itemToBuy, quantityNum);
            uiManager.PopMenu();
        }

        void SellItem()
        {
            GameControl.control.RemoveItem(itemToSell, quantityNum);
            uiManager.PopMenu();
        }

        private void Update()
        {
            base.Update();
            quantity.text = quantityNum.ToString();
            price.text = priceValue.ToString();
            totalPrice.text = (quantityNum * priceValue).ToString();

            if (Input.GetKeyUp(gameControl.rightKey))
            {
                quantityNum++;
                //UpdateArrowStates();
            }
            else if (Input.GetKeyUp(gameControl.leftKey))
            {
                if (quantityNum > 1)
                    quantityNum--;
                //UpdateArrowStates();
            }

        }

    }
}