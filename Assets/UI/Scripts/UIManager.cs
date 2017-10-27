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
            if (canvas == null) LogError(canvas);
            
        }

        /// <summary>
        /// For turning menus on and off (visible/invisible). Ex: Pausing and Unpausing the game 
        /// </summary>
        /// <param name="menuPrefab"></param>

        public void EnableMenu(GameObject menuPrefab)
        {
            var menuToEnable = menuPrefab.GetComponent<Menu>();

            GameObject menuObj;
            // check if we have already have this menu in the scene. If we do, enable it
            if(dictionary_existingMenus.ContainsKey(menuToEnable))
            {
                menuObj = dictionary_existingMenus[menuToEnable];
                menuObj.SetActive(true);
                list_currentMenus.Add(menuObj);
            }
            // if we don't already have one, create it
            else
            {
                menuObj = GameObject.Instantiate(menuPrefab);
                menuObj.transform.SetParent(canvas.transform, false);
                menuObj.transform.localPosition = Vector3.zero;
                dictionary_existingMenus.Add(menuToEnable, menuObj);
                list_currentMenus.Add(menuObj);
            }
            menuObj.GetComponent<Menu>().Init();
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
            if(parentMenu)
            {
                parentMenu.FirstButton = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
                parentMenu.ToggleButtonState(false);
            }
            EnableMenu(menuPrefab);
        }
        public void PopMenu()
        {
            if (list_currentMenus.Count > 1)
            {
                var lastPos = list_currentMenus.Count - 1;
                list_currentMenus[lastPos].SetActive(false);
                list_currentMenus.RemoveAt(lastPos);
                lastPos = list_currentMenus.Count - 1;
                menuInFocus = list_currentMenus[lastPos];
                var menu = menuInFocus.GetComponent<Menu>();
                EventSystem.current.SetSelectedGameObject(menu.FirstButton.gameObject);
                menu.ToggleButtonState(true);
            }
            else
                DisableAllMenus();
        }
        
        public void LogError(Object nullObject)
        {
            Debug.LogError(nullObject.name + " is null. Be sure you assigned the variable properly. Or make sure there is an object of type: " + nullObject.GetType() + ", in the scene.");
        }        
        

    }
}