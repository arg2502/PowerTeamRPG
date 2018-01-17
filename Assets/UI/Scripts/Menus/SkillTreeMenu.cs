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
    }
}