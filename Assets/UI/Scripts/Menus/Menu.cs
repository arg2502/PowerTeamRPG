﻿namespace UI
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
        protected Animator animator;
        protected float animationSpeed = 1.75f;
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

            animator = GetComponent<Animator>();
            gameControl = GameControl.control;
            uiManager = GameControl.UIManager;
            uiDatabase = uiManager.uiDatabase;
            AddButtons();
            AddListeners();
            rootButton = AssignRootButton();
            SetButtonNavigation();
            TurnOnMenu();
        } 

        void FadeIn()
        {
            StopAllCoroutines(); // stops menu from fading out (if the menu was backed out of and then quickly selected again)

            if (animator == null) return;

            animator.speed = animationSpeed;
            animator.Play("TurnOn");
        }

        IEnumerator FadeOut()
        {
            if (animator != null)
            {
                animator.speed = animationSpeed;
                animator.Play("TurnOff");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// When the menu is already created and you just want to turn it back on
        /// </summary>
        public virtual void TurnOnMenu()
        {
            gameObject.SetActive(true);
            FadeIn();
            SetSelectedObjectToRoot();
            transform.SetAsLastSibling();     
        }

        protected virtual void AddListeners()
        {
            if (listOfButtons == null) return;
            foreach(Button button in listOfButtons)
            {
                // Add an EventTrigger Component to the button here
                button.gameObject.AddComponent<EventTrigger>();

                // These functions add specific triggers based on certain actions performed with the buttons
                AddDescriptionEvent(button); // Changes the description text obj when the button is highlighted
                AddButtonSounds(button); // plays button sfx when highlighted/pressed
            }
        }

        protected void AddDescriptionEvent(Button button)
        {
            // ADD AN EVENT TRIGGER AND SET FUNCTION UP HERE
            var trigger = button.gameObject.GetComponent<EventTrigger>(); // get EventTrigger if already existing
            if (!trigger) // if there is no event trigger already, add one
                trigger = button.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((baseEventData) => AssignDescription(baseEventData));
            trigger.triggers.Add(entry);
        }
        
        protected void AddButtonSounds(Button button)
        {
            //var trigger = button.gameObject.GetComponent<EventTrigger>();

            //// Menu nav sfx
            //var entry = new EventTrigger.Entry();
            //entry.eventID = EventTriggerType.Deselect;
            //entry.callback.AddListener((unused) => AudioManager.instance.PlayMenuNav());
            //trigger.triggers.Add(entry);

            //// Menu select sfx
            //entry = new EventTrigger.Entry();
            //entry.eventID = EventTriggerType.Submit;
            //entry.callback.AddListener((unused) => AudioManager.instance.PlayMenuSelect());
            //trigger.triggers.Add(entry);
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
            if(rootButton == null) { /*Debug.LogError("You forgot to set FirstButton.");*/ return; }
            //if (rootButton.gameObject == EventSystem.current.currentSelectedGameObject) return;
            StartCoroutine(SetRoot());
        }
        IEnumerator SetRoot()
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(rootButton.gameObject);
            rootButton.OnSelect(null); // to highlight button

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

        public void SetButtonNavigationHorizontal()
        {
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                var navigation = listOfButtons[i].navigation;
                navigation.mode = Navigation.Mode.Explicit;

                // set where the menu will go when 
                if (i > 0)
                    navigation.selectOnLeft = listOfButtons[i - 1];
                if (i < listOfButtons.Count - 1)
                    navigation.selectOnRight = listOfButtons[i + 1];


                listOfButtons[i].navigation = navigation;
            }

            CheckForInactiveButtons(horizontal : true);
        }

        /// <summary>
        /// for when you left a menu and returned to it, but it was never turned off
        ///some values may need to be reset -- refocus on this current menu
        /// </summary>
        public virtual void Refocus() { }

        /// <summary>
        /// Called right as you leave a menu -- so objects aren't left selected that shouldn't be
        /// </summary>
        public virtual void Close()
        {
            StartCoroutine(FadeOut());
        }

        /// <summary>
        /// Called when backspace is pressed if this menu is an Exception Menu
        /// Use this is you want something to occur right before closing/popping the menu.
        /// </summary>
        public virtual void PrePop() { }

        protected void Update()
        {
            if (Input.GetButtonDown("Back"))
                uiManager.poppable = true;

            if (this.gameObject != uiManager.menuInFocus) return;
            
            if (Input.GetButtonDown("Back")
                && uiManager.poppable)
            {
                if (!ExceptionMenus())
                    uiManager.PopMenu();
                else
                    PrePop();

                uiManager.poppable = false;
            }

        }

        /// <summary>
        /// If true, then this menu cannot be popped. Ex: You cannot back out of the battle menu
        /// </summary>
        /// <returns></returns>
        bool ExceptionMenus()
        {
            if (this.gameObject.GetComponent<BattleMenu>()
                || this.gameObject.GetComponent<VictoryMenu>()
                || this.gameObject.GetComponent<StatPointsMenu>()
                || this.gameObject.GetComponent<DialogueMenu>()
                || this.gameObject.GetComponent<DialogueResponseMenu>()
                )
                return true;
            else
                return false;
        }

        void CheckForInactiveButtons(bool horizontal = false)
        {
            // check for any inactive buttons
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                if (!listOfButtons[i].interactable && listOfButtons.Count > 1)
                {
                    //Debug.Log(i + ": not interactable");
                    // if button is at top
                    if (i <= 0)
                    {
                        var navigation = listOfButtons[i + 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        if (!horizontal) navigation.selectOnUp = null;
                        else navigation.selectOnLeft = null;

                        listOfButtons[i + 1].navigation = navigation;
                    }

                    // if button is at bottom
                    else if (i >= listOfButtons.Count - 1)
                    {
                        var navigation = listOfButtons[i - 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        if (!horizontal) navigation.selectOnDown = null;
                        else navigation.selectOnRight = null;

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

                        if (!horizontal) navigation.selectOnDown = button;
                        else navigation.selectOnRight = button;

                        listOfButtons[i - 1].navigation = navigation;
                        //}
                        //if((i + 1) <= listOfButtons.Count - 1 && (i-1) >= 0)
                        //{
                        // set navigation for button below
                        navigation = listOfButtons[i + 1].navigation;
                        navigation.mode = Navigation.Mode.Explicit;

                        button = FindNextActiveButton(i, -1);

                        if (!horizontal) navigation.selectOnUp = button;
                        else navigation.selectOnLeft = button;

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

        /// <summary>
        /// If there is a menu that needs passed in values after Initialization, call this Refresh function
        /// to reinitialize with the proper values
        /// </summary>
        public void Refresh()
        {
            AddButtons();
            AddListeners();
            rootButton = AssignRootButton();
            SetButtonNavigation();
            TurnOnMenu();
        }
    }
}