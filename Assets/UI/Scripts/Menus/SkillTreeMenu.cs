namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SkillTreeMenu : GridMenu
    {
        public List<GameObject> rowParentList;
        SkillTreeManager treeManager;
        SkillTree.MyTree currentTree;
        public int currentHero;
        int currentTreeIndex;
        public List<Button> treeOptionsList;
        public GameObject treeLineObj;
        List<GameObject> linesList = new List<GameObject>();
        public ButtonSprites buttonSprites;

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

        public override void Init()
        {
            base.Init();

            treeManager = new SkillTreeManager();
            currentHero = 0; // TEMP
            AssignTree(0, currentHero);
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
            treeManager.SetHero(hero); // TEMP
            Debug.Log(treeManager.currentSkillTree);
            currentTree = treeManager.currentSkillTree.listOfTrees[currentTreeIndex];

            RootButton = buttonGrid[currentTree.rootCol][currentTree.rootRow];

            foreach (var b in treeOptionsList)
                b.transform.localScale = Vector3.one;
            treeOptionsList[currentTreeIndex].transform.localScale = Vector3.one * 1.25f;


            SetTechniquesToButtons();
        }

        void SetTechniquesToButtons()
        {
            // deactivate all buttons first
            foreach(var list in buttonGrid)
            {
                foreach (var button in list)
                    button.gameObject.SetActive(false);
            }

            // deactivate all lines too, if any
            foreach(var line in linesList)
            {
                line.SetActive(false);
            }


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
            AssignTree(0, hero);
            SetSelectedObjectToRoot();
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
                    Debug.LogError("MISSING PREREQUISITES");
                    return;
                }
            }

            // if the hero does not have enough tech points, ignore (and display message)
            if(gameControl.heroList[currentHero].techPts < tech.Cost)
            {
                // just debug log for now
                Debug.LogError("YOU DO NOT HAVE ENOUGH POINTS");
                Debug.Log("Hero points: " + gameControl.heroList[currentHero].techPts);
                Debug.Log("Technique cost: " + tech.Cost);
                return;
            }

            // otherwise, we're good to buy the technique
            treeManager.AddTechnique(gameControl.heroList[currentHero], tech);
            gameControl.heroList[currentHero].techPts -= tech.Cost; // reduce points
            SetButtonState(button, tech, true); // change button appearance
        }

        new void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.RightBracket))
                IncreaseHero();
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
                DecreaseHero();

            if (Input.GetKeyUp(KeyCode.T))
            {
                Debug.Log("YOU PRESSED 'T' AND GOT A TECH POINT. YOU CHEATING BASTARD");
                gameControl.heroList[currentHero].techPts++;
                Debug.Log(gameControl.heroList[currentHero].name + " tech points: " + gameControl.heroList[currentHero].techPts);
            }
        }
    }
}