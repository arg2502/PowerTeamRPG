﻿namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using UnityEngine.EventSystems;
    using System;
    using System.Collections.Generic;

    public class InventoryMenu : GridMenu
    {
        public GameObject itemSlotsContainer;
        Vector3 origPosSlotsContainer;
        public GameObject itemPrefab;
        public GameObject invisiblePrefab;        
        public GameObject upScroll, downScroll;
        Rect screen = new Rect(0,0, Screen.width, Screen.height);
        bool moving;
        float startTime = 0;
        Rect itemSlotsWorld;
        Vector3[] objectCorners;
        GameObject currentObj;
        int outerListPosition;
        int innerListPosition;
        //internal Item chosenItem;
		internal InventoryItem chosenItem;
        internal string currentDescription;

        float buttonDistance = 125f;
        float listDistance = 2000f;
        float lerpTimeVertical = 10f;
        float lerpTimeHorizontal = 5f;


        [Serializable]
        public struct WhichInventoryToggles
        {
            public Toggle consumables, weapons, equipment, keyItems;
        }

        public WhichInventoryToggles InventoryToggles;

        public override void Init()
        {
            base.Init();
            origPosSlotsContainer = itemSlotsContainer.transform.position;
            CheckIfListOffScreen();
        }
        public override void Close()
        {
            itemSlotsContainer.transform.position = origPosSlotsContainer;
            base.Close();
        }
        protected override void SetHorizontalNavigation(int buttonIterator, int listIterator)
        {
            // setting horizontal movement between each list                    
            if (listIterator > 0 && buttonGrid[listIterator - 1].Count > 0)
                navigation.selectOnLeft = buttonGrid[listIterator - 1][0];
            if (listIterator < buttonGrid.Count - 1 && buttonGrid[listIterator + 1].Count > 0)
                navigation.selectOnRight = buttonGrid[listIterator + 1][0];
        }
        protected override void AddButtons()
        {
            // delete anything in button grid
            if (buttonGrid != null)
            {
                for (int i = 0; i < buttonGrid.Count; i++)
                {
                    for (int j = 0; j < buttonGrid[i].Count; j++)
                    {
                        Destroy(buttonGrid[i][j].transform.parent.gameObject);
                    }
                }

                buttonGrid.Clear();
            }

            // create grid with 4 lists --- for the 4 categories of the inventory
            buttonGrid = new List<List<Button>>() { new List<Button>(), new List<Button>(), new List<Button>(), new List<Button>() };

//            FillList(gameControl.consumables, 0);
//            FillList(gameControl.weapons, 1);
//            FillList(gameControl.equipment, 2);
//            FillList(gameControl.reusables, 3);

			FillList(gameControl.consumables, "Consumable", 0);
			FillList(gameControl.weapons, "Weapon", 1);
			FillList(gameControl.equipment, "Armor", 2);
			FillList(gameControl.key, "Key", 3);

            SetButtonNavigation(); // reset button navigation
            gameControl.itemAdded = false; // reset flag to false
        }

        //void FillList(List<GameObject> category, int listPosition)
		void FillList(List<InventoryItem> category, string _type, int listPosition)
        {
            for (int i = 0; i < category.Count; i++)
            {
//                var item = Instantiate(itemPrefab);
//                item.name = category[i].GetComponent<Item>().name + "_Button";
//                item.transform.SetParent(itemSlotsContainer.transform);
//                item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, i * -buttonDistance);
//                item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent
//
//                var button = item.GetComponentInChildren<Button>();
//                var itemInfo = category[i].GetComponent<Item>();
//                button.GetComponentInChildren<Text>().text = itemInfo.name;
//                buttonGrid[listPosition].Add(button);
//
//                // description
//                item.GetComponentInChildren<Description>().description = "<b>" + itemInfo.name + "</b>\n\n" + itemInfo.description;
//
//                var itemSlot = item.GetComponent<ItemSlot>();
//
//                // set item to item slot so it's connected to the button
//                itemSlot.SetItem(itemInfo);
//
//                // add listener
//                button.onClick.AddListener(OnSelect);

				var item = Instantiate(itemPrefab);
                item.name = category[i].name;// + "_Button";
				item.transform.SetParent(itemSlotsContainer.transform);
				item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, i * -buttonDistance);
				item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent
				
				var button = item.GetComponentInChildren<Button>();
				var itemInfo = ItemDatabase.GetItem(_type, item.name);
				button.GetComponentInChildren<Text>().text = itemInfo.name;
				buttonGrid[listPosition].Add(button);
				
				// description
				item.GetComponentInChildren<Description>().description = "<b>" + itemInfo.name + "</b>\n\n" + itemInfo.description;
				
				var itemSlot = item.GetComponent<ItemSlot>();
				
				// set item to item slot so it's connected to the button
				itemSlot.SetItem(category[i]);
				
				// add listener
				button.onClick.AddListener(OnSelect);
            }
            if (category.Count <= 0)
            {
                CreateInvisibleButton(listPosition);      
            }
        }

        public override Button AssignRootButton()
        {
            outerListPosition = (int)gameControl.whichInventoryEnum;//0; // TEMP
            ToggleTextChange();

            var buttonObj = buttonGrid[outerListPosition][innerListPosition];
            var descriptionObj = buttonObj.GetComponentInParent<Description>();

            if (descriptionObj != null)
                descriptionText.text = descriptionObj.description;
            else
                descriptionText.text = "";

            currentDescription = descriptionText.text;

            //Debug.Log("Inventory menu: assign root: " + currentListPosition);
            return buttonGrid[outerListPosition][innerListPosition];

        }
        public override void TurnOnMenu()
        {
            // update the items displayed if a new item has been added to the inventory
            if (gameControl.itemAdded) // TEST THIS WHEN YOU CAN ADD ITEMS
            {
                Debug.Log("item has been added -- update");
                AddButtons();
            }

            innerListPosition = 0;
            rootButton = AssignRootButton();
            currentObj = rootButton.gameObject;
            if (CheckIfOffScreen(currentObj))
            {
                OutsideOfViewInstant(currentObj);
            }
            
            UpdateItemQuantity();

            base.TurnOnMenu();
        }
        public override void Refocus()
        {
            base.Refocus();
            rootButton = AssignRootButton();
            SetSelectedObjectToRoot();
            UpdateItemQuantity();

        }

        void CreateInvisibleButton(int position)
        {
            var item = Instantiate(invisiblePrefab);
            item.transform.SetParent(itemSlotsContainer.transform);
            item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * position, 0);
            item.GetComponent<RectTransform>().localScale = Vector3.one;

            var button = item.GetComponentInChildren<Button>();
            button.GetComponentInChildren<Text>().text = "";
            buttonGrid[position].Add(button);
        }

        bool CheckIfOffScreen(GameObject buttonObj)
        {
            // find the Item Slots Container's rect in world coordinates
            // to see if the button is within it's view or not
            var screenCorners = new Vector3[4];
            var itemSlotsRectTransform = itemSlotsContainer.transform.parent.GetComponent<RectTransform>();
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


            //// if any corner of the object is on screen, break out of the method
            //foreach (Vector3 corner in objectCorners)
            //{
            //    if (itemSlotsWorld.Contains(corner))
            //        return false;
            //}

            //return true;

            // WRONG -- It should be, if any corner is off screen, return true
            foreach (Vector3 corner in objectCorners)
            {
                if (!itemSlotsWorld.Contains(corner))
                    return true;
            }
            return false;
        }

        void OutsideOfViewInstant(GameObject desiredObj)
        {
            var desiredPosition = desiredObj.transform.position;

            // horizontal movement
            if (objectCorners[0].x > itemSlotsWorld.xMax)
            {
                desiredPosition = buttonGrid[outerListPosition - 1][0].transform.position;
            }
            else if (objectCorners[2].x < itemSlotsWorld.xMin)
            {
                desiredPosition = buttonGrid[outerListPosition + 1][0].transform.position;
            }
            float distance;
            Vector2 newPosition = Vector2.zero;
            if (desiredPosition.x != desiredObj.transform.position.x)
            {
                distance = desiredPosition.x - desiredObj.transform.position.x;
                newPosition = new Vector2(itemSlotsContainer.transform.position.x + distance, itemSlotsContainer.transform.position.y);
            }

            itemSlotsContainer.transform.position = newPosition;
            if (CheckIfOffScreen(desiredObj))
                OutsideOfViewInstant(desiredObj);
        }

        void OutsideOfView(GameObject buttonObj)
        {
            //Debug.Log("outside");

            // if we reach this point, that means we are off screen
            // bring it on screen
            // find the button
            int ourButtonListPosition = 0;
            for (int i = 0; i < buttonGrid[outerListPosition].Count; i++)
            {
                if (buttonObj == buttonGrid[outerListPosition][i].gameObject)
                {
                    ourButtonListPosition = i;
                    break;
                }
            }


            // if below screen, set the button above it as the desired position
            Vector3 desiredPosition = buttonObj.transform.position;
            // bool belowScreen = false;

            // vertical movement
            if (objectCorners[0].y < itemSlotsWorld.yMin)
            {
                if (ourButtonListPosition > 0)
                    desiredPosition = buttonGrid[outerListPosition][ourButtonListPosition - 1].transform.position;
                else
                    desiredPosition = buttonGrid[outerListPosition][0].transform.position;
            }
            else if (objectCorners[2].y > itemSlotsWorld.yMax)
            {
                var lastElement = buttonGrid[outerListPosition].Count - 1;
                if (ourButtonListPosition < lastElement)
                    desiredPosition = buttonGrid[outerListPosition][ourButtonListPosition + 1].transform.position;
                else
                    desiredPosition = buttonGrid[outerListPosition][lastElement].transform.position;
            }

            // horizontal movement
            if (objectCorners[0].x > itemSlotsWorld.xMax)
            {
                desiredPosition = buttonGrid[outerListPosition - 1][0].transform.position;
            }
            else if (objectCorners[2].x < itemSlotsWorld.xMin)
            {
                desiredPosition = buttonGrid[outerListPosition + 1][0].transform.position;
            }

            // determine distance and new position based on difference in either x or y
            float distance;
            Vector2 newPosition;
            float lerpTime;
            if (desiredPosition.x != buttonObj.transform.position.x)
            {
                distance = desiredPosition.x - buttonObj.transform.position.x;
                newPosition = new Vector2(itemSlotsContainer.transform.position.x + distance, origPosSlotsContainer.y);
                lerpTime = lerpTimeHorizontal;
            }
            else
            {
                distance = desiredPosition.y - buttonObj.transform.position.y;
                newPosition = new Vector2(itemSlotsContainer.transform.position.x, itemSlotsContainer.transform.position.y + distance);
                lerpTime = lerpTimeVertical;
            }
            
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
            original = itemSlotsContainer.transform.position;
            while ((itemSlotsContainer.transform.position - newPosition).magnitude >= 0.1f)
            {
                itemSlotsContainer.transform.position = Vector2.Lerp(original, newPosition, (Time.time - startTime) * lerpTime);                
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
            var currentList = buttonGrid[outerListPosition];

            // if the first item in the list is off screen, then turn on the up scroll notifier
            if (CheckIfOffScreen(currentList[0].gameObject))
                upScroll.SetActive(true);
            else
                upScroll.SetActive(false);

            // if the last item in the list is off screen, then turn on the down scroll notifier
            if (CheckIfOffScreen(currentList[currentList.Count - 1].gameObject))
                downScroll.SetActive(true);
            else
                downScroll.SetActive(false);
        }

        void OnEnable()
        {
            currentObj = EventSystem.current.currentSelectedGameObject;
        }

        void ToggleTextChange()
        {
            InventoryToggles.consumables.isOn = false;
            InventoryToggles.weapons.isOn = false;
            InventoryToggles.equipment.isOn = false;
            InventoryToggles.keyItems.isOn = false;

            switch (outerListPosition)
            {
                case 0:
                    InventoryToggles.consumables.isOn = true;
                    break;
                case 1:
                    InventoryToggles.weapons.isOn = true;
                    break;
                case 2:
                    InventoryToggles.equipment.isOn = true;
                    break;
                case 3:
                    InventoryToggles.keyItems.isOn = true;
                    break;
            }
        }

        void SetDescription()
        {
            // set description text if applicable
            // (invisible buttons do not have descriptions)

            var descriptionObj = currentObj.GetComponentInParent<Description>();
            if (descriptionObj != null)
                descriptionText.text = descriptionObj.description;
            else
                descriptionText.text = "";

            currentDescription = descriptionText.text;
        }
        
        void OnSelect()
        {
            // save the item you wish to use/equip
            chosenItem = EventSystem.current.currentSelectedGameObject.GetComponentInParent<ItemSlot>().item;

            // open the ConfirmUse menu
            uiManager.PushMenu(uiDatabase.ConfirmUseMenu);

            //var count = uiManager.list_currentMenus.Count;
            //var confirmUse = uiManager.list_currentMenus[count - 1].GetComponent<ConfirmUseMenu>();
            //confirmUse.item = chosenItem;
            //confirmUse.descriptionText = descriptionText;
            //uiManager.PushMenu(uiDatabase.UseItemMenu);

            //// set the Item Use menu's item to the one chosen
            //var count = uiManager.list_currentMenus.Count;
            //var useItem = uiManager.list_currentMenus[count - 1].GetComponent<UseItemMenu>();
            //useItem.item = chosenItem;
            //useItem.descriptionText = descriptionText;
            //useItem.icon.sprite = chosenItem.sprite;
        }

        void UpdateItemQuantity()
        {
            var itemSlot = currentObj.GetComponentInParent<ItemSlot>();
            if (itemSlot == null) return;
            itemSlot.UpdateQuantity();
            
            // check if consumable & zero
            if(itemSlot.item.type == "consumable"
                && itemSlot.item.quantity - itemSlot.item.uses <= 0)
            {
                // remove item button from grid and delete
                // but first save the index position
                var index = buttonGrid[outerListPosition].IndexOf(currentObj.GetComponent<Button>());//Find(currentObj.GetComponent<Button>())
                buttonGrid[outerListPosition].Remove(currentObj.GetComponent<Button>());
                Destroy(currentObj.transform.parent.gameObject);
                gameControl.RemoveItem(itemSlot.item);                

                // add an invisible button if there are no more items in the list
                if(buttonGrid[outerListPosition].Count <= 0)
                {
                    CreateInvisibleButton(outerListPosition);
                }

                // move any buttons below up
                //item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, i * -buttonDistance);
                if (index < buttonGrid[outerListPosition].Count)
                {
                    for(int i = index; i < buttonGrid[outerListPosition].Count; i++)
                        buttonGrid[outerListPosition][i].transform.parent.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * outerListPosition, i * -buttonDistance);

                    // reset navigation
                    SetButtonNavigation();
                    rootButton = buttonGrid[outerListPosition][index];

                    EventSystem.current.SetSelectedGameObject(rootButton.gameObject);
                }
                // if not buttons below (last in column), just set eventsystem
                else
                {
                    EventSystem.current.SetSelectedGameObject(buttonGrid[outerListPosition][buttonGrid[outerListPosition].Count - 1].gameObject);
                }
            }
        }

        new void Update()
        {
            base.Update();

            if (this.gameObject != uiManager.menuInFocus) return;

            if (currentObj == EventSystem.current.currentSelectedGameObject) return;

            currentObj = EventSystem.current.currentSelectedGameObject;

            if (currentObj == null) return;
            
            // set current list position by finding the new current button
            for(int i = 0; i < buttonGrid.Count; i++)
            {
                if (buttonGrid[i].Count > 0 && currentObj == buttonGrid[i][0].gameObject)
                {
                    outerListPosition = i;
                    gameControl.whichInventoryEnum = (GameControl.WhichInventory) outerListPosition;
                    break;
                }
            }
            // find position within the column
            for(int i = 0; i < buttonGrid[outerListPosition].Count; i++)
            {
                if(currentObj == buttonGrid[outerListPosition][i].gameObject)
                {
                    innerListPosition = i;
                    break;
                }
            }

            ToggleTextChange();
            SetDescription();

            if (CheckIfOffScreen(currentObj))
                OutsideOfView(currentObj);
            
        }
    }
}