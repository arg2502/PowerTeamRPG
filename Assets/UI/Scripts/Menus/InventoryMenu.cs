namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using UnityEngine.EventSystems;
    using System;
    using System.Collections.Generic;

    public class InventoryMenu : Menu
    {
        public GameObject itemSlotsContainer;
        public GameObject itemPrefab;
        float buttonDistance = -55f;
        public GameObject upScroll, downScroll;
        Rect screen = new Rect(0,0, Screen.width, Screen.height);
        bool moving;
        float startTime = 0;
        Rect itemSlotsWorld;
        Vector3[] objectCorners;
        GameObject currentObj;


        public override void Init()
        {
            base.Init();

            CheckIfListOffScreen();
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new System.Collections.Generic.List<Button>();
            CreateButtons();
            

            //foreach (var button in itemSlotsContainer.GetComponentsInChildren<Button>())
            //    listOfButtons.Add(button);
            
            //for(int i = 0; i < listOfButtons.Count; i++)// GameControl.control.consumables.Count; i++)
            //{
            //    if(i >= GameControl.control.consumables.Count)
            //    {
            //        //listOfButtons[i].gameObject.SetActive(false);
            //        continue;
            //    }
            //    listOfButtons[i].GetComponentInChildren<Text>().text = GameControl.control.consumables[i].GetComponent<Item>().name;
            //}
        }

        public override Button AssignRootButton()
        {
            return listOfButtons[0];

        }

        public void CreateButtons()
        {
            List<GameObject> inventoryList;

            if (gameControl.whichInventoryEnum == GameControl.WhichInventory.Consumables)
                inventoryList = gameControl.consumables;
            else if (gameControl.whichInventoryEnum == GameControl.WhichInventory.Weapons)
                inventoryList = gameControl.weapons;
            else if (gameControl.whichInventoryEnum == GameControl.WhichInventory.Equipment)
                inventoryList = gameControl.equipment;
            else
                inventoryList = gameControl.reusables;

            // create the inventory list and set the appropriate text based on which inventory
            for (int i = 0; i < inventoryList.Count; i++)
            {
                GameObject item;

                // if we are beyond the existing list of buttons we need to create more
                // on first creation, this statement should always be true
                if (i >= listOfButtons.Count)
                {
                    item = Instantiate(itemPrefab);
                    item.transform.SetParent(itemSlotsContainer.transform);
                    item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * buttonDistance);
                }
                // if we are returning to the menu, or to a new category, there should normally be at least one button already created
                // set it to that one before creating more
                else
                {
                    listOfButtons[i].gameObject.SetActive(true);
                    item = listOfButtons[i].gameObject;
                }

                var button = item.GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = inventoryList[i].GetComponent<Item>().name;

                // add button to the list, if the list does not already contain it
                if (!listOfButtons.Contains(button))
                    listOfButtons.Add(button);
            }


            EventSystem.current.SetSelectedGameObject(AssignRootButton().gameObject);

            // hide any leftover buttons
            if (listOfButtons.Count > inventoryList.Count)
            {
                for (int i = inventoryList.Count; i < listOfButtons.Count; i++)
                {
                    listOfButtons[i].gameObject.SetActive(false);
                }
            }

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

        void OutsideOfView(GameObject buttonObj)
        {
            //Debug.Log("outside");

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
           // bool belowScreen = false;
            if (objectCorners[0].y < itemSlotsWorld.yMin)
            {
                if (ourButtonListPosition <= 0) return;                
                desiredPosition = listOfButtons[ourButtonListPosition - 1].transform.position;
                //belowScreen = true;
            }
            else if(objectCorners[2].y > itemSlotsWorld.yMax)
            {
                if (ourButtonListPosition >= listOfButtons.Count - 1) return;
                desiredPosition = listOfButtons[ourButtonListPosition + 1].transform.position;
               // belowScreen = false;
            }
            var distance = desiredPosition.y - buttonObj.transform.position.y;
            var newPosition = new Vector2(itemSlotsContainer.transform.position.x, itemSlotsContainer.transform.position.y + distance);
            if (!moving)
            {
                moving = true;
                startTime = Time.time;
                StartCoroutine(MoveButtonOntoScreen(newPosition));
            }
        }

        private IEnumerator MoveButtonOntoScreen(Vector3 newPosition)
        {
            var original = new Vector3();
            original = itemSlotsContainer.transform.position;
            while (itemSlotsContainer.transform.position != newPosition)
            {
                itemSlotsContainer.transform.position = Vector2.Lerp(original, newPosition, (Time.time - startTime) * 10);
                Debug.Log(newPosition);
                //itemSlotsContainer.transform.Translate(0, distance, 0);
                yield return null;
            }
            moving = false;
            CheckIfListOffScreen();
        }

        void CheckIfListOffScreen()
        {
            // if the first item in the list is off screen, then turn on the up scroll notifier
            if (CheckIfOffScreen(listOfButtons[0].gameObject))
                upScroll.SetActive(true);
            else
                upScroll.SetActive(false);

            // if the last item in the list is off screen, then turn on the down scroll notifier
            if (CheckIfOffScreen(listOfButtons[listOfButtons.Count - 1].gameObject))
                downScroll.SetActive(true);
            else
                downScroll.SetActive(false);
        }

        public override void TurnOnMenu()
        {
            CreateButtons();

            base.TurnOnMenu();
        }

        new void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (gameControl.whichInventoryEnum == GameControl.WhichInventory.KeyItems) return;

                gameControl.whichInventoryEnum++;
                CreateButtons();
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (gameControl.whichInventoryEnum == GameControl.WhichInventory.Consumables) return;
                gameControl.whichInventoryEnum--;
                CreateButtons();
            }

            if (currentObj == EventSystem.current.currentSelectedGameObject) return;

            currentObj = EventSystem.current.currentSelectedGameObject;

            if (listOfButtons.Count <= 0) return;

            if(CheckIfOffScreen(currentObj))
                OutsideOfView(currentObj);

            
        }
    }
}