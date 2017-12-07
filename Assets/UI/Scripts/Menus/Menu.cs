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

        public virtual void Init()
        {
            gameControl = GameControl.control;
            uiManager = GameControl.UIManager;
            //uiManager = tempControl.UIManager;
            uiDatabase = uiManager.UIDatabase;
            AddButtons();
            AddListeners();
            rootButton = AssignRootButton();
            SetButtonNavigation();
            TurnOnMenu();
        } 
        public virtual void TurnOnMenu()
        {
            gameObject.SetActive(true);
            AssignEventToRoot();
        }
        protected virtual void AddListeners()
        {
            if (listOfButtons == null) return;
            foreach(Button button in listOfButtons)
            {
                // ADD AN EVENT TRIGGER AND SET FUNCTION UP HERE
                var trigger = button.gameObject.AddComponent<EventTrigger>();
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((baseEventData)=>AssignDescription(baseEventData));
                trigger.triggers.Add(entry);
            }
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
        protected void AssignEventToRoot()
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
                descriptionText.text = button.GetComponent<Description>().GetDescription(); //button.name;
        }

        protected virtual void SetButtonNavigation()
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
        }

        /// <summary>
        /// for when you left a menu and returned to it, but it was never turned off
        ///some values may need to be reset -- refocus on this current menu
        /// </summary>
        public virtual void Refocus() { }

        protected void Update()
        {
            if (this.gameObject != uiManager.menuInFocus) return;

            if (Input.GetKeyUp(KeyCode.Backspace))
                uiManager.PopMenu();


        }        
    }
}