﻿namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.EventSystems;

    public class ListSub : Menu
    {
        BattleManager battleManager;

        public GameObject slotPrefab;
        public GameObject jethroSkillsContainers, jethroSpellsContainers,
            coleSkillsContainers, coleSpellsContainers,
            eleanorSkillsContainers, eleanorSpellsContainers,
            joulietteSkillsContainers, joulietteSpellsContainers;

        float slotDistance = 65f;

        // move button variables
        float defaultLerpTime = 10f;
        float startTime = 0f;
        Rect itemSlotsWorld;
        Vector3[] objectCorners;
        GameObject currentContainer;
        GameObject lastContainer;
        bool moving;
        GameObject currentObj;
        Vector2 originalContainerPos;

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            currentContainer = jethroSkillsContainers; // default -- for positioning only
            originalContainerPos = currentContainer.transform.localPosition;
            currentContainer = null;
            base.Init();
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();
            listOfButtons = new List<Button>();
            FillList(battleManager.CurrentHero);
            
            //if (currentContainer != lastContainer)
            //{
                rootButton = listOfButtons[0];
                currentContainer.transform.localPosition = originalContainerPos;
                //lastContainer = currentContainer;
            //}   
            SetSelectedObjectToRoot();
        }
        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>();
        }

        public override void Refocus()
        {
            base.Refocus();
            uiManager.ShowAllMenus();            
        }

        void FillList(Hero currentHero) // SPELL IS TEMPORARY -- SHOULD BE ABLE TO HANDLE ANY TECHNIQUE
        {
            // set/find container
            if (currentHero is Jethro)
                currentContainer = jethroSpellsContainers;
            else if (currentHero is Cole)
                currentContainer = coleSpellsContainers;
            else if (currentHero is Eleanor)
                currentContainer = eleanorSpellsContainers;
            else
                currentContainer = joulietteSpellsContainers;

            currentContainer.SetActive(true);

            // if we already created this hero's specific technique buttons, just add them to the buttons list
            if (currentContainer.GetComponentsInChildren<Button>().Length > 0)
            {
                foreach (var b in currentContainer.GetComponentsInChildren<Button>())
                    listOfButtons.Add(b);
            }
            else
            {

                var category = currentHero.SpellsList;

                for (int i = 0; i < category.Count; i++)
                {
                    var item = Instantiate(slotPrefab);
                    item.name = category[i].Name + "_Button";
                    item.transform.SetParent(currentContainer.transform);
                    item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * -slotDistance);
                    item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                    // assign variables of UI
                    item.GetComponent<ListButton>().SetTechnique(category[i]);

                    var button = item.GetComponentInChildren<Button>();
                    //button.GetComponentInChildren<Text>().text = category[i].Name;
                    listOfButtons.Add(button);

                    // assign attacks
                    //// remove spaces from attack name
                    //var attack = category[i].Name.Replace(" ", string.Empty);
                    var attack = category[i].Name;
                    button.onClick.AddListener(() => OnSelect(attack));
                }
                SetButtonNavigation();
            }
        }

        void OnSelect(string attack)
        {
            uiManager.HideAllMenus();
            battleManager.DetermineTargetType(attack);
            uiManager.PushMenu(uiDatabase.TargetMenu);
        }        

        public void SetContainersToNull()
        {
            //currentContainer = null;
            //lastContainer = null;
        }

        // SCOLLING METHODS
        bool CheckIfOffScreen(GameObject buttonObj)
        {
            // find the Item Slots Container's rect in world coordinates
            // to see if the button is within it's view or not
            var screenCorners = new Vector3[4];
            var itemSlotsRectTransform = currentContainer.transform.parent.GetComponent<RectTransform>();
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

            // see if any part of the button is hanging off screen
            int cornerCounter = 0;
            foreach (Vector3 corner in objectCorners)
            {
                if (itemSlotsWorld.Contains(corner))
                    cornerCounter++;
            }

            // if ALL corners of the object is on screen, return false
            if (cornerCounter < 4)
                return true;
            else return false;
        }

        void OutsideOfViewInstant(GameObject desiredObj)
        {
            var desiredPosition = desiredObj.transform.position;

            // horizontal movement
            //if (objectCorners[0].x > itemSlotsWorld.xMax)
            //{
            //    desiredPosition = buttonGrid[outerListPosition - 1][0].transform.position;
            //}
            //else if (objectCorners[2].x < itemSlotsWorld.xMin)
            //{
                desiredPosition = listOfButtons[0].transform.position;
            //}
            float distance;
            Vector2 newPosition = Vector2.zero;
            if (desiredPosition.y != desiredObj.transform.position.y)
            {
                distance = desiredPosition.y - desiredObj.transform.position.y;
                newPosition = new Vector2(currentContainer.transform.position.x, currentContainer.transform.position.y + distance);
            }

            currentContainer.transform.position = newPosition;
            //if (CheckIfOffScreen(desiredObj))
            //    OutsideOfViewInstant(desiredObj);
        }

        void OutsideOfView(GameObject buttonObj, float lerpTime = -1f)
        {
            if (lerpTime < 0)
                lerpTime = defaultLerpTime;
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

            // vertical movement
            if (objectCorners[0].y < itemSlotsWorld.yMin)
            {
                if (ourButtonListPosition <= 0) return;
                desiredPosition = listOfButtons[ourButtonListPosition - 1].transform.position;
                //belowScreen = true;
            }
            else if (objectCorners[2].y > itemSlotsWorld.yMax)
            {
                if (ourButtonListPosition >= listOfButtons.Count - 1) return;
                desiredPosition = listOfButtons[ourButtonListPosition + 1].transform.position;
                // belowScreen = false;
            }

            // determine distance and new position based on difference in either x or y
            float distance;
            Vector2 newPosition;
            distance = desiredPosition.y - buttonObj.transform.position.y;
            newPosition = new Vector2(currentContainer.transform.position.x, currentContainer.transform.position.y + distance);
            
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
            original = currentContainer.transform.position;
            while ((currentContainer.transform.position - newPosition).magnitude >= 0.1f)
            {
                currentContainer.transform.position = Vector2.Lerp(original, newPosition, (Time.time - startTime) * lerpTime);
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
            //if (CheckIfOffScreen(listOfButtons[0].gameObject))
            //    upScroll.SetActive(true);
            //else
            //    upScroll.SetActive(false);

            //// if the last item in the list is off screen, then turn on the down scroll notifier
            //if (CheckIfOffScreen(listOfButtons[listOfButtons.Count - 1].gameObject))
            //    downScroll.SetActive(true);
            //else
            //    downScroll.SetActive(false);
        }

        new void Update()
        {
            base.Update();

            if (currentObj == EventSystem.current.currentSelectedGameObject
                || !gameObject.activeSelf)
                return;

            currentObj = EventSystem.current.currentSelectedGameObject;
            rootButton = currentObj.GetComponent<Button>();

            if (CheckIfOffScreen(currentObj))
                OutsideOfView(currentObj);
        }

    }
}