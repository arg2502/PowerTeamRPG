namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;
    using System.Collections.Generic;

    public class Menu : MonoBehaviour
    {
        protected GameControl gameControl;
        protected UIManager uiManager;
        protected UIDatabase uiDatabase;
        protected Button rootButton;
        protected List<Button> listOfButtons;

        public virtual void Init()
        {
            gameControl = GameControl.control;
            //uiManager = GameControl.UIManager;
            uiManager = tempControl.UIManager;
            uiDatabase = uiManager.UIDatabase;
            AddListeners();
            AddButtons();
            rootButton = AssignRootButton();
            TurnOnMenu();
        } 
        public void TurnOnMenu()
        {
            gameObject.SetActive(true);
            AssignEventToRoot();
        }
        protected virtual void AddListeners() { }
        protected virtual void AddButtons() { }
        public void ToggleButtonState(bool isInteractable)
        {
            foreach (var button in listOfButtons)
                button.interactable = isInteractable;
        }
        public virtual Button AssignRootButton() { return rootButton; }
        public Button RootButton { get { return rootButton; } set { rootButton = value; } }
        protected void AssignEventToRoot()
        {
            if(rootButton == null) { Debug.LogError("You forgot to set FirstButton."); return; }
            //uiManager.eventSystem.firstSelectedGameObject = firstButton.gameObject;
            EventSystem.current.SetSelectedGameObject(rootButton.gameObject);
            print(EventSystem.current);
        }
        void Update()
        {
            //if (Input.GetKeyUp(gameControl.backKey))
            if(Input.GetKeyUp(KeyCode.Backspace) && uiManager.menuInFocus == this.gameObject)
                uiManager.PopMenu();
        }   
    }
}