namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;
    using System.Collections.Generic;

    public class GridMenu : Menu
    {
        protected List<List<Button>> buttonGrid;
        protected Navigation navigation;

        protected override void AddListeners()
        {
            base.AddListeners();
            foreach(var list in buttonGrid)
            {
                foreach(var b in list)
                {
                    AddDescriptionEvent(b);
                }
            }
        }
        public override void SetButtonNavigation()
        {
            for (int listIterator = 0; listIterator < buttonGrid.Count; listIterator++)
            {               
                for (int buttonIterator = 0; buttonIterator < buttonGrid[listIterator].Count; buttonIterator++)
                {
                    navigation = buttonGrid[listIterator][buttonIterator].navigation;
                    navigation.mode = Navigation.Mode.Explicit;

                    // TODO:
                    // Add a case if you are going to allow horizontal movement or not
                    // Ex: Inventory does allow, but puts you back to the top of the next list
                    // But Skill Tree will not allow horizontal movement at all

                    SetHorizontalNavigation(buttonIterator, listIterator);
                    SetVerticalNavigation(buttonIterator, listIterator);                   

                    buttonGrid[listIterator][buttonIterator].navigation = navigation;
                }
            }
        }

        protected virtual void SetHorizontalNavigation(int buttonIterator, int listIterator)
        {
            // setting horizontal movement between each list                    
            if (listIterator > 0 && buttonGrid[listIterator - 1].Count > 0)
                navigation.selectOnLeft = buttonGrid[listIterator - 1][buttonIterator];
            if (listIterator < buttonGrid.Count - 1 && buttonGrid[listIterator + 1].Count > 0)
                navigation.selectOnRight = buttonGrid[listIterator + 1][buttonIterator];            
        }
        protected virtual void SetVerticalNavigation(int buttonIterator, int listIterator)
        {
            // setting vertical movement within each list
            if (buttonIterator > 0)
                navigation.selectOnUp = buttonGrid[listIterator][buttonIterator - 1];
            if (buttonIterator < buttonGrid[listIterator].Count - 1)
                navigation.selectOnDown = buttonGrid[listIterator][buttonIterator + 1];
        }
    }
}