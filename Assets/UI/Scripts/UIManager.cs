namespace UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class UIManager
    {
        public UIDatabase uiDatabase;
        public Dictionary<Menu, GameObject> dictionary_existingMenus;
        public List<GameObject> list_currentMenus;
        public GameObject menuInFocus; // the menu the player is currently on
        Canvas canvas;
        public bool poppable;

        public UIManager()
        {
            uiDatabase = Resources.Load<UIDatabase>("Databases/UIDatabase");
            dictionary_existingMenus = new Dictionary<Menu, GameObject>();
            list_currentMenus = new List<GameObject>();

            // assign canvas and eventsystem, but make sure they exist
            SetCanvas();
            if (canvas == null) Debug.LogError("You need to add a canvas to the scene.");

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearLists;

        }

        void SetCanvas()
        {
            //canvas = GameObject.FindObjectOfType<Canvas>(); // this should probably be set in scene or another way. What if we can multiple canvases for some reason?
            var canvasObj = GameObject.FindGameObjectWithTag("MainCanvas");
            if (canvasObj == null)
                Debug.LogError("Canvas not found. Did you add the 'MainCanvas' tag to the Canvas obj?");
            else
                canvas = canvasObj.GetComponent<Canvas>();
        }

        public Menu CurrentMenu
        {
            get
            {
                var list = list_currentMenus;
                var count = list.Count;
                var latestMenu = list[count - 1].GetComponent<Menu>();
                return latestMenu;
            }

        }

        public Menu FindMenu(GameObject databaseMenuToFind)
        {
            var tempMenu = databaseMenuToFind.GetComponent<Menu>();

            // back out if we don't have the menu yet
            if (!dictionary_existingMenus.ContainsKey(tempMenu))
                return null;

            var menuToReturn = dictionary_existingMenus[tempMenu].GetComponent<Menu>();

            if (menuToReturn != null)
                return menuToReturn;
            else
            {
                if (databaseMenuToFind.GetComponent<Menu>() == null)
                    Debug.LogError("Menu not found in Database");
                else if (!dictionary_existingMenus.ContainsKey(tempMenu))
                    Debug.LogError("Menu not found in Dictionary");
                else
                    Debug.LogError("Unknown error. Returned null menu");

                return null;
            }
        }

        public void ActivateMenu(GameObject menuPrefab, bool sub = false)
        {
            EnableMenu(menuPrefab, sub);
            InitMenu(menuPrefab.GetComponent<Menu>());
        }

        /// <summary>
        /// For turning menus on and off (visible/invisible). Ex: Pausing and Unpausing the game 
        /// </summary>
        /// <param name="menuPrefab"></param>
        public void EnableMenu(GameObject menuPrefab, bool sub = false)
        {
            //Debug.Log("enable menu");
            var menuToEnable = menuPrefab.GetComponent<Menu>();

            GameObject menuObj;
            // check if we have already have this menu in the scene. If we do, enable it
            if (dictionary_existingMenus.ContainsKey(menuToEnable)
                && dictionary_existingMenus[menuToEnable] != null)
            {
                menuObj = dictionary_existingMenus[menuToEnable];
                //menuObj.GetComponent<Menu>().Init();
                if (sub) AssignSubPosition(menuObj);
                list_currentMenus.Add(menuObj);

                //menuObj.GetComponent<Menu>().TurnOnMenu();
            }
            // if we don't already have one, create it
            else
            {
                if (canvas == null)
                    SetCanvas();
                menuObj = GameObject.Instantiate(menuPrefab);
                menuObj.transform.SetParent(canvas.transform, false);
                menuObj.transform.localPosition = Vector3.zero;
                if (sub) AssignSubPosition(menuObj);

                // add to dictionary if not already there
                // if key exists, assign the newly created menu to the dictionary
                // a little hacky, but oh well...it fixes the issue of menus not existing when you return to the scene
                //if (!dictionary_existingMenus.ContainsKey(menuToEnable))
                    dictionary_existingMenus.Add(menuToEnable, menuObj);
               // else
                    //dictionary_existingMenus[menuToEnable] = menuObj;

                list_currentMenus.Add(menuObj);

                //menuObj.GetComponent<Menu>().Init();
            }
            // set this menu as the one in focus (currently on)
            menuInFocus = menuObj;

        }

        void InitMenu(Menu menu)
        {
            var menuObj = dictionary_existingMenus[menu];
            menuObj.GetComponent<Menu>().Init();
        }

        /// <summary>
        /// Turning off all menus currently in the scene
        /// </summary>
        /// <param name="menuPrefab"></param>
        public void DisableAllMenus()
        {
            HideAllMenus();

            // clear list
            list_currentMenus.Clear();

            // set focus to null
            menuInFocus = null;
        }

        /// <summary>
        ///  // for when we want to go to another menu, but simply deactivate/gray the other menu, not disable/turn invisible.
        ///  Ex: Pause Menu -> Sub Menus
        /// </summary>
        public void PushMenu(GameObject menuPrefab, Menu parentMenu = null)
        {
            if (parentMenu)
            {
                parentMenu.RootButton = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
                //parentMenu.ToggleButtonState(false);
                //EnableMenu(menuPrefab);
                ActivateMenu(menuPrefab);
            }
            else
            {
                //EnableMenu(menuPrefab);
                ActivateMenu(menuPrefab);
            }
        }
        public void PopMenu()
        {
            if (list_currentMenus.Count > 1)
            {
                var lastPos = list_currentMenus.Count - 1;
                var menu = list_currentMenus[lastPos].GetComponent<Menu>();
                menu.Close();
                //list_currentMenus[lastPos].SetActive(false);
                menu.RootButton = menu.AssignRootButton();
                list_currentMenus.RemoveAt(lastPos);
                lastPos = list_currentMenus.Count - 1;
                menuInFocus = list_currentMenus[lastPos];
                menu = menuInFocus.GetComponent<Menu>();
                menu.gameObject.SetActive(true);
                menu.SetSelectedObjectToRoot();
                //menu.ToggleButtonState(true);
                menu.Refocus();
            }
            else
                DisableAllMenus();
        }

        void AssignSubPosition(GameObject menuObj)
        {
            // if the menu is a sub menu, base it's position off of the parent's button that 
            var parentMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<Menu>();
            menuObj.transform.SetParent(parentMenu.transform);
            menuObj.transform.localPosition = Vector3.zero;

            var rootButtonPos = parentMenu.RootButton.transform.position;
            var buttonWidth = parentMenu.RootButton.GetComponent<RectTransform>().rect.width;
            menuObj.transform.position = new Vector3(rootButtonPos.x + buttonWidth, rootButtonPos.y);

        }

        public void PushConfirmationMenu(string messageText, Action yesAction, Action noAction = null)
        {
            EnableMenu(uiDatabase.ConfirmationMenu);
            var confirmMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<ConfirmationMenu>();
            confirmMenu.specificText.text = messageText;
            confirmMenu.yesAction = yesAction;
            confirmMenu.noAction = noAction;
            //InitMenu(confirmMenu);
            confirmMenu.Init();
        }

        public void HideAllMenus()
        {
            // turn off all menus
            foreach (var menu in list_currentMenus)
                menu.SetActive(false);
        }
        public void ShowAllMenus()
        {
            foreach (var menu in list_currentMenus)
                menu.SetActive(true);
        }

        // clear the current menu list and dictionary of existing menu objects
        // should be called on scene changes when the menu objects are destroyed
        public void ClearLists(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene last)
        {
            list_currentMenus.Clear();
            dictionary_existingMenus.Clear();
        }
    }
}