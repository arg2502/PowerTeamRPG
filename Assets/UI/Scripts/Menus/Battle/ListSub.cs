namespace UI
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
        public GameObject itemsContainers,
            jethroSkillsContainers, jethroSpellsContainers,
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

        [Header("Description Info")]
        public GameObject damageObj;
        public GameObject accuracyObj;
        public GameObject critObj;
        public Image targetType;

        [Header("Sprite Icons")]
        public Sprite damageIcon;
        public Sprite hpIcon;
        public Sprite pmIcon;
        public Sprite reviveIcon;
        public Sprite selfTarget;
        public Sprite singleTarget;
        public Sprite splashTarget;
        public Sprite teamTarget;

        Image _damageImage;
        Text damageText;
        Text accuracyText;
        Text critText;

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            //descriptionText = battleManager.DescriptionText;
            currentContainer = jethroSkillsContainers; // default -- for positioning only
            originalContainerPos = currentContainer.transform.localPosition;
            currentContainer = null;

            _damageImage = damageObj.GetComponentInChildren<Image>();
            damageText = damageObj.GetComponentInChildren<Text>();
            accuracyText = accuracyObj.GetComponentInChildren<Text>();
            critText = critObj.GetComponentInChildren<Text>();

            base.Init();
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();
            listOfButtons = new List<Button>();
            FillList();
            //if (currentContainer != lastContainer)
            //{
            rootButton = AssignRootButton();
            currentContainer.transform.localPosition = originalContainerPos;
            //lastContainer = currentContainer;
            //}   
            UpdateButtonStatInfo();
            SetSelectedObjectToRoot();
            
        }
        public override Button AssignRootButton()
        {
            if (listOfButtons.Count <= 0)
                return null;

            for(int i = 0; i < listOfButtons.Count; i++)
            {
                if (listOfButtons[i].interactable)
                    return listOfButtons[i];
            }

            // We should not reach this point, as we should not be able to enter a menu with 0 interactable buttons
            // return 0 pos just in case, but also log error
            Debug.LogError("No interactable buttons found. We should not be in this menu.");
            return listOfButtons[0];
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
            battleManager.ShowAllShortCardsExceptCurrent();
            //rootButton = currentObj.GetComponent<Button>();
            //SetSelectedObjectToRoot();
            //print("REFOCUS");
        }

        void FillList()
        {
            // Items
            if (battleManager.menuState == MenuState.ITEMS)
            {
                currentContainer = itemsContainers;
                FillItems();
            }
            // Skills or Spells
            else
            {
               
                FillTechniques();
            }

            currentContainer.SetActive(true);

            
        }

        void FillItems()
        {
            var containerButtons = currentContainer.GetComponentsInChildren<ListButton>();
            //print("length: " + containerButtons.Length);

            // check if we've already created the list
            // check and see if we have the right number of buttons that we need -- if we used the last of an item in the last round, then we would have more buttons than we need
            // check if the quanity - inUse of any item is 0
            if (containerButtons.Length > 0
                && containerButtons.Length == gameControl.consumables.Count
                && AllItemsAboveZero())
            {

                // now we can add the buttons we have to our button list
                foreach (var b in currentContainer.GetComponentsInChildren<Button>())
                {
                    listOfButtons.Add(b);
                }

                // but we also need to update the item quantity
                for (int i = 0; i < containerButtons.Length; i++)
                {
                    var listButton = containerButtons[i];
                    listButton.RefreshItemQuantity(gameControl.consumables[i].GetComponent<Item>());
                }
                // we're all set, now we can leave
                return;
                
            }

            // if the num of buttons does not match the num of consumables, then we need to start all over
            for (int i = 0; i < containerButtons.Length; i++)
            {
                Destroy(containerButtons[i].gameObject);
            }
            CreateItemList();
            
        }
        void CreateItemList()
        {
            var category = gameControl.consumables;

            // search for any unavailable items (items where the remaining are chosen to be used up in battle by other heroes)
            for(int i = 0; i < category.Count; i++)
            {
                var item = category[i].GetComponent<ConsumableItem>();
                // if there are no more of this item available, skip it
                if (!item.Available)
                    category.RemoveAt(i);
            }

            for (int i = 0; i < category.Count; i++)
            {
                var consumableItem = category[i].GetComponent<ConsumableItem>();

                var itemObj = Instantiate(slotPrefab);
                itemObj.name = consumableItem.name + "_Button";
                itemObj.transform.SetParent(currentContainer.transform);
                itemObj.GetComponent<RectTransform>().localPosition = new Vector2(0, i * -slotDistance);
                itemObj.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                // assign variables of UI
                itemObj.GetComponent<ListButton>().SetItem(consumableItem);

                var button = itemObj.GetComponentInChildren<Button>();
                button.name = consumableItem.name;
                button.GetComponentInChildren<Text>().text = consumableItem.name;
                listOfButtons.Add(button);

                // assign attacks
                //// remove spaces from attack name
                button.onClick.RemoveAllListeners();
                var itemName = consumableItem.name;
                button.onClick.AddListener(() => OnSelect(itemName));

                // set description
                button.GetComponent<Description>().SetDescription(consumableItem.description);
            }
            SetButtonNavigation();
            AddListeners();
        }

        bool AllItemsAboveZero()
        {
            foreach(var itemObj in gameControl.consumables)
            {
                var item = itemObj.GetComponent<ConsumableItem>();
                if (!item.Available)
                {
                    print("IS THIS ABOVE ZERO YO?: " + item.name + " " + (item.quantity - item.inUse));
                    return false;
                }
            }
            return true;
        }

        void FillTechniques()
        {
            var currentHero = battleManager.CurrentDenigen as Hero;

            // set/find container
            if (battleManager.menuState == MenuState.SPELLS)
            {
                if (currentHero is Jethro)
                    currentContainer = jethroSpellsContainers;
                else if (currentHero is Cole)
                    currentContainer = coleSpellsContainers;
                else if (currentHero is Eleanor)
                    currentContainer = eleanorSpellsContainers;
                else
                    currentContainer = joulietteSpellsContainers;
            }
            else if(battleManager.menuState == MenuState.SKILLS)
            {
                if (currentHero is Jethro)
                    currentContainer = jethroSkillsContainers;
                else if (currentHero is Cole)
                    currentContainer = coleSkillsContainers;
                else if (currentHero is Eleanor)
                    currentContainer = eleanorSkillsContainers;
                else
                    currentContainer = joulietteSkillsContainers;
            }

            // if we already created this hero's specific technique buttons, just add them to the buttons list
            if (currentContainer.GetComponentsInChildren<Button>().Length > 0)
            {
                foreach (var b in currentContainer.GetComponentsInChildren<Button>())
                    listOfButtons.Add(b);

                var containerButtons = currentContainer.GetComponentsInChildren<ListButton>();
                // but we also need to update the pm cost
                for (int i = 0; i < containerButtons.Length; i++)
                {
                    var listButton = containerButtons[i];
                    listButton.RefreshPMCost();

                    // assign attacks
                    var button = listButton.GetComponentInChildren<Button>();
                    var tech = listButton.ThisTech;

                    button.onClick.RemoveAllListeners();

                    if (currentHero.EnoughPm(tech))
                    {
                        var attack = tech.Name;
                        button.interactable = true;
                        button.onClick.AddListener(() => OnSelect(attack));
                    }
                    else
                    {
                        button.interactable = false;
                    }
                }
            }
            else
            {
                List<Technique> category = new List<Technique>();// = currentHero.SpellsList;

                if(battleManager.menuState == MenuState.SKILLS)
                {
                    foreach (var skill in currentHero.SkillsList)
                        category.Add(skill);
                }
                else if(battleManager.menuState == MenuState.SPELLS)
                {
                    foreach (var spell in currentHero.SpellsList)
                        category.Add(spell);
                }

                for (int i = 0; i < category.Count; i++)
                {
                    var item = Instantiate(slotPrefab);
                    //item.name = category[i].Name + "_Button";
                    item.transform.SetParent(currentContainer.transform);
                    item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * -slotDistance);
                    item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                    // assign variables of UI
                    item.GetComponent<ListButton>().SetTechnique(category[i]);

                    var button = item.GetComponentInChildren<Button>();
                    button.GetComponentInChildren<Text>().text = category[i].Name;
                    listOfButtons.Add(button);

                    // assign attacks
                    button.onClick.RemoveAllListeners();

                    if (currentHero.EnoughPm(category[i]))
                    {
                        var attack = category[i].Name;
                        button.interactable = true;
                        button.onClick.AddListener(() => OnSelect(attack));
                    }
                    else
                    {
                        button.interactable = false;
                    }

                    //var attack = category[i].Name;
                    //button.onClick.AddListener(() => OnSelect(attack));

                    // set description
                    button.GetComponent<Description>().SetDescription(category[i].Description);

                }
            }
            SetButtonNavigation();
            AddListeners();
        }

        void OnSelect(string attack)
        {
            battleManager.DetermineTargetType(attack);

            // if attack menu, check if we have enough PM for the technique
            // if we do, proceed to targetMenu
            // if not (and if the player has not turned off that menu (add that functionality later)), 
            // show a notification prompt asking if the player REALLY wants to use the technique
            //if (battleManager.menuState != MenuState.ITEMS
            //    && !battleManager.CurrentHero.EnoughPm)
            //{
            //    OpenNotEnoughPMPrompt();
            //}
            //else
            //{
            //    OpenTarget();
            //}
            OpenTarget();
        }        

        void OpenNotEnoughPMPrompt()
        {
            string message;
            var currentAttack = battleManager.CurrentDenigen.CurrentAttack;
            message = "You currently do not have enough power magic to use this technique:\n\n";
            message += currentAttack.Name + ": " + currentAttack.Pm;
            message += "\nPM: " + battleManager.CurrentDenigen.Pm;
            message += "\n\nAre you sure you want to try to use this technique?";
            uiManager.PushConfirmationMenu(message, OpenTarget);
        }

        void OpenTarget()
        {
            uiManager.HideAllMenus();
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

        void OnEnable()
        {
            if (currentContainer != null)
                currentContainer.SetActive(true);
        }
        void OnDisable()
        {
            currentContainer.SetActive(false);
        }

        new void Update()
        {
            base.Update();

            if (currentObj == EventSystem.current.currentSelectedGameObject
                || uiManager.menuInFocus != this.gameObject)
                return;

            currentObj = EventSystem.current.currentSelectedGameObject;
            rootButton = currentObj.GetComponentInChildren<Button>();
            //print("ListSub: currentObj: " + currentObj);
            //print("ListSub: rootButton: " + rootButton);
            UpdateButtonStatInfo();

            if (CheckIfOffScreen(currentObj))
                OutsideOfView(currentObj);
        }

        void UpdateButtonStatInfo()
        {
            if (battleManager.menuState == MenuState.SPELLS)
            {
                foreach (var spell in battleManager.CurrentDenigen.SpellsList)
                {
                    if (string.Equals(rootButton.GetComponentInChildren<Text>().text, spell.Name))
                        SetTechStats(spell);
                }
            }
            else if (battleManager.menuState == MenuState.SKILLS)
            {
                foreach (var skill in battleManager.CurrentDenigen.SkillsList)
                {
                    if (string.Equals(rootButton.GetComponentInChildren<Text>().text, skill.Name))
                        SetTechStats(skill);
                }
            }
            else
            {
                foreach (var item in gameControl.consumables)
                {
                    if (string.Equals(rootButton.GetComponentInChildren<Text>().text, item.GetComponent<Item>().name))
                        SetItemStats(item.GetComponent<Item>() as ConsumableItem);
                }
            }
        }        

        void SetTechStats(Technique tech)
        {
            // show objects
            damageObj.SetActive(true);
            accuracyObj.SetActive(true);
            critObj.SetActive(true);

            // update images
            _damageImage.sprite = damageIcon;

            SetTargetSprite(tech);

            // update text
            damageText.text = tech.Damage.ToString();
            accuracyText.text = tech.Accuaracy.ToString();
            critText.text = tech.Critical.ToString();
        }
        void SetItemStats(ConsumableItem item)
        {
            // hide objects
            accuracyObj.SetActive(false);
            critObj.SetActive(false);

            if (item.hpChange > 0)
            {
                _damageImage.sprite = hpIcon;
                damageText.text = item.hpChange.ToString();
            }
            else if (item.pmChange > 0)
            {
                _damageImage.sprite = pmIcon;
                damageText.text = item.pmChange.ToString();
            }
            else
            {
                _damageImage.sprite = reviveIcon;
                damageText.text = "";
            }            
        }

        void SetTargetSprite(Technique tech)
        {
            if (tech.targetType == TargetType.HERO_SELF)
                targetType.sprite = selfTarget;
            else if (tech.targetType == TargetType.ENEMY_SINGLE || tech.targetType == TargetType.HERO_SINGLE)
                targetType.sprite = singleTarget;
            else if (tech.targetType == TargetType.ENEMY_SPLASH || tech.targetType == TargetType.HERO_SPLASH)
                targetType.sprite = splashTarget;
            else if (tech.targetType == TargetType.ENEMY_TEAM || tech.targetType == TargetType.HERO_TEAM)
                targetType.sprite = teamTarget;
        }
    }
}