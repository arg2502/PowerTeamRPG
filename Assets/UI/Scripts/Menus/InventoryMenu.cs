namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using UnityEngine.EventSystems;
    using System;

    public class InventoryMenu : Menu
    {
        public GameObject itemSlotsContainer;
        Rect screen = new Rect(0,0, Screen.width, Screen.height);
        bool moving;
        float startTime = 0;
        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new System.Collections.Generic.List<Button>();
            foreach (var button in itemSlotsContainer.GetComponentsInChildren<Button>())
                listOfButtons.Add(button);
            
            for(int i = 0; i < listOfButtons.Count; i++)// GameControl.control.consumables.Count; i++)
            {
                if(i >= GameControl.control.consumables.Count)
                {
                    //listOfButtons[i].gameObject.SetActive(false);
                    continue;
                }
                listOfButtons[i].GetComponentInChildren<Text>().text = GameControl.control.consumables[i].GetComponent<Item>().name;
            }
        }

        public override Button AssignRootButton()
        {
            return listOfButtons[0];

        }

        void CheckIfOffScreen(GameObject buttonObj)
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
            var itemSlotsWorld = new Rect(screenCorners[0], size);

            // find the buttons world coordinates
            var objectCorners = new Vector3[4];
            var contentTransform = buttonObj.GetComponent<RectTransform>();
            contentTransform.GetWorldCorners(objectCorners);

            // if any corner of the object is on screen, break out of the method
            foreach (Vector3 corner in objectCorners)
            {
                if (itemSlotsWorld.Contains(corner))                
                    return;                
            }
            Debug.Log("outside of view");
            // if we reach this point, that means we are off screen
            // bring it on screen
            // find the button
            int ourButtonListPosition = 0;
            for(int i = 0; i < listOfButtons.Count; i++)
            {
                if(buttonObj == listOfButtons[i].gameObject)
                {
                    ourButtonListPosition = i;
                    break;
                }
            }


            // if below screen, set the button above it as the desired position
            Vector3 desiredPosition;
            bool belowScreen;
            if (objectCorners[0].y < screen.yMin)
            {
                desiredPosition = listOfButtons[ourButtonListPosition - 1].transform.position;
                belowScreen = true;
            }
            else
            {
                desiredPosition = listOfButtons[ourButtonListPosition + 1].transform.position;
                belowScreen = false;
            }
            var distance = desiredPosition.y - buttonObj.transform.position.y;
            var newPosition = new Vector2(itemSlotsContainer.transform.position.x, itemSlotsContainer.transform.position.y + distance);
            if (!moving)
            {
                moving = true;
                startTime = Time.time;
                StartCoroutine(MoveButtonOntoScreen(newPosition, belowScreen));
            }
        }

        private IEnumerator MoveButtonOntoScreen(Vector3 newPosition, bool belowScreen)
        {
            var original = new Vector3();
            original = itemSlotsContainer.transform.position;
            while (itemSlotsContainer.transform.position != newPosition)
            {
                itemSlotsContainer.transform.position = Vector2.Lerp(original, newPosition, (Time.time - startTime) * 10);
                //itemSlotsContainer.transform.Translate(0, distance, 0);
                yield return null;
            }
            moving = false;
        }

        new void Update()
        {
            base.Update();

            CheckIfOffScreen(EventSystem.current.currentSelectedGameObject);
            
        }
    }
}