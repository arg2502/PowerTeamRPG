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
        public Text totalGold;
        public Image skPortrait;
        public Text skSpeaker;
        public Text skComment;

        //ShopKeeperDialogue currentShopkeeper;
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

        public struct ShopkeeperComment
        {
            public Sprite sprite;
            public string comment;
        }
        Dictionary<string, ShopkeeperComment> dictComments;

        protected override void AddButtons()
        {
            base.AddButtons();

            if (listOfButtons == null)
                listOfButtons = new List<Button>();

            //if (gameControl.CurrentShopkeeper == null)
                //return;

            for (int i = 0; i < gameControl.CurrentShopkeeper.shopInventory.Count; i++)
            {
                var item = Instantiate(itemPrefab);
                item.name = gameControl.CurrentShopkeeper.shopInventory[i].name;// + "_Button";
                item.transform.SetParent(listParent.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * buttonDistance);
                item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                var button = item.GetComponentInChildren<Button>();
                //var itemInfo = ItemDatabase.GetItem(currentShopkeeper.shopInventory[i].type, item.name); //??
                var itemInfo = ScriptableObject.Instantiate(gameControl.CurrentShopkeeper.shopInventory[i]);                

                button.GetComponentInChildren<Text>().text = gameControl.CurrentShopkeeper.shopInventory[i].name;                
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

            gameControl.CurrentShopkeeper.StartDialogue();
            //currentShopkeeper = null;
        }

        public override void TurnOnMenu()
        {
            DecipherComments();
            if (listOfButtons.Count <= 0)
            {
                Refresh();
                return;
            }

            upScroll.gameObject.SetActive(false);

            base.TurnOnMenu();
        }

        //public void FillSlots(ShopKeeperDialogue shopkeeper)
        //{
        //    currentShopkeeper = shopkeeper;
        //    Refresh();
        //}

        private void OnSelect()
        {
            // check if we can afford at least one of the item.
            if (currentItemToBuy.value < gameControl.totalGold)
            {
                uiManager.PushMenu(uiDatabase.ItemQuantityMenu, this);
                var quantityMenu = uiManager.FindMenu(uiDatabase.ItemQuantityMenu).GetComponent<ItemQuantityMenu>();
                quantityMenu.SetItem(currentItemToBuy);
            }
            // If not, don't open
            else
            {
                uiManager.PushNotificationMenu("You don't have enough gold for that item.");
            }
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

        void DecipherComments()
        {
            skSpeaker.text = gameControl.CurrentShopkeeper.npcName;
            dictComments = new Dictionary<string, ShopkeeperComment>();

            // 0 - item name, 1 - emotion, 2 - comment
            if (gameControl.CurrentShopkeeper.commentList == null) return;
            var rows = gameControl.CurrentShopkeeper.commentList.text.Split('\n');

            for(int i = 0; i < rows.Length; i++)
            {
                var line = rows[i];
                var comment = line.Split('\t');

                var itemName = comment[0];
                var emotion = comment[1];
                var commentText = comment[2];

                Sprite _sprite = null;
                if (string.IsNullOrEmpty(emotion))
                    _sprite = gameControl.CurrentShopkeeper.neutralSpr;
                else
                {
                    if (string.Equals(emotion, "HAPPY"))
                        _sprite = gameControl.CurrentShopkeeper.happySpr;

                    else if (string.Equals(emotion, "SAD"))
                        _sprite = gameControl.CurrentShopkeeper.sadSpr;

                    else if (string.Equals(emotion, "ANGRY"))
                        _sprite = gameControl.CurrentShopkeeper.angrySpr;
                }

                var newComment = new ShopkeeperComment();
                newComment.sprite = _sprite;
                newComment.comment = commentText;

                dictComments.Add(itemName, newComment);
            }
        }

        void UpdateComment(ScriptableItem newItem)
        {
            if(dictComments.ContainsKey(newItem.name))
            {
                var currentComment = dictComments[newItem.name];
                skPortrait.sprite = currentComment.sprite;
                skComment.text = currentComment.comment;
            }
            // Default values -- in case we don't have comments for the item for some reason
            else
            {
                skPortrait.sprite = gameControl.CurrentShopkeeper.neutralSpr;
                skComment.text = "I actually don't know what that is.";
            }
        }

        private new void Update()
        {
            base.Update();
            
            if (EventSystem.current.currentSelectedGameObject == null) return;

            totalGold.text = gameControl.totalGold.ToString();

            var itemSlot = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemSlot>();

            if (itemSlot)
                currentItemToBuy = itemSlot.s_item;
            UpdateStats(currentItemToBuy);
            UpdateComment(currentItemToBuy);

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