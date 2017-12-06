namespace UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections;
    using System.Collections.Generic;

    public class UIManager
    {
        public UIDatabase UIDatabase;
        public Dictionary<Menu, GameObject> dictionary_existingMenus;
        public List<GameObject> list_currentMenus;
        public GameObject menuInFocus; // the menu the player is currently on
        Canvas canvas;

        public UIManager()
        {
            UIDatabase = Resources.Load<UIDatabase>("Databases/UIDatabase");
            dictionary_existingMenus = new Dictionary<Menu, GameObject>();
            list_currentMenus = new List<GameObject>();

            // assign canvas and eventsystem, but make sure they exist
            canvas = GameObject.FindObjectOfType<Canvas>(); // this should probably be set in scene or another way. What if we can multiple canvases for some reason?
            if (canvas == null) Debug.LogError("You need to add a canvas to the scene.");
            
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

        /// <summary>
        /// For turning menus on and off (visible/invisible). Ex: Pausing and Unpausing the game 
        /// </summary>
        /// <param name="menuPrefab"></param>

        public void EnableMenu(GameObject menuPrefab, bool sub = false)
        {
            var menuToEnable = menuPrefab.GetComponent<Menu>();

            GameObject menuObj;
            // check if we have already have this menu in the scene. If we do, enable it
            if(dictionary_existingMenus.ContainsKey(menuToEnable))
            {
                menuObj = dictionary_existingMenus[menuToEnable];
                menuObj.GetComponent<Menu>().TurnOnMenu();
                if (sub) AssignSubPosition(menuObj);
                list_currentMenus.Add(menuObj);
            }
            // if we don't already have one, create it
            else
            {
                menuObj = GameObject.Instantiate(menuPrefab);
                menuObj.transform.SetParent(canvas.transform, false);
                menuObj.transform.localPosition = Vector3.zero;
                if (sub) AssignSubPosition(menuObj); 
                dictionary_existingMenus.Add(menuToEnable, menuObj);
                list_currentMenus.Add(menuObj);

                menuObj.GetComponent<Menu>().Init();
            }
            // set this menu as the one in focus (currently on)
            menuInFocus = menuObj;
            
        }

        /// <summary>
        /// Turning off all menus currently in the scene
        /// </summary>
        /// <param name="menuPrefab"></param>
        public void DisableAllMenus()
        {
            // turn off all menus
            foreach (var menu in list_currentMenus)
                menu.SetActive(false);

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
                parentMenu.ToggleButtonState(false);
                EnableMenu(menuPrefab, true);
            }
            else
            {
                EnableMenu(menuPrefab);
            }
        }
        public void PopMenu()
        {
            if (list_currentMenus.Count > 1)
            {
                var lastPos = list_currentMenus.Count - 1;
                list_currentMenus[lastPos].SetActive(false);
                var menu = list_currentMenus[lastPos].GetComponent<Menu>();
                menu.RootButton = menu.AssignRootButton();
                list_currentMenus.RemoveAt(lastPos);
                lastPos = list_currentMenus.Count - 1;
                menuInFocus = list_currentMenus[lastPos];
                menu = menuInFocus.GetComponent<Menu>();
                EventSystem.current.SetSelectedGameObject(menu.RootButton.gameObject);
                menu.ToggleButtonState(true);
                menu.TurnOnMenu();
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
        

    }
}