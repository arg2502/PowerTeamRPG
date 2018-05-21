namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;
    using System.Collections.Generic;
    using System;

    public class Menu : MonoBehaviour
    {
        protected GameControl gameControl;
        protected UIManager uiManager;
        protected UIDatabase uiDatabase;
        protected Button rootButton;
        protected List<Button> listOfButtons;
        public Text descriptionText;

        /// <summary>
        /// When first instantiating a menu
        /// </summary>
        public virtual void Init()
        {
            if(gameControl != null)
            {
                TurnOnMenu();
                return;
            }
            
            gameControl = GameControl.control;
            uiManager = GameControl.UIManager;
            uiDatabase = uiManager.uiDatabase;
            AddButtons();
            AddListeners();
            rootButton = AssignRootButton();
            SetButtonNavigation();
            TurnOnMenu();
        } 

        /// <summary>
        /// When the menu is already created and you just want to turn it back on
        /// </summary>
        public virtual void TurnOnMenu()
        {
            gameObject.SetActive(true);
            SetSelectedObjectToRoot();
            transform.SetAsLastSibling();     
        }

        protected virtual void AddListeners()
        {
            if (listOfButtons == null) return;
            foreach(Button button in listOfButtons)
            {
                AddDescriptionEvent(button);
            }
        }

        protected void AddDescriptionEvent(Button button)
        {
            // ADD AN EVENT TRIGGER AND SET FUNCTION UP HERE
            var trigger = button.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((baseEventData) => AssignDescription(baseEventData));
            trigger.triggers.Add(entry);
        }

        protected virtual void AddButtons() { }
        public void ToggleButtonState(bool isInteractable)
        {
            if (listOfButtons == null) return;
            foreach (var button in listOfButtons)
                button.interactable = isInteractable;
        }
        public virtual Button AssignRootButton() { return rootButton; }
        public Button RootButton { get { return rootButton; } set { rootButton = value; } }
        public void SetSelectedObjectToRoot()
        {
            if(rootButton == null) { Debug.LogError("You forgot to set FirstButton."); return; }            
            if (rootButton.gameObject == EventSystem.current.currentSelectedGameObject) return;
            EventSystem.current.SetSelectedGameObject(rootButton.gameObject);
        }

        //public void AssignDescription(Button button)
        public void AssignDescription(BaseEventData bed = null)
        {
            GameObject button;

            if (bed == null)
                button = EventSystem.current.currentSelectedGameObject;
            else
                button = bed.selectedObject;

            var descriptionComponent = button.GetComponent<Description>();
            if (descriptionText != null && button != null && descriptionComponent != null && !descriptionComponent.noDescription)
                descriptionText.text = button.GetComponent<Description>().GetDescription();
        }

        public virtual void SetButtonNavigation()
        {
            for(int i = 0; i < listOfButtons.Count; i++)
            {
                var navigation = listOfButtons[i].navigation;
                navigation.mode = Navigation.Mode.Explicit;
                
                // set where the menu will go when 
                if (i > 0)
                    navigation.selectOnUp = listOfButtons[i - 1];
                if (i < listOfButtons.Count - 1)
                    navigation.selectOnDown = listOfButtons[i + 1];
                

                listOfButtons[i].navigation = navigation;
            }

            CheckForInactiveButtons();
        }

        /// <summary>
        /// for when you left a menu and returned to it, but it was never turned off
        ///some values may need to be reset -- refocus on this current menu
        /// </summary>
        public virtual void Refocus() { }

        /// <summary>
        /// Called right as you leave a menu -- so objects aren't left selected that shouldn't be
        /// </summary>
        public virtual void Close() { }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
                uiManager.poppable = true;

            if (this.gameObject != uiManager.menuInFocus) return;
            
            if (Input.GetKeyUp(KeyCode.Backspace)
                && !ExceptionMenus()
                && uiManager.poppable)
            {
                uiManager.PopMenu();
                uiManager.poppable = false;
            }

        }

        /// <summary>
        /// If true, then this menu cannot be popped. Ex: You cannot back out of the battle menu
        /// </summary>
        /// <returns></returns>
        bool ExceptionMenus()
        {
            if (this.gameObject.GetComponent<BattleMenu>())
                return true;
            else
                return false;
        }

        void CheckForInactiveButtons()
        {
            // check for any inactive buttons
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                if (!listOfButtons[i].interactable)
                {
                    //Debug.Log(i + ": not interactable");
                    // if button is at top
                    if (i <= 0)
                    {
                        var navigation = listOfButtons[i + 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        navigation.selectOnUp = null;

                        listOfButtons[i + 1].navigation = navigation;
                    }

                    // if button is at bottom
                    else if (i >= listOfButtons.Count - 1)
                    {
                        var navigation = listOfButtons[i - 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        navigation.selectOnDown = null;

                        listOfButtons[i - 1].navigation = navigation;
                    }

                    // if button is in middle of menu
                    //if ((i - 1) >= 0 && (i + 1) <= listOfButtons.Count - 1)
                    else
                    {
                        // set navigation for button above
                        var navigation = listOfButtons[i - 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        var button = FindNextActiveButton(i, +1);
                        navigation.selectOnDown = button;

                        listOfButtons[i - 1].navigation = navigation;
                        //}
                        //if((i + 1) <= listOfButtons.Count - 1 && (i-1) >= 0)
                        //{
                        // set navigation for button below
                        navigation = listOfButtons[i + 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        button = FindNextActiveButton(i, -1);
                        navigation.selectOnUp = button;

                        listOfButtons[i + 1].navigation = navigation;
                    }
                }
            }
        }   
        
        Button FindNextActiveButton(int origin, int increment)
        {
            var index = origin + increment;
            
            while(index >= 0 && index <= listOfButtons.Count - 1)
            {
                if (listOfButtons[index].interactable)
                    return listOfButtons[index];
                else
                    index += increment;
            }

            return null;

        }  
    }
}