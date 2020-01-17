namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;
    using System.Collections.Generic;

    public class UseItemMenu : Menu
    {
        public Button jethro, cole, eleanor, juliette;
        internal InventoryItem item;
        public Image icon;
        public Text itemName;
        public Text titleText;
        GameObject currentObj;

        public enum MenuState { Use, Equip, Remove };
        public MenuState menuState;

        public override void Init()
        {
            base.Init();
        }
        public void AssignTitleText()
        {
            switch (menuState)
            {
                case MenuState.Use:
                    titleText.text = "Use on...";
                    break;
                case MenuState.Equip:
                    titleText.text = "Equip to...";
                    break;
                case MenuState.Remove:
                    titleText.text = "Remove from...";
                    break;
            }
        }
        protected override void AddButtons()
        {
            base.AddButtons();

            listOfButtons = new List<Button>() { jethro, cole, eleanor, juliette };
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
                return juliette;
        }
        protected override void AddListeners()
        {
            base.AddListeners();

            jethro.onClick.AddListener(OnJethro);
            cole.onClick.AddListener(OnCole);
            eleanor.onClick.AddListener(OnEleanor);
            juliette.onClick.AddListener(OnJouliette);
        }
        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

        }

        void SetDescription()
        {
            if (currentObj == jethro.gameObject)
                StatChangeDescription(gameControl.heroList[0]);
            else if (currentObj == cole.gameObject)
                StatChangeDescription(gameControl.heroList[1]);
            else if (currentObj == eleanor.gameObject)
                StatChangeDescription(gameControl.heroList[2]);
            else if (currentObj == juliette.gameObject)
                StatChangeDescription(gameControl.heroList[3]);
        }

        void StatChangeDescription(DenigenData currentHero)
        {
            descriptionText.text = "<b>" + currentHero.denigenName + "</b>";
			// status
			descriptionText.text += "\n\nStatus: " + currentHero.statusState;

			//get the info on the item we are using
			if (item.type == "consumable") {
				ScriptableConsumable _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableConsumable;
                //Consumables are the only items that offer status changes
                descriptionText.text += " " + _item.statusChange;
				
				//call the rest of the description text
				GenerateStatDescription (currentHero, _item);
			} else if (item.type == "augment") {
				ScriptableAugment _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableAugment;
				//generate most of the description text
				GenerateStatDescription (currentHero, _item);
				//generate weapon specific text -- ADD LATER
			} else if (item.type == "armor") {
				ScriptableArmor _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableArmor;
				//generate most of the description text
				GenerateStatDescription (currentHero, _item);
				//generate Armor specific text -- ADD LATER
			} else if (item.type == "key") {
				//This is probably going to be very different from the other 3 types of item
				ScriptableKey _item = ItemDatabase.GetItem (item.type, item.name) as ScriptableKey;
				//generate key specifit text -- ADD LATER
			} else {
				print("No item named " + item.name + " of type " + item.type + " exists.");
			}

        }

		void GenerateStatDescription(DenigenData currentHero, ScriptableItem _item){

			descriptionText.text += "\nHP: " + currentHero.hp + " / " + currentHero.hpMax;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "HP"){
					CheckIfChange(currentHero.hp, currentHero.hpMax, b.boost);
				}
			}
			
			descriptionText.text += "\nPM: " + currentHero.pm + " / " + currentHero.pmMax;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "PM"){
					CheckIfChange(currentHero.pm, currentHero.pmMax, b.boost);
				}
			}
			
			descriptionText.text += "\nAtk: " + currentHero.atk;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "ATK"){
					CheckIfChange(currentHero.atk, b.boost);
				}
			}
			
			descriptionText.text += "\nDef: " + currentHero.def;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "DEF"){
					CheckIfChange(currentHero.def, b.boost);
				}
			}
			
			descriptionText.text += "\nMgk Atk: " + currentHero.mgkAtk;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "MGKATK"){
					CheckIfChange(currentHero.mgkAtk, b.boost);
				}
			}
			
			descriptionText.text += "\nMgk Def: " + currentHero.mgkDef;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "MGKDEF"){
					CheckIfChange(currentHero.mgkDef, b.boost);
				}
			}
			
			descriptionText.text += "\nLuck: " + currentHero.luck;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "LUCK"){
					CheckIfChange(currentHero.luck, b.boost);
				}
			}
			
			descriptionText.text += "\nEvasion: " + currentHero.evasion;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "EVASION"){
					CheckIfChange(currentHero.evasion, b.boost);
				}
			}
			
			descriptionText.text += "\nSpeed: " + currentHero.spd;
			foreach (Boosts b in _item.statBoosts) {
				if(b.statName == "SPD"){
					CheckIfChange(currentHero.spd, b.boost);
				}
			}

		}

        void CheckIfChange(int herostat, int change)
        {
            if (menuState == MenuState.Remove)
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
            if(menuState == MenuState.Use || menuState == MenuState.Remove)
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
                        uiManager.PushConfirmationMenu(message, () => ReplaceItem(hero));
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
        }

        void UseItem(DenigenData hero)
        {
            Debug.Log("Before use -- quantity: " + item.quantity + ", uses: " + item.uses);
            switch (menuState)
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
            uiManager.PopMenu();
            uiManager.PopMenu();
            currentObj = null; // for resetting the description text when we return to this menu
        }

        /// <summary>
        /// Sets up necessary variables after all the other init functions (Init(), TurnOnMenu(), Refocus())
        /// Needs to be independently called
        /// </summary>
        public void Setup()
        {
            jethro.interactable = true;
            cole.interactable = true;
            eleanor.interactable = true;
            juliette.interactable = true;

            if (menuState == MenuState.Remove)
            {
                if (!gameControl.heroList[0].augments.Contains(item)) jethro.interactable = false;
                if (gameControl.heroList.Count > 1 && !gameControl.heroList[1].augments.Contains(item)) cole.interactable = false;
                if (gameControl.heroList.Count > 2 && !gameControl.heroList[2].augments.Contains(item)) eleanor.interactable = false;
                if (gameControl.heroList.Count > 3 && !gameControl.heroList[3].augments.Contains(item)) juliette.interactable = false;
            }

            AssignTitleText();
            SetButtonNavigation();
            RootButton = AssignRootButton();
            SetSelectedObjectToRoot();
        }

        new void Update()
        {
            base.Update();

            if (currentObj == EventSystem.current.currentSelectedGameObject) return;

            currentObj = EventSystem.current.currentSelectedGameObject;
                        
            SetDescription();            

        }
    }
}