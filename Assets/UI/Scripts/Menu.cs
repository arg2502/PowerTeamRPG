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
        protected Button firstButton;
        public List<Button> listOfButtons;

        public virtual void Init()
        {
            gameControl = GameControl.control;
            //uiManager = GameControl.UIManager;
            uiManager = tempControl.UIManager;
            uiDatabase = uiManager.UIDatabase;
            AddListeners();
            AddButtons();
            firstButton = AssignFirstButton();
            gameObject.SetActive(true);
            AssignEventToFirst();
        } 
        protected virtual void AddListeners() { }
        protected virtual void AddButtons() { listOfButtons = new List<Button>(); }
        public void ToggleButtonState(bool isInteractable)
        {
            foreach (var button in listOfButtons)
                button.interactable = isInteractable;
        }
        public virtual Button AssignFirstButton() { return firstButton; }
        public Button FirstButton { get { return firstButton; } set { firstButton = value; } }
        protected void AssignEventToFirst()
        {
            if(firstButton == null) { Debug.LogError("You forgot to set FirstButton."); return; }
            //uiManager.eventSystem.firstSelectedGameObject = firstButton.gameObject;
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
            print(EventSystem.current);
        }
        void Update()
        {
            //if (Input.GetKeyUp(gameControl.backKey))
            if(Input.GetKeyUp(KeyCode.Backspace))
                uiManager.PopMenu();
        }   
    }
}