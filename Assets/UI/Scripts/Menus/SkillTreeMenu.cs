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

        new void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.RightBracket))
                IncreaseHero();
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
                DecreaseHero();
        }
    }
}