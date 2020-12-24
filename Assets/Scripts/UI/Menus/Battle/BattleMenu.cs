using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : Menu
{
    public Button attack, block, items, flee;
    BattleManager battleManager;
    public Image dimmer;

    public override void Init()
    {
        battleManager = FindObjectOfType<BattleManager>();
        descriptionText = battleManager.DescriptionText;
        base.Init();
    }

    protected override void AddButtons()
    {
        base.AddButtons();
        listOfButtons = new List<Button>() { attack, block, items, flee };
    }
    protected override void AddListeners()
    {
        base.AddListeners();

        attack.onClick.AddListener(OnAttack);
        block.onClick.AddListener(OnBlock);
        items.onClick.AddListener(OnItems);
        flee.onClick.AddListener(OnFlee);
    }

    public override Button AssignRootButton()
    {
        return attack;
    }

    public override void TurnOnMenu()
    {
        rootButton = AssignRootButton();
        base.TurnOnMenu();

        CheckForItems();
    }

    public override void Refocus()
    {
        base.Refocus();
        CheckForItems();
    }

    void OnAttack()
    {
        battleManager.SetMenuState(MenuState.STRIKE);
        UIManager.PushMenu(uiDatabase.AttackSub, this);
    }
    void OnBlock()
    {
        UIManager.HideAllMenus();
        battleManager.DetermineTargetType("Block");

    }
    void OnItems()
    {
        battleManager.SetMenuState(MenuState.ITEMS);
        UIManager.PushMenu(uiDatabase.ListSub, this);
    }
    void OnFlee()
    {
        // flee successful
        // end the battle
        if (battleManager.CalcFlee())
        {
            UIManager.HideAllMenus();
            battleManager.StartFlee();
        }
        // flee failed
        // skip to Attack state
        else
        {
            UIManager.HideAllMenus();
            battleManager.FleeFailed();
        }
    }    
        
    void CheckForItems()
    {
        if (DoWeHaveItems())
            items.interactable = true;
        else
            items.interactable = false;
        SetButtonNavigation();
    }    

    bool DoWeHaveItems()
    {
        // first check, if it's 0, we obviously don't have any items
        if (gameControl.consumables.Count <= 0)
			return false;
		else
			return true;
    }
}
