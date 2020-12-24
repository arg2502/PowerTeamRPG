using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;

public class AttackSub : Menu
{
public Button strike, skills, spells;
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
    listOfButtons = new List<Button>() { strike, skills, spells };
}

protected override void AddListeners()
{
    base.AddListeners();

    strike.onClick.AddListener(OnStrike);
    skills.onClick.AddListener(OnSkills);
    spells.onClick.AddListener(OnSpells);
}
public override Button AssignRootButton()
{
    return strike;
}

public override void TurnOnMenu()
{
    base.TurnOnMenu();
    dimmer.gameObject.SetActive(false);
    rootButton = listOfButtons[0];
    SetSelectedObjectToRoot();
    CheckTechniques(); 
}

public override void Refocus()
{
    base.Refocus();
    UIManager.ShowAllMenus();
    dimmer.gameObject.SetActive(false);
}
void OnStrike()
{
    UIManager.HideAllMenus();
    battleManager.DetermineTargetType("Strike");
    UIManager.PushMenu(uiDatabase.TargetMenu);
}
void OnSkills()
{
    battleManager.SetMenuState(MenuState.SKILLS);
    UIManager.PushMenu(uiDatabase.ListSub);
    dimmer.gameObject.SetActive(true);
}
void OnSpells()
{
    battleManager.SetMenuState(MenuState.SPELLS);
    UIManager.PushMenu(uiDatabase.ListSub);
    dimmer.gameObject.SetActive(true);      
}
        
/// <summary>
/// Determines whether or not to disable the current denigens skills and/or spells buttons
/// </summary>
void CheckTechniques()
{
    var hero = battleManager.CurrentDenigen;

    // sets buttons to false if lists are empty, or if we can't afford any techniques
    // -- The Cast<> function is from the System.Linq library, it allows us to pass
    // a List of child objects in as a List of parent objects --
    skills.interactable = CheckInteractableOfTechs(hero.SkillsList.Cast<Technique>().ToList());
    spells.interactable = CheckInteractableOfTechs(hero.SpellsList.Cast<Technique>().ToList());

    // reset buttons
    SetButtonNavigation();
}

/// <summary>
/// Checks if we can enter into a Skills or Spells list. If no techniques available, false.
/// Otherwise, check if there's at least one technique that we can afford, if we can't, false.
/// If we can, true.
/// </summary>
/// <returns></returns>
bool CheckInteractableOfTechs(List<Technique> techniqueList)
{
    // if we don't have any techniques, there's no list to show, deactivate button
    if (techniqueList.Count <= 0)
        return false;

    // check to see if we have enough PM for at least 1 technique, if we do, allow them through
    var hero = battleManager.CurrentDenigen;
    for(int i = 0; i < techniqueList.Count; i++)
    {
        if (hero.Pm >= techniqueList[i].GetPmCost(hero))
            return true;
    }

    // if there are no techniques we can afford, deactivate the button
    return false;
}

new void Update()
{
    base.Update();

    if (EventSystem.current.currentSelectedGameObject != null &&
        rootButton == EventSystem.current.currentSelectedGameObject.GetComponent<Button>()
        || UIManager.menuInFocus != this.gameObject
        || !gameObject.activeSelf)
        return;

    rootButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
}
}
