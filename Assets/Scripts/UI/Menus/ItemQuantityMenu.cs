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

        ShopKeeper currentShopkeeper;

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
                priceValue = currentShopkeeper.GetItemPrice(itemToSell);
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

        public void SetShopkeeper(ShopKeeper skd)
        {
            currentShopkeeper = skd;
        }

        void BuyItem()
        {   
            gameControl.AddItem(itemToBuy, quantityNum);
            gameControl.totalGold -= (quantityNum * priceValue);
            uiManager.PopMenu();
        }

        void SellItem()
        {
            gameControl.RemoveItem(itemToSell, quantityNum);
            gameControl.totalGold += (quantityNum * priceValue);
            uiManager.PopMenu();
        }

        int GetMaxAmount()
        {
            return gameControl.totalGold / priceValue;
        }

        private new void Update()
        {
            base.Update();
            quantity.text = quantityNum.ToString();
            price.text = priceValue.ToString();
            totalPrice.text = (quantityNum * priceValue).ToString();

            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
            {
                // if buying, only increase until we can't afford it
                // if selling, only increase until we run out
                if ((currentPurchaseState == PurchaseState.BUY && quantityNum < GetMaxAmount())
                    || (currentPurchaseState == PurchaseState.SELL && quantityNum < itemToSell.Remaining))
                {
                    quantityNum++;
                }
                else
                {
                    quantityNum = 1;
                }
            }
            else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
            {
                if (quantityNum > 1)
                    quantityNum--;
                else
                {
                    if (currentPurchaseState == PurchaseState.BUY)
                        quantityNum = GetMaxAmount();
                    else
                        quantityNum = itemToSell.quantity;
                }
            }

        }

    }
}