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
        float buttonDistance = 55f;
        float listDistance = 200f;
        public GameObject upScroll, downScroll;
        Rect screen = new Rect(0,0, Screen.width, Screen.height);
        bool moving;
        float startTime = 0;
        Rect itemSlotsWorld;
        Vector3[] objectCorners;
        GameObject currentObj;
        int currentListPosition;


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
                item.transform.SetParent(itemSlotsContainer.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, i * -buttonDistance);

                var button = item.GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = category[i].GetComponent<Item>().name;
                buttonGrid[listPosition].Add(button);
            }
            if (category.Count <= 0)
            {
                var item = Instantiate(invisiblePrefab);
                item.transform.SetParent(itemSlotsContainer.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(listDistance * listPosition, 0);

                var button = item.GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = "";
                buttonGrid[listPosition].Add(button);
            }
        }

        public override Button AssignRootButton()
        {
            currentListPosition = 0; // TEMP
            return buttonGrid[currentListPosition][0];

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

        new void Update()
        {
            base.Update();

            if (currentObj == EventSystem.current.currentSelectedGameObject) return;

            currentObj = EventSystem.current.currentSelectedGameObject;
            
            // set current list position by finding the new current button
            for(int i = 0; i < buttonGrid.Count; i++)
            {
                if (buttonGrid[i].Count > 0 && currentObj == buttonGrid[i][0].gameObject)
                {
                    currentListPosition = i;
                    Debug.Log("Current list pos: " + currentListPosition);
                    break;
                }
            }

            if(CheckIfOffScreen(currentObj))
                OutsideOfView(currentObj);
            
        }
    }
}