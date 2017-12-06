namespace UI
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
        public GameObject itemPrefab;
        public GameObject invisiblePrefab;        
        public GameObject upScroll, downScroll;
        Rect screen = new Rect(0,0, Screen.width, Screen.height);
        bool moving;
        float startTime = 0;
        Rect itemSlotsWorld;
        Vector3[] objectCorners;
        GameObject currentObj;
        int currentListPosition;
        internal Item chosenItem;
        internal string currentDescription;

        float buttonDistance = 55f;
        float listDistance = 1000f;
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

            CheckIfListOffScreen();
        }

        protected override void AddButtons()
        {
            // create grid with 4 lists --- for the 4 categories of the inventory
            buttonGrid = new List<List<Button>>() { new List<Button>(), new List<Button>(), new List<Button>(), new List<Button>() };

            FillList(gameControl.consumables, 0);
            FillList(gameControl.weapons, 1);
            FillList(gameControl.equipment, 2);
            FillList(gameControl.reusables, 3);
        }

        void FillList(List<GameObject> category, int listPosition)
        {
            for (int i = 0; i < category.Count; i++)
            {
                var item = Instantiate(itemPrefab);
                item.name = category[i].GetComponent<Item>().name + "_Button";
                item.transform.SetParent(itemSlotsContainer.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, i * -buttonDistance);

                var button = item.GetComponentInChildren<Button>();
                var itemInfo = category[i].GetComponent<Item>();
                button.GetComponentInChildren<Text>().text = itemInfo.name;
                buttonGrid[listPosition].Add(button);

                // description
                item.GetComponentInChildren<Description>().description = "<b>" + itemInfo.name + "</b>\n\n" + itemInfo.description;

                var itemSlot = item.GetComponent<ItemSlot>();

                // set item to item slot so it's connected to the button
                itemSlot.SetItem(itemInfo);

                // add listener
                button.onClick.AddListener(OnSelect);
            }
            if (category.Count <= 0)
            {
                var item = Instantiate(invisiblePrefab);
                item.transform.SetParent(itemSlotsContainer.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, 0);

                var button = item.GetComponentInChildren<Button>();
                button.GetComponentInChildren<Text>().text = "";
                buttonGrid[listPosition].Add(button);                
            }
        }

        public override Button AssignRootButton()
        {
            currentListPosition = (int)gameControl.whichInventoryEnum;//0; // TEMP
            ToggleTextChange();

            var buttonObj = buttonGrid[currentListPosition][0];
            var descriptionObj = buttonObj.GetComponentInParent<Description>();

            if (descriptionObj != null)
                descriptionText.text = descriptionObj.description;
            else
                descriptionText.text = "";

            currentDescription = descriptionText.text;

            //Debug.Log("Inventory menu: assign root: " + currentListPosition);
            return buttonGrid[currentListPosition][0];

        }
        public override void TurnOnMenu()
        {
            rootButton = AssignRootButton();
            currentObj = rootButton.gameObject;
            if (CheckIfOffScreen(currentObj))
            {
                OutsideOfViewInstant(currentObj);
            }

            base.TurnOnMenu();
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

            // if any corner of the object is on screen, break out of the method
            foreach (Vector3 corner in objectCorners)
            {
                if (itemSlotsWorld.Contains(corner))                
                    return false;                
            }

            return true;
        }

        void OutsideOfViewInstant(GameObject desiredObj)
        {
            var desiredPosition = desiredObj.transform.position;

            // horizontal movement
            if (objectCorners[0].x > itemSlotsWorld.xMax)
            {
                desiredPosition = buttonGrid[currentListPosition - 1][0].transform.position;
            }
            else if (objectCorners[2].x < itemSlotsWorld.xMin)
            {
                desiredPosition = buttonGrid[currentListPosition + 1][0].transform.position;
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
            for (int i = 0; i < buttonGrid[currentListPosition].Count; i++)
            {
                if (buttonObj == buttonGrid[currentListPosition][i].gameObject)
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
                if (ourButtonListPosition <= 0) return;                
                desiredPosition = buttonGrid[currentListPosition][ourButtonListPosition - 1].transform.position;
                //belowScreen = true;
            }
            else if(objectCorners[2].y > itemSlotsWorld.yMax)
            {
                if (ourButtonListPosition >= buttonGrid[currentListPosition].Count - 1) return;
                desiredPosition = buttonGrid[currentListPosition][ourButtonListPosition + 1].transform.position;
               // belowScreen = false;
            }

            // horizontal movement
            if(objectCorners[0].x > itemSlotsWorld.xMax)
            {
                desiredPosition = buttonGrid[currentListPosition - 1][0].transform.position;
            }
            else if (objectCorners[2].x < itemSlotsWorld.xMin)
            {
                desiredPosition = buttonGrid[currentListPosition + 1][0].transform.position;
            }

            // determine distance and new position based on difference in either x or y
            float distance;
            Vector2 newPosition;
            float lerpTime;
            if (desiredPosition.x != buttonObj.transform.position.x)
            {
                distance = desiredPosition.x - buttonObj.transform.position.x;
                newPosition = new Vector2(itemSlotsContainer.transform.position.x + distance, itemSlotsContainer.transform.position.y);
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
            var currentList = buttonGrid[currentListPosition];

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

            switch (currentListPosition)
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

        new void Update()
        {
            base.Update();

            if (this.gameObject != uiManager.menuInFocus) return;

            if (currentObj == EventSystem.current.currentSelectedGameObject) return;

            currentObj = EventSystem.current.currentSelectedGameObject;
            
            // set current list position by finding the new current button
            for(int i = 0; i < buttonGrid.Count; i++)
            {
                if (buttonGrid[i].Count > 0 && currentObj == buttonGrid[i][0].gameObject)
                {
                    currentListPosition = i;
                    gameControl.whichInventoryEnum = (GameControl.WhichInventory) currentListPosition;
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