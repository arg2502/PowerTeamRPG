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
        int currentTreeIndex;
        public List<Button> treeOptionsList;

        public override void Init()
        {
            base.Init();

            treeManager = new SkillTreeManager();
            AssignTree(0);
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
        }

        public override Button AssignRootButton()
        {
            return buttonGrid[0][0]; // TEMP
        }

        void AssignTree(int index)
        {
            currentTreeIndex = index;
            treeManager.SetHero(0); // TEMP
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

            foreach(var technique in currentTree.listOfContent)
            {
                // activate buttons that we need
                buttonGrid[technique.ColPos][technique.RowPos].gameObject.SetActive(true);

                var button = buttonGrid[technique.ColPos][technique.RowPos];

                // set description
                button.GetComponent<Description>().description = technique.Description;

                // set image
                var icon = button.GetComponentsInChildren<Image>()[1];
                if (technique.TreeImage != null)
                    icon.sprite = technique.TreeImage;
                else
                    icon.sprite = treeManager.imageDatabase.defaultSprite;
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

        public void TreeOptionSelect(int whichTree)
        {
            if (whichTree != currentTreeIndex)
                AssignTree(whichTree);
        }

    }
}