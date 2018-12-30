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
        
        // scrolling objects
        Rect itemSlotsWorld;
        Vector3[] objectCorners;
        float startTime = 0;
        float lerpTime = 10f;
        bool moving;
        public Image upScroll, downScroll;

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
                itemSlot.SetItem(itemInfo);

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
            // since this menu needs to be used for multiple shopkeepers,
            // we can't hold onto all the scriptable objects
            // for now, let's just destroy all the items and the buttons
            // maybe at some point we can figure out a way to not destroy if it's the same shopkeeper
            for(int i = 0; i < listOfButtons.Count; i++)
            {
                Destroy(listOfButtons[i].GetComponentInParent<ItemSlot>().s_item);
                Destroy(listOfButtons[i]);
            }
            listOfButtons.Clear();

            base.Close();

            currentShopkeeper.StartDialogue();
            currentShopkeeper = null;
        }

        public void FillSlots(ShopKeeperDialogue shopkeeper)
        {
            currentShopkeeper = shopkeeper;
            Refresh();
        }

        private void OnSelect()
        {
            //currentItemToBuy = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemSlot>().s_item;

            uiManager.PushMenu(uiDatabase.ItemQuantityMenu, this);
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
            
            if (EventSystem.current.currentSelectedGameObject == null) return;

            var itemSlot = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemSlot>();

            if (itemSlot)
                currentItemToBuy = itemSlot.s_item;
            UpdateStats(currentItemToBuy);


            if (CheckIfOffScreen(EventSystem.current.currentSelectedGameObject))
                OutsideOfView(EventSystem.current.currentSelectedGameObject);
        }


        bool CheckIfOffScreen(GameObject buttonObj)
        {
            // find the Item Slots Container's rect in world coordinates
            // to see if the button is within it's view or not
            var screenCorners = new Vector3[4];
            var itemSlotsRectTransform = listParent.transform.parent.GetComponent<RectTransform>();
            itemSlotsRectTransform.GetWorldCorners(screenCorners);

            // take the world coordinate vectors and create a new rect from them
            var width = Mathf.Abs(screenCorners[2].x - screenCorners[0].x);
            var height = Mathf.Abs(screenCorners[2].y - screenCorners[0].y);
            var size = new Vector2(width, height);
            itemSlotsWorld = new Rect(screenCorners[0], size);

            // find the buttons world coordinates
            objectCorners = new Vector3[4];
            var contentTransform = buttonObj.GetComponent<RectTransform>();
            contentTransform.GetWorldCorners(objectCorners);

            // if any corner of the object is on screen, break out of the method
            foreach (Vector3 corner in objectCorners)
            {
                if (itemSlotsWorld.Contains(corner))
                    return false;
            }

            return true;
        }
        
        void OutsideOfView(GameObject buttonObj)
        {
            // if we reach this point, that means we are off screen
            // bring it on screen
            // find the button
            int ourButtonListPosition = 0;
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                if (buttonObj == listOfButtons[i].gameObject)
                {
                    ourButtonListPosition = i;
                    break;
                }
            }


            // if below screen, set the button above it as the desired position
            Vector3 desiredPosition = buttonObj.transform.position;
            
            // vertical movement
            if (objectCorners[0].y < itemSlotsWorld.yMin)
            {
                if (ourButtonListPosition <= 0) return;
                desiredPosition = listOfButtons[ourButtonListPosition - 1].transform.position;                
            }
            else if (objectCorners[2].y > itemSlotsWorld.yMax)
            {
                if (ourButtonListPosition >= listOfButtons.Count - 1) return;
                desiredPosition = listOfButtons[ourButtonListPosition + 1].transform.position;                
            }            

            // determine distance and new position based on difference in either x or y
            float distance;
            Vector2 newPosition;
            
            distance = desiredPosition.y - buttonObj.transform.position.y;
            newPosition = new Vector2(listParent.transform.position.x, listParent.transform.position.y + distance);
            
            if (!moving)
            {
                moving = true;
                startTime = Time.time;
                StartCoroutine(MoveButtonOntoScreen(newPosition, lerpTime));
            }
        }

        private IEnumerator MoveButtonOntoScreen(Vector3 newPosition, float lerpTime)
        {
            var original = new Vector3();
            original = listParent.transform.position;
            while ((listParent.transform.position - newPosition).magnitude >= 0.1f)
            {
                listParent.transform.position = Vector2.Lerp(original, newPosition, (Time.time - startTime) * lerpTime);
                yield return null;
            }
            moving = false;

            if (CheckIfOffScreen(EventSystem.current.currentSelectedGameObject))
                OutsideOfView(EventSystem.current.currentSelectedGameObject);
            else
                CheckIfListOffScreen();
        }

        void CheckIfListOffScreen()
        {
            // if the first item in the list is off screen, then turn on the up scroll notifier
            if (CheckIfOffScreen(listOfButtons[0].gameObject))
                upScroll.gameObject.SetActive(true);
            else
                upScroll.gameObject.SetActive(false);

            // if the last item in the list is off screen, then turn on the down scroll notifier
            if (CheckIfOffScreen(listOfButtons[listOfButtons.Count - 1].gameObject))
                downScroll.gameObject.SetActive(true);
            else
                downScroll.gameObject.SetActive(false);
        }

    }
}