using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu {

    public Button saveButton, loadButton;

    public override void TurnOnMenu()
    {
        base.TurnOnMenu();

        RootButton = AssignRootButton();
        SetSelectedObjectToRoot();
    }

    public override Button AssignRootButton()
    {
        return saveButton;
    }

    protected override void AddButtons()
    {
        base.AddButtons();

        listOfButtons = new List<Button>() { saveButton, loadButton };
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        saveButton.onClick.AddListener(OnSave);
        loadButton.onClick.AddListener(OnLoad);
    }

    void OnSave()
    {
        // save the game here
        UIManager.PushMenu(uiDatabase.SaveMenu, this);
    }

    void OnLoad()
    {
        // load the game here
        UIManager.PushMenu(uiDatabase.LoadMenu, this);
    }
}
