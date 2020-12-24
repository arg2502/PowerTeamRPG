
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager
{
    public static UIDatabase uiDatabase;
    public static Dictionary<Menu, GameObject> dictionary_existingMenus = new Dictionary<Menu, GameObject>();
    public static List<GameObject> list_currentMenus = new List<GameObject>();
    public static GameObject menuInFocus; // the menu the player is currently on
    private static Canvas canvas;
    public static bool poppable;
    public static PauseCarousel pCarousel;

    public static void Init()
    {
        uiDatabase = Resources.Load<UIDatabase>("Databases/UIDatabase");

        // assign canvas and eventsystem, but make sure they exist
        SetCanvas();

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearLists;

    }

    private static void SetCanvas()
    {
        var canvasObj = GameObject.FindGameObjectWithTag("MainCanvas");
        if (canvasObj == null)
            Debug.LogError("Canvas not found. Did you add the 'MainCanvas' tag to the Canvas obj?");
        else
            canvas = canvasObj.GetComponent<Canvas>();
    }

    public static Menu CurrentMenu
    {
        get
        {
            var list = list_currentMenus;
            var count = list.Count;
            var latestMenu = list[count - 1].GetComponent<Menu>();
            return latestMenu;
        }

    }

    public static Menu FindMenu(GameObject databaseMenuToFind)
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

    public static GameObject ActivateMenu(GameObject menuPrefab, bool sub = false)
    {
        // freeze player if first menu
        if (GameControl.control.currentCharacterState != characterControl.CharacterState.Menu)
        {
            if (GameControl.control.currentCharacterState != characterControl.CharacterState.Battle) // I don't remember why we don't want MENU state while in battle, but for consistency, I'll make it the same when coming out of menus below
                GameControl.control.SetCharacterState(characterControl.CharacterState.Menu);
        }

        EnableMenu(menuPrefab, sub);
        InitMenu(menuPrefab.GetComponent<Menu>());

        return menuInFocus;
    }

    /// <summary>
    /// For turning menus on and off (visible/invisible). Ex: Pausing and Unpausing the game 
    /// </summary>
    /// <param name="menuPrefab"></param>
    public static void EnableMenu(GameObject menuPrefab, bool sub = false)
    {
        var menuToEnable = menuPrefab.GetComponent<Menu>();

        GameObject menuObj;
        // check if we have already have this menu in the scene. If we do, enable it
        if (dictionary_existingMenus.ContainsKey(menuToEnable)
            && dictionary_existingMenus[menuToEnable] != null)
        {
            menuObj = dictionary_existingMenus[menuToEnable];
            if (sub) AssignSubPosition(menuObj);
            list_currentMenus.Add(menuObj);
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


            dictionary_existingMenus.Add(menuToEnable, menuObj);
               
            list_currentMenus.Add(menuObj);
        }
        // set this menu as the one in focus (currently on)
        menuInFocus = menuObj;

    }

    private static void InitMenu(Menu menu)
    {
        var menuObj = dictionary_existingMenus[menu];
        menuObj.GetComponent<Menu>().Init();
    }

    /// <summary>
    /// Turning off all menus currently in the scene
    /// </summary>
    /// <param name="menuPrefab"></param>
    public static void DisableAllMenus()
    {
        HideAllMenus();

        // clear list
        list_currentMenus.Clear();

        // set focus to null
        menuInFocus = null;

        // set back to previous state
        if (GameControl.control.currentCharacterState != characterControl.CharacterState.Battle)
            GameControl.control.WaitAFrameAndSetCharacterState(GameControl.control.PrevState);
    }

    public static void PopAllMenus()
    {
        while (list_currentMenus.Count > 0)
            PopMenu();
    }

    /// <summary>
    ///  // for when we want to go to another menu, but simply deactivate/gray the other menu, not disable/turn invisible.
    ///  Ex: Pause Menu -> Sub Menus
    /// </summary>
    public static GameObject PushMenu(GameObject menuPrefab, Menu parentMenu = null)
    {
        if (parentMenu)
            parentMenu.RootButton = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
                
            return ActivateMenu(menuPrefab);
    }
    public static void PopMenu()
    {
        // Close the current menu and remove it from our menu stack
        var lastPos = list_currentMenus.Count - 1;

        // if there are no menus in our list or the menu is null somehow, ignore Pop
        if (lastPos < 0 || list_currentMenus[lastPos] == null)
            return;

        var menu = list_currentMenus[lastPos].GetComponent<Menu>();
        menu.Close();
        menu.RootButton = menu.AssignRootButton();
        list_currentMenus.RemoveAt(lastPos);

        // If there are more menus, set the new last menu to the current menu
        if (list_currentMenus.Count > 0)
        {
            lastPos = list_currentMenus.Count - 1;
            menuInFocus = list_currentMenus[lastPos];
            menu = menuInFocus.GetComponent<Menu>();
            menu.gameObject.SetActive(true);
            menu.SetSelectedObjectToRoot();
            menu.Refocus();
        }
        // otherwise, clear the stack
        else
            DisableAllMenus();
    }

    private static void AssignSubPosition(GameObject menuObj)
    {
        // if the menu is a sub menu, base it's position off of the parent's button that 
        var parentMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<Menu>();
        menuObj.transform.SetParent(parentMenu.transform);
        menuObj.transform.localPosition = Vector3.zero;

        var rootButtonPos = parentMenu.RootButton.transform.position;
        var buttonWidth = parentMenu.RootButton.GetComponent<RectTransform>().rect.width;
        menuObj.transform.position = new Vector3(rootButtonPos.x + buttonWidth, rootButtonPos.y);
    }

    public static void PushConfirmationMenu(string messageText, Action yesAction, Action noAction = null)
    {
        EnableMenu(uiDatabase.ConfirmationMenu);
        var confirmMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<ConfirmationMenu>();
        confirmMenu.specificText.text = messageText;
        confirmMenu.yesAction = yesAction;
        confirmMenu.noAction = noAction;
        confirmMenu.Refresh(); // refresh listeners
        confirmMenu.Init();
    }

    public static void PushNotificationMenu(string messageText)
    {
        EnableMenu(uiDatabase.NotificationMenu);
        var notificationMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<NotificationMenu>();
        notificationMenu.messageText.text = messageText;
        notificationMenu.Init();
    }

    public static void PushBeggarMenu(NPCBeggar beggar)
    {
        EnableMenu(uiDatabase.BeggarMenu);
        var beggarMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<BeggarMenu>();
        beggarMenu.beggar = beggar;
        beggarMenu.Init();
    }

    public static void PushThirstyManMenu(NPCThirstyMan thirstyMan)
    {
        EnableMenu(uiDatabase.ConsumablesOnlyMenu);
        var thirstyManMenu = list_currentMenus[list_currentMenus.Count - 1].GetComponent<ConsumablesOnlyMenu>();
        thirstyManMenu.thirstyMan = thirstyMan;
        thirstyManMenu.Init();
    }

    public static void HideAllMenus()
    {
        // turn off all menus
        foreach (var menu in list_currentMenus)
            menu.SetActive(false);
    }
    public static void ShowAllMenus()
    {
        foreach (var menu in list_currentMenus)
            menu.SetActive(true);
    }

    // clear the current menu list and dictionary of existing menu objects
    // should be called on scene changes when the menu objects are destroyed
    public static void ClearLists(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene last)
    {
        list_currentMenus.Clear();
        dictionary_existingMenus.Clear();
    }

    public static void ShowArbitersIcon()
    {
        GameObject.Instantiate(uiDatabase.ArbitersIcon, canvas.transform);
    }
    public static void ShowQuestStart(string questName)
    {
        var questStart = GameObject.Instantiate(uiDatabase.QuestStartEnd, canvas.transform);
        questStart.GetComponent<QuestStartEnd>().Init(true, questName);
    }

    public static void ShowQuestEnd(string questName)
    {
        var questStart = GameObject.Instantiate(uiDatabase.QuestStartEnd, canvas.transform);
        questStart.GetComponent<QuestStartEnd>().Init(false, questName);
    }

    public static InteractionNotification ShowInteractionNotification(Transform transformToFollow, string newMessage)
    {
        var interNot = GameObject.Instantiate(uiDatabase.InteractionNotification);
        interNot.GetComponent<InteractionNotification>().Init(transformToFollow, newMessage);
        return interNot.GetComponent<InteractionNotification>();
    }

    public static GameObject ShowQualityUI(Transform transformParent)
    {
        return GameObject.Instantiate(uiDatabase.QualityUI, transformParent);
    }

    public static void PopUntil(Menu topMenu)
    {
        while (CurrentMenu != topMenu)
            PopMenu();
    }
        
    public static void SetToTop(GameObject menuObj)
    {
        list_currentMenus.Remove(menuObj);
        list_currentMenus.Add(menuObj);
        menuInFocus = list_currentMenus[list_currentMenus.Count - 1];
    }

    public static void PushPauseCarousel()
    {
        if (pCarousel == null)
        {
            var pc = GameObject.Instantiate(uiDatabase.PauseCarousel);
            pCarousel = pc.GetComponent<PauseCarousel>();
        }
        var list = new List<GameObject>()
        {
            uiDatabase.InventoryMenu,
            uiDatabase.SkillTreeMenu,
            uiDatabase.OptionsMenu                
        };
        pCarousel.TurnOn(list);
    }

}
