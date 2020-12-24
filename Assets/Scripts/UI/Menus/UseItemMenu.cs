using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UseItemMenu : Menu
{
    public Button jethro, cole, eleanor, jouliette;
    internal InventoryItem item;
    public Image icon;
    public Text itemName;
    public Text titleText;
    GameObject currentObj;
    public Image jethroE, coleE, eleanorE, joulietteE;
    public InventoryMenu inventory;

    public enum MenuState { Use, Equip, Remove };
    //public MenuState menuState;

    public override void Init()
    {
        base.Init();
    }
    public void AssignTitleText()
    {
        titleText.text = "";
        //switch (menuState)
        //{
        //    case MenuState.Use:
        //        titleText.text = "Use on...";
        //        break;
        //    case MenuState.Equip:
        //        titleText.text = "Equip to...";
        //        break;
        //    case MenuState.Remove:
        //        titleText.text = "Remove from...";
        //        break;
        //}
    }

    DenigenData CurrentHero
    {
        get
        {
            if (currentObj == jethro.gameObject)
                return gameControl.heroList[0];
            else if (currentObj == cole.gameObject)
                return gameControl.heroList[1];
            else if (currentObj == eleanor.gameObject)
                return gameControl.heroList[2];
            else
                return gameControl.heroList[3];
        }
    }

    MenuState CurrentState
    {
        get
        {
            if (item.type == "consumable")
                return MenuState.Use;
            else
            {
                if (item.type == "augment")
                {
                    if (CurrentHero.augments.Contains(item))
                        return MenuState.Remove;
                    else
                        return MenuState.Equip;

                }
                else
                {
                    if (CurrentHero.armor.Contains(item))
                        return MenuState.Remove;
                    else
                        return MenuState.Equip;
                }
            }
        }
    }

    protected override void AddButtons()
    {
        base.AddButtons();

        listOfButtons = new List<Button>() { jethro, cole, eleanor, jouliette };
    }
    public override Button AssignRootButton()
    {
        if (jethro.interactable)
            return jethro;
        else if (cole.interactable)
            return cole;
        else if (eleanor.interactable)
            return eleanor;
        else
            return jouliette;
    }
    protected override void AddListeners()
    {
        base.AddListeners();

        jethro.onClick.AddListener(OnJethro);
        cole.onClick.AddListener(OnCole);
        eleanor.onClick.AddListener(OnEleanor);
        jouliette.onClick.AddListener(OnJouliette);
    }
    public override void TurnOnMenu()
    {
        base.TurnOnMenu();

    }

    void SetDescription()
    {
        StatChangeDescription(CurrentHero);
            
    }

    void StatChangeDescription(DenigenData currentHero)
    {
        descriptionText.text = "<b>" + currentHero.denigenName + "</b>";
		// status
		descriptionText.text += "\n\nStatus: " + currentHero.statusState;

		//get the info on the item we are using
		if (item.type == "consumable")
        {
			ScriptableConsumable _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableConsumable;
            //Consumables are the only items that offer status changes
            descriptionText.text += " " + _item.statusChange;
				
			//call the rest of the description text
			GenerateStatDescription (currentHero, _item);
		}
        else if (item.type == "augment")
        {
			ScriptableAugment _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableAugment;
            //generate most of the description text
            var itemToReplace = (currentHero.augments.Count > 0 && CurrentState == MenuState.Equip) ? ItemDatabase.GetItem(currentHero.augments[0].type, currentHero.augments[0].name) : null; // FOR NOW ONLY ONE AUGMENT ALLOWED -- WILL HAVE TO BE CHANGED IF MORE THAN ONE AUGMENT IS ALLOWED
            GenerateStatDescription(currentHero, _item, itemToReplace);
		}
        else if (item.type == "armor")
        {
			ScriptableArmor _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableArmor;
            //generate most of the description text
            ScriptableItem itemToReplace = null;
            if (CurrentState == MenuState.Equip)
            {
                var invItem = currentHero.armor.Find((i) => i.subtype == _item.GetSubType());
                if (invItem != null)
                    itemToReplace = ItemDatabase.GetItem(invItem);
            }
			GenerateStatDescription (currentHero, _item, itemToReplace);
			//generate Armor specific text -- ADD LATER
		}
        else if (item.type == "key")
        {
			//This is probably going to be very different from the other 3 types of item
			ScriptableKey _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableKey;
			//generate key specifit text -- ADD LATER
		}
        else
        {
			print("No item named " + item.name + " of type " + item.type + " exists.");
		}

    }

	void GenerateStatDescription(DenigenData currentHero, ScriptableItem _item, ScriptableItem itemToReplace = null){

        int boost = 0;
        descriptionText.text += "\nHP: " + currentHero.hp + " / " + currentHero.hpMax;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "HP");
		foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "HP"){
                boost += b.boost;					
			}
		}
        CheckIfChange(currentHero.hp, currentHero.hpMax, boost);

        boost = 0;
		descriptionText.text += "\nPM: " + currentHero.pm + " / " + currentHero.pmMax;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "PM");
		foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "PM"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.pm, currentHero.pmMax, boost);

        boost = 0;
		descriptionText.text += "\nAtk: " + currentHero.atk;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "ATK");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "ATK"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.atk, boost);

        boost = 0;
		descriptionText.text += "\nDef: " + currentHero.def;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "DEF");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "DEF"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.def, boost);

        boost = 0;
		descriptionText.text += "\nMgk Atk: " + currentHero.mgkAtk;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "MGKATK");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "MGKATK"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.mgkAtk, boost);

        boost = 0;
		descriptionText.text += "\nMgk Def: " + currentHero.mgkDef;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "MGKDEF");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "MGKDEF"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.mgkDef, boost);

        boost = 0;
		descriptionText.text += "\nLuck: " + currentHero.luck;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "LUCK");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "LUCK"){
                boost += b.boost;
			}
		}
        CheckIfChange(currentHero.luck, boost);

        boost = 0;
		descriptionText.text += "\nEvasion: " + currentHero.evasion;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "EVASION");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "EVASION"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.evasion, boost);

        boost = 0;
		descriptionText.text += "\nSpeed: " + currentHero.spd;
        if (itemToReplace != null) boost -= GetReplaceLoss(itemToReplace.statBoosts, "SPD");
        foreach (Boosts b in _item.statBoosts) {
			if(b.statName == "SPD"){
                boost += b.boost;
			}
        }
        CheckIfChange(currentHero.spd, boost);

    }

    int GetReplaceLoss(Boosts[] boosts, string statName)
    {
        foreach (Boosts b in boosts)
        {
            if (b.statName == statName)
                return b.boost;                
        }
        return 0;
    }

    void CheckIfChange(int herostat, int change)
    {
        if (CurrentState == MenuState.Remove)
            change *= -1;

        if (change != 0)
        {
            if (change > 0)
                descriptionText.text += " <color=green>" + (herostat + change) + "</color>";
            else
            {
                if (herostat + change < 0)
                    descriptionText.text += " <color=red>" + 0 + "</color>";
                else
                    descriptionText.text += " <color=red>" + (herostat + change) + "</color>";
            }
                
                
        }
    }
	//method used for healing items to ensure that a value higher than maxhp or maxPm in not displayed
	void CheckIfChange(int herostatChange, int herostatMax, int change)
	{
		if (change != 0 )
		{
			if (change > 0)
			{
				if (herostatChange + change > herostatMax)
					descriptionText.text += " <color=green>" + (herostatMax) + " / " + (herostatMax) + "</color>";
				else
					descriptionText.text += " <color=green>" + (herostatChange + change) + " / " + (herostatMax) + "</color>";
			}
			else
			{
				if (herostatChange + change < 0)
					descriptionText.text += " <color=red>" + 0;
				else
					descriptionText.text += " <color=red>" + herostatChange + change;
			}
		}
	}
    void CheckIfChange(int herostatChange, int herostatMax, int change, int max)
    {
        if (change != 0 || max != 0)
        {
            if (change > 0 || max > 0)
            {
                if (herostatChange + change > herostatMax + max)
                    descriptionText.text += " <color=green>" + (herostatMax + max) + " / " + (herostatMax + max) + "</color>";
                else
                    descriptionText.text += " <color=green>" + (herostatChange + change) + " / " + (herostatMax + max) + "</color>";
            }
            else
            {
                if (herostatChange + change < 0)
                    descriptionText.text += " <color=red>" + 0;
                else
                    descriptionText.text += " <color=red>" + herostatChange + change;

                if (herostatMax + max < 0)
                    descriptionText.text += " / " + 0 + "</color>";
                else
                    descriptionText.text += " / " + herostatMax + max + "</color>";
            }
        }
    }
               
    // button functions
    void OnJethro() { CheckIfCanUseItem(gameControl.heroList[0]); }
    void OnCole() { CheckIfCanUseItem(gameControl.heroList[1]); }
    void OnEleanor() { CheckIfCanUseItem(gameControl.heroList[2]); }
    void OnJouliette() { CheckIfCanUseItem(gameControl.heroList[3]); }

    void CheckIfCanUseItem(DenigenData hero)
    {
        if(CurrentState == MenuState.Use || CurrentState == MenuState.Remove)
        {
            UseItem(hero);                
        }
        else
        {
            if(item.type == "augment")
            {
                if (hero.augments.Count >= hero.maxAugmentsAmt)
                {
                    string message = "Remove " + hero.augments[0].name + " from " + hero.denigenName + " and equip " + item.name + "?";
                    UIManager.PushConfirmationMenu(message, () => ReplaceItem(hero));
                }
                else
                    UseItem(hero);
            }
            else if (item.type == "armor")
            {
                var itemToReplace = hero.armor.Find((i) => i.subtype == item.subtype);
                if (itemToReplace != null)
                {
                    string message = "Remove " + itemToReplace.name + " from " + hero.denigenName + " and equip " + item.name + "?";
                    UIManager.PushConfirmationMenu(message, () => ReplaceItem(hero));
                }
                else
                    UseItem(hero);
            }
        }


    }

    void ReplaceItem(DenigenData hero)
    {
        if(item.type == "augment")
        {
            var itemToRemove = hero.augments[hero.maxAugmentsAmt - 1];
            GameControl.itemManager.RemoveItem(hero, itemToRemove);
            UseItem(hero);
        }
        else if(item.type == "armor")
        {
            var itemToRemove = hero.armor.Find((i) => i.subtype == item.subtype);
            GameControl.itemManager.RemoveItem(hero, itemToRemove);
            UseItem(hero);
        }
    }

    void UseItem(DenigenData hero)
    {
        Debug.Log("Before use -- quantity: " + item.quantity + ", uses: " + item.uses);
        switch (CurrentState)
        {
            case MenuState.Use:
                GameControl.itemManager.ItemUse(hero, item);
                break;

            case MenuState.Equip:
                GameControl.itemManager.EquipItem(hero, item);
                break;

            case MenuState.Remove:
                GameControl.itemManager.RemoveItem(hero, item);
                break;
        }
        Debug.Log("After use -- quantity: " + item.quantity + ", uses: " + item.uses);
        CheckIfOutOfUses();
        //UIManager.PopMenu();
        //UIManager.PopMenu();
        inventory.UpdateItemQuantity();
        SetDescription();
        SetEquippedIcon();
        //currentObj = null; // for resetting the description text when we return to this menu
    }

    /// <summary>
    /// Sets up necessary variables after all the other init functions (Init(), TurnOnMenu(), Refocus())
    /// Needs to be independently called
    /// </summary>
    public void Setup()
    {
        SetEquippedIcon();
        //jethro.interactable = true;
        //cole.interactable = true;
        //eleanor.interactable = true;
        //jouliette.interactable = true;

        //if (menuState == MenuState.Remove)
        //{
        //    if (!gameControl.heroList[0].augments.Contains(item) || !gameControl.heroList[0].armor.Contains(item)) jethro.interactable = false;
        //    if (gameControl.heroList.Count > 1 && !gameControl.heroList[1].augments.Contains(item)) cole.interactable = false;
        //    if (gameControl.heroList.Count > 2 && !gameControl.heroList[2].augments.Contains(item)) eleanor.interactable = false;
        //    if (gameControl.heroList.Count > 3 && !gameControl.heroList[3].augments.Contains(item)) jouliette.interactable = false;
        //}

        AssignTitleText();
        SetButtonNavigation();
        RootButton = AssignRootButton();
        SetSelectedObjectToRoot();
    }

    void SetEquippedIcon()
    {
        jethroE.gameObject.SetActive(IsEquipped(gameControl.Jethro));
        coleE.gameObject.SetActive(IsEquipped(gameControl.Cole));
        eleanorE.gameObject.SetActive(IsEquipped(gameControl.Eleanor));
        joulietteE.gameObject.SetActive(IsEquipped(gameControl.Jouliette));
    }

    void SetInteractable(bool itemAvailable)
    {
        jethro.interactable = itemAvailable ? itemAvailable : IsEquipped(gameControl.Jethro);
        cole.interactable = itemAvailable ? itemAvailable : IsEquipped(gameControl.Cole);
        eleanor.interactable = itemAvailable ? itemAvailable : IsEquipped(gameControl.Eleanor);
        jouliette.interactable = itemAvailable ? itemAvailable : IsEquipped(gameControl.Jouliette);
    }

    bool IsEquipped(DenigenData hero)
    {
        if (hero == null) return false;

        if (item.type == "consumable")
            return false;
        else if (item.type == "augment")
            return (hero.augments.Contains(item));
        else if (item.type == "armor")
            return (hero.armor.Contains(item));
        else
            return false;
    }

    void CheckIfOutOfUses()
    {
        if (item.quantity <= 0) return;
        bool itemAvailable = (item.uses < item.quantity);
        SetInteractable(itemAvailable);
        SetButtonNavigation();
            
    }

    new void Update()
    {
        base.Update();
                        
        if (currentObj == EventSystem.current.currentSelectedGameObject) return;

        currentObj = EventSystem.current.currentSelectedGameObject;
        if (this.gameObject != UIManager.menuInFocus) return;
        SetDescription();            

    }
}