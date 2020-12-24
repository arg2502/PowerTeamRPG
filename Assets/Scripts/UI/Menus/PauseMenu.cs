using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : Menu
{
    public Button exitMenuButton, teamInfoButton, inventoryButton, saveButton, loadButton;
    public Text totalGold;

    public override void TurnOnMenu()
    {
        base.TurnOnMenu();

        RootButton = AssignRootButton();
        SetSelectedObjectToRoot();
        totalGold.text = gameControl.totalGold.ToString();
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        exitMenuButton.onClick.AddListener(OnExit);
        teamInfoButton.onClick.AddListener(OnTeamInfo);
        inventoryButton.onClick.AddListener(OnInventory);
        saveButton.onClick.AddListener(OnSave);
        loadButton.onClick.AddListener(OnLoad);
    }
    protected override void AddButtons()
    {
        listOfButtons = new List<Button>() { exitMenuButton, teamInfoButton, inventoryButton, saveButton, loadButton };
    }

    public override Button AssignRootButton() { return exitMenuButton; }

    void OnExit()
    {
        UIManager.PopMenu();
    }

    void OnTeamInfo()
    {            
        UIManager.PushMenu(uiDatabase.TeamInfoSub, this);
    }

    void OnInventory()
    {
        UIManager.PushMenu(uiDatabase.InventorySub, this);
    }

    void OnSave()
    {
        // save the game here
        UIManager.PushMenu(uiDatabase.SaveMenu, this);
    }

    void OnLoad()
    {
        // load the game here
        //GameControl.control.Load();
        UIManager.PushMenu(uiDatabase.LoadMenu, this);
    }
}
