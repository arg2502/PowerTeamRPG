namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SkillTreeMenu : GridMenu
    {
        public Text costText;
        public Text prereqText;
        public Text heroTechText;
        public List<GameObject> rowParentList;
        SkillTreeManager treeManager;
        SkillTree.MyTree currentTree;
        public int currentHero;
        int currentTreeIndex;
        public List<Button> treeOptionsList;
        public GameObject treeLineObj;
        List<GameObject> linesList = new List<GameObject>();
        public ButtonSprites buttonSprites;
        public GameObject heroListGroup;
        public Toggle[] heroListToggles;
        bool canSwitchCharacters = true;
        Dictionary<Button, Technique> buttonTechniqueLinker = new Dictionary<Button, Technique>();
        TeamInfoSub teamInfoSub;
        Button currentButton;

        [Header("Warning Texts")]
        public GameObject costWarning;
        public GameObject prereqWarning;
        float warningTime = 3f;

        Technique techniqueToAdd;
        Button buttonToChange;

        [Serializable]
        public struct ButtonSprites
        {
            [Header("Normal")]
            public Sprite normal;
            public Sprite hover;
            public Sprite pressed;

            [Header("Disabled")]
            public Sprite dis_normal;
            public Sprite dis_hover;
            public Sprite dis_pressed;
        }
        
        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            treeManager = GameControl.skillTreeManager;
        }

        public void SetHero(int heroIndex)
        {
            // the hero to show first is stored inside TeamInfoSub where you selected the hero
            //teamInfoSub = uiManager.FindMenu(uiDatabase.TeamInfoSub) as TeamInfoSub;

            // instantly move list to chosen hero
            MoveHeroList(heroIndex, true);

            //currentHero = teamInfoSub.currentHero; // kinda redundant cause currentHero is set inside AssignTree, but oh whale
            currentHero = heroIndex;
            AssignTree(0, currentHero);
            SetSelectedObjectToRoot();
            ToggleTextChange();
        }

        protected override void AddButtons()
        {
            buttonGrid = new List<List<Button>>();

            foreach(var parent in rowParentList)
            {
                var list = new List<Button>();
                var row = parent.GetComponentsInChildren<Button>();
                
                foreach (var button in row)
                    list.Add(button);

                buttonGrid.Add(list);
            }

            foreach(var b in treeOptionsList)
                AddDescriptionEvent(b);
        }

        public override Button AssignRootButton()
        {
            return buttonGrid[0][0]; // TEMP
        }
        
        void AssignTree(int treeIndex, int hero)
        {
            currentTreeIndex = treeIndex;
            currentHero = hero;
            treeManager.SetHero(hero);
            //Debug.Log(treeManager.currentSkillTree);
            currentTree = treeManager.currentSkillTree.listOfTrees[currentTreeIndex];

            RootButton = buttonGrid[currentTree.rootCol][currentTree.rootRow];
            currentButton = RootButton;

            foreach (var b in treeOptionsList)
                b.transform.localScale = Vector3.one;
            treeOptionsList[currentTreeIndex].transform.localScale = Vector3.one * 1.25f;


            SetTechniquesToButtons();

            // set tech points text
            SetHeroTechText();
        }

        void SetTechniquesToButtons()
        {
            // deactivate all buttons first -- AND REMOVE THEIR LISTENERS
            foreach(var list in buttonGrid)
            {
                foreach (var button in list)
                {
                    button.onClick.RemoveAllListeners();
                    button.gameObject.SetActive(false);
                }
            }

            // deactivate all lines too, if any
            foreach(var line in linesList)
            {
                line.SetActive(false);
            }

            // clear linker dictionary
            buttonTechniqueLinker.Clear();

            foreach(var technique in currentTree.listOfContent)
            {
                // activate buttons that we need
                buttonGrid[technique.ColPos][technique.RowPos].gameObject.SetActive(true);

                var button = buttonGrid[technique.ColPos][technique.RowPos];

                // set description
                button.GetComponent<Description>().description = 
                    "<b>" + technique.Name + "</b>\n\n" + technique.Description;

                // set image
                var icon = button.GetComponentsInChildren<Image>()[1];
                if (technique.TreeImage != null)
                    icon.sprite = technique.TreeImage;
                else
                    icon.sprite = treeManager.imageDatabase.defaultSprite;

                // TECHNIQUE STATES
                // if we have the technique in our lists, mark it as Active & show appropriate sprites                                
                if (treeManager.HasTechnique(gameControl.heroList[currentHero], technique))
                {
                    SetButtonState(button, technique, true);
                }
                // otherwise -- we don't have it yet, mark as inactive
                else
                {
                    SetButtonState(button, technique, false);
                }

                // LISTENERS
                // instead of adding the listeners through `override AddListeners()`,
                // we can add the listeners here and pass in the technique (while we have the info)..
                // to link the button and technique together

                // IMPORTANT -- variable needs to be copied before passing into lambda. 
                // If `technique` is passed directly, it would only refer to the last technique in the list
                // *A weird quirk of lambda functions*
                var tech = technique; 
                button.onClick.AddListener(() => OnPurchaseTech(tech, button));
                buttonTechniqueLinker.Add(button, tech);

                // LINES
                // find NEXT techniques
                if (technique.ListNextTechnique != null)
                {
                    foreach (var t in technique.ListNextTechnique)
                    {
                        if (technique.treeLinesDictionary.ContainsKey(t))
                        {
                            technique.treeLinesDictionary[t].SetActive(true);
                        }
                        else
                        {
                            var nextPos = buttonGrid[t.ColPos][t.RowPos].transform.position;
                            //Debug.DrawLine(button.transform.position, nextPos, Color.red, 10f);
                            var line = (GameObject)Instantiate(treeLineObj, button.transform.position, Quaternion.identity);
                            line.transform.SetParent(transform.Find("TreeGrid").GetChild(0));
                            //line.GetComponent<LineRenderer>().SetPositions(new Vector3[] { button.transform.position, nextPos });
                            Vector3 differenceVector = nextPos - button.transform.position;
                            var imageRectTransform = line.GetComponent<Image>().rectTransform;
                            var lineWidth = 1f;
                            imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
                            imageRectTransform.pivot = new Vector2(0, 0.5f);
                            imageRectTransform.position = button.transform.position;
                            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
                            imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);

                            technique.treeLinesDictionary.Add(t, line);
                            linesList.Add(line);
                        }
                    }
                }
            }

            // reset new navigation
            SetButtonNavigation();
        }

        void OnPurchaseTech(Technique tech, Button button)
        {
            Debug.Log("Want to purchase: " + tech.Name);
            // if the technique is already active, there's nothing else to do
            if (tech.Active) return;

            // if the hero doesn't have all the prerequisites, ignore (and display message)
            if (tech.Prerequisites != null)
            {
                var prereqCount = 0;
                foreach (var prereq in tech.Prerequisites)
                {
                    if (treeManager.HasTechnique(gameControl.heroList[currentHero], prereq))
                        prereqCount++;
                }
                if (prereqCount < tech.Prerequisites.Count)
                {
                    StartCoroutine(ShowWarning(prereqWarning));
                    return;
                }
            }

            // if the hero does not have enough tech points, ignore (and display message)
            if (gameControl.heroList[currentHero].techPts < tech.TechPointCost)
            {
                StartCoroutine(ShowWarning(costWarning));
                return;
            }

            // otherwise, we're good to buy the technique
            string messageText = "<i>You want to buy:</i>\n\t<b>" + tech.Name + "</b>\n<i>Cost:</i><b>\t" + tech.TechPointCost + "</b>";
            techniqueToAdd = tech;
            buttonToChange = button;
            RootButton = buttonToChange;
            uiManager.PushConfirmationMenu(messageText, PurchaseTechnique);
        }

        void PurchaseTechnique()
        {
            treeManager.AddTechnique(gameControl.heroList[currentHero], techniqueToAdd);
            print("you now have " + techniqueToAdd.Name);
            gameControl.heroList[currentHero].techPts -= techniqueToAdd.TechPointCost; // reduce points
            SetButtonState(buttonToChange, techniqueToAdd, true); // change button appearance
            SetHeroTechText();
            techniqueToAdd = null;
            buttonToChange = null;
        }

        public override void SetButtonNavigation()
        {
            base.SetButtonNavigation();

            navigation = treeOptionsList[0].navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnDown = RootButton;
            navigation.selectOnRight = treeOptionsList[1];
            treeOptionsList[0].navigation = navigation;

            navigation = treeOptionsList[1].navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnDown = RootButton;
            navigation.selectOnLeft = treeOptionsList[0];
            treeOptionsList[1].navigation = navigation;


        }
        protected override void SetHorizontalNavigation(int buttonIterator, int listIterator)
        {
            // ignore if inactive
            if (!buttonGrid[listIterator][buttonIterator].gameObject.activeSelf)
                return;

            // finding the next active button to the left   

            navigation.selectOnLeft = null; // reset

            if (listIterator > 0 && buttonGrid[listIterator - 1].Count > 0)
            {
                for (int index = listIterator - 1; index >= 0; index--)
                {
                    if (buttonGrid[index][buttonIterator].gameObject.activeSelf)
                    {
                        navigation.selectOnLeft = buttonGrid[index][buttonIterator];
                        break;
                    }
                    else
                        navigation.selectOnLeft = null;
                }

            }

            // finding the next active button to the right
            navigation.selectOnRight = null; // reset

            if (listIterator < buttonGrid.Count - 1 && buttonGrid[listIterator + 1].Count > 0)
            {
                for (int index = listIterator + 1; index < buttonGrid.Count; index++)
                {
                    if (buttonGrid[index][buttonIterator].gameObject.activeSelf)
                    {
                        navigation.selectOnRight = buttonGrid[index][buttonIterator];
                        break;
                    }
                    else
                        navigation.selectOnRight = null;
                }
            }
        }

        protected override void SetVerticalNavigation(int buttonIterator, int listIterator)
        {
            // ignore if inactive
            if (!buttonGrid[listIterator][buttonIterator].gameObject.activeSelf)
                return;

            navigation.selectOnUp = null; // reset

            // finding the next active button above
            if (buttonIterator > 0)
            {
                for (int index = buttonIterator - 1; index >= 0; index--)
                {
                    if (buttonGrid[listIterator][index].gameObject.activeSelf)
                    {
                        navigation.selectOnUp = buttonGrid[listIterator][index];
                        break;
                    }
                    else
                        navigation.selectOnUp = null;
                }
            }


            // if there is no button above, set to the treeOptionsList current button
            if (navigation.selectOnUp == null)
                navigation.selectOnUp = treeOptionsList[currentTreeIndex];

            navigation.selectOnDown = null; // reset

            // finding the next active button below
            if (buttonIterator < buttonGrid[listIterator].Count - 1)
            {
                for (int index = buttonIterator + 1; index < buttonGrid[listIterator].Count; index++)
                {
                    if (buttonGrid[listIterator][index].gameObject.activeSelf)
                    {
                        navigation.selectOnDown = buttonGrid[listIterator][index];
                        break;
                    }
                    else
                        navigation.selectOnDown = null;
                }
            }
        }

        /// <summary>
        /// Called when switching between a hero's 2 skill trees
        /// </summary>
        /// <param name="whichTree"></param>
        public void TreeOptionSelect(int whichTree)
        {
            if (whichTree != currentTreeIndex)
                AssignTree(whichTree, currentHero);
        }

        /// <summary>
        /// Called when switching between heroes
        /// </summary>
        /// <param name="hero">GameControl hero index</param>
        void HeroSelect(int hero)
        {
            if (!canSwitchCharacters) return;

            // move hero list before changing currentHero
            MoveHeroList(hero);

            // change the tree grid
            AssignTree(0, hero);
            SetSelectedObjectToRoot();

            // refresh button -- to refresh button sprite state
            var buttonObj = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttonObj);

            // deactivate warnings
            DeactivateWarningTexts();

            // show hero list change
            ToggleTextChange();

        }

        void IncreaseHero()
        {
            if (currentHero < gameControl.heroList.Count - 1)
                HeroSelect(currentHero + 1);
        }
        void DecreaseHero()
        {
            if (currentHero > 0)
                HeroSelect(currentHero - 1);
        }

        void SetButtonState(Button button, Technique technique, bool active)
        {
            if(active)
            {
                button.GetComponent<Image>().sprite = buttonSprites.normal;
                var spriteState = button.spriteState;
                spriteState.highlightedSprite = buttonSprites.hover;
                spriteState.pressedSprite = buttonSprites.pressed;
                spriteState.disabledSprite = buttonSprites.pressed;
                button.spriteState = spriteState;
                technique.Active = true;
            }
            else
            {
                button.GetComponent<Image>().sprite = buttonSprites.dis_normal;
                var spriteState = button.spriteState;
                spriteState.highlightedSprite = buttonSprites.dis_hover;
                spriteState.pressedSprite = buttonSprites.dis_pressed;
                spriteState.disabledSprite = buttonSprites.dis_pressed;
                button.spriteState = spriteState;
                technique.Active = false;
            }
        }
        
        void ToggleTextChange()
        {
            foreach (var toggle in heroListToggles)
                toggle.isOn = false;

            heroListToggles[currentHero].isOn = true;
        }

        void MoveHeroList(int newHeroPos, bool instant = false)
        {
            // find where the text needs to go next
            var desiredPosition = heroListToggles[newHeroPos].transform.position;
            var currentPosition = heroListToggles[currentHero].transform.position;
            var differenceX = -(desiredPosition.x - currentPosition.x);
            var newPosition = new Vector3(heroListGroup.transform.position.x + differenceX, heroListGroup.transform.position.y);

            // move the text over a short period of time -- lerp it
            if (!instant)
                StartCoroutine(LerpText(newPosition));
            else
                heroListGroup.transform.position = newPosition;
            
        }
        
        IEnumerator LerpText(Vector3 newPosition)
        {
            // while we're moving the text, disable the ability to switch to another character's tree
            // this is to avoid a bug where the text gets stuck and doesn't move
            canSwitchCharacters = false;

            var startTime = Time.time;
            var lerpTime = 10f;
            var originalPosition = heroListGroup.transform.position;
            while ((newPosition - heroListGroup.transform.position).magnitude >= 0.1f)
            {
                heroListGroup.transform.position = Vector3.Lerp(originalPosition, newPosition, (Time.time - startTime) * lerpTime);
                yield return null;
            }

            // now that the text is where it needs to be, reenable the ability to switch between trees
            canSwitchCharacters = true;
        }

        void SetHeroTechText()
        {
            heroTechText.text = "Tech Pts: <b>" + gameControl.heroList[currentHero].techPts + "</b>";
        }

        void UpdateText()
        {
            if (EventSystem.current.currentSelectedGameObject == null) return;

            // turn off warnings if we've switched to a different button
            if(currentButton != EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
            {
                DeactivateWarningTexts();
                currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            }

            // if we're not on a tree button, set all text to null
            if (!buttonTechniqueLinker.ContainsKey(EventSystem.current.currentSelectedGameObject.GetComponent<Button>()))
            {
                costText.text = "";
                prereqText.text = "";
                return;
            }

            // save technique
            var tech = buttonTechniqueLinker[EventSystem.current.currentSelectedGameObject.GetComponent<Button>()];

            // refresh description
            descriptionText.text = "<b>" + tech.Name + "</b>\n\n" + tech.Description;

            // set cost text
            costText.text = "Cost: " + tech.TechPointCost;

            // set prerequites, if there are any
            if (tech.Prerequisites != null && tech.Prerequisites.Count > 0)
            {
                prereqText.text = "<b>Prerequisites:</b>";

                foreach (var prereq in tech.Prerequisites)
                    prereqText.text += "\n" + prereq.Name;
            }
            else
                prereqText.text = "";
            
        }
        
        IEnumerator ShowWarning(GameObject textObj)
        {
            // stop if object is already active
            if (textObj.activeSelf) yield break;

            // show warning, wait a couple seconds, then hide again
            textObj.SetActive(true);
            yield return new WaitForSeconds(warningTime);
            textObj.SetActive(false);
        }

        void DeactivateWarningTexts()
        {
            costWarning.SetActive(false);
            prereqWarning.SetActive(false);
        }

        new void Update()
        {
            base.Update();

            if (this.gameObject != uiManager.menuInFocus) return;

            UpdateText();
            

            if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") > 0)
                IncreaseHero();
            else if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") < 0)
                DecreaseHero();            
        }
    }
}