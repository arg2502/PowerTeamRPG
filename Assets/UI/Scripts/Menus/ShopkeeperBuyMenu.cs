namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class ShopkeeperBuyMenu : Menu
    {
        // text references
        public Image icon;
        public Text title;
        public Text price;
        public Text stats1;
        public Text stats2;
        public Text itemDescription;

        ShopKeeperDialogue currentShopkeeper;
        public GameObject listParent;
        public GameObject itemPrefab;
        float buttonDistance = -50f;

        ScriptableItem currentItemToBuy = null;

        protected override void AddButtons()
        {
            base.AddButtons();

            if (listOfButtons == null)
                listOfButtons = new List<Button>();

            if (currentShopkeeper == null)
                return;

            for (int i = 0; i < currentShopkeeper.shopInventory.Count; i++)
            {
                var item = Instantiate(itemPrefab);
                item.name = currentShopkeeper.shopInventory[i].name;// + "_Button";
                item.transform.SetParent(listParent.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * buttonDistance);
                item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                var button = item.GetComponentInChildren<Button>();
                //var itemInfo = ItemDatabase.GetItem(currentShopkeeper.shopInventory[i].type, item.name); //??
                var itemInfo = ScriptableObject.Instantiate(currentShopkeeper.shopInventory[i]);


                button.GetComponentInChildren<Text>().text = currentShopkeeper.shopInventory[i].name;                
                listOfButtons.Add(button);

                // description
                item.GetComponentInChildren<Description>().description = "<b>" + itemInfo.name + "</b>\n\n" + itemInfo.description;

                var itemSlot = item.GetComponent<ItemSlot>();

                // set item to item slot so it's connected to the button
                itemSlot.SetItem(currentShopkeeper.shopInventory[i]);

                // add listener
                button.onClick.AddListener(OnSelect);
            }
        }

        public override Button AssignRootButton()
        {
            if (listOfButtons.Count > 0)
                return listOfButtons[0];
            else return null;

        }

        public override void Close()
        {
            currentShopkeeper = null;
            base.Close();
        }

        public void FillSlots(ShopKeeperDialogue shopkeeper)
        {
            currentShopkeeper = shopkeeper;
            Refresh();
        }

        private void OnSelect()
        {
            currentItemToBuy = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemSlot>().s_item;

            uiManager.PushMenu(uiDatabase.ItemQuantityMenu);
            var quantityMenu = uiManager.FindMenu(uiDatabase.ItemQuantityMenu).GetComponent<ItemQuantityMenu>();
            quantityMenu.SetItem(currentItemToBuy);
        }


        void UpdateStats(ScriptableItem newItem)
        {
            icon.sprite = newItem.sprite;
            title.text = newItem.name;
            price.text = newItem.value.ToString();
            itemDescription.text = newItem.description;
            
            // Currently we'll display a max of 6 stats -- 3 per stat text
            // if our stats length is 3 or less, we only use the first one            
            stats1.text = "";
            stats2.text = "";
            for(int i = 0; i < newItem.statBoosts.Length; i++)
            {
                if (i < 3)
                    stats1.text += newItem.statBoosts[i].statName + ": +" + newItem.statBoosts[i].boost + "\n";
                else
                    stats2.text += newItem.statBoosts[i].statName + ": +" + newItem.statBoosts[i].boost + "\n";
            }
           
        }

        private new void Update()
        {
            base.Update();
            if (this != uiManager.CurrentMenu) return;

            var chosenItem = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemSlot>().s_item;
            UpdateStats(chosenItem);
        }

    }
}