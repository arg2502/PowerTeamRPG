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
        internal Item item;
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
            juliette.onClick.AddListener(OnJuliette);
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

        void StatChangeDescription(HeroData currentHero)
        {
            descriptionText.text = "<b>" + currentHero.name + "</b>";

            // status
            descriptionText.text += "\n\nStatus: " + currentHero.statusState;

            if (!string.IsNullOrEmpty(item.statusChange))
                descriptionText.text += " " + item.statusChange;
            
            descriptionText.text += "\nHP: " + currentHero.hp + " / " + currentHero.hpMax;
            CheckIfChange(currentHero.hp, currentHero.hpMax, item.hpChange, item.hpMaxChange);
            
            descriptionText.text += "\nPM: " + currentHero.pm + " / " + currentHero.pmMax;
            CheckIfChange(currentHero.pm, currentHero.pmMax, item.pmChange, item.pmMaxChange);

            descriptionText.text += "\nAtk: " + currentHero.atk;
            CheckIfChange(currentHero.atk, item.atkChange);

            descriptionText.text += "\nDef: " + currentHero.def;
            CheckIfChange(currentHero.def, item.defChange);

            descriptionText.text += "\nMgk Atk: " + currentHero.mgkAtk;
            CheckIfChange(currentHero.mgkAtk, item.mgkAtkChange);

            descriptionText.text += "\nMgk Def: " + currentHero.mgkDef;
            CheckIfChange(currentHero.mgkDef, item.mgkDefChange);

            descriptionText.text += "\nLuck: " + currentHero.luck;
            CheckIfChange(currentHero.luck, item.luckChange);

            descriptionText.text += "\nEvasion: " + currentHero.evasion;
            CheckIfChange(currentHero.evasion, item.evadeChange);

            descriptionText.text += "\nSpeed: " + currentHero.spd;
            CheckIfChange(currentHero.spd, item.spdChange);

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

        public void CheckIfHeroesAreElligible()
        {
            foreach (var hero in gameControl.heroList)
            {
                if (menuState == MenuState.Equip)
                {
                    if (item.GetComponent<WeaponItem>()
                        && hero.weapon != null
                       && item == hero.weapon.GetComponent<WeaponItem>())
                        ToggleHero(hero, false);

                    else if (item.GetComponent<ArmorItem>()
                        && hero.EquipmentContainsItem(item))
                        ToggleHero(hero, false);

                    else
                        ToggleHero(hero, true);
                }
                else if (menuState == MenuState.Remove)
                {
                    if (item.GetComponent<WeaponItem>()
                        && hero.weapon != null
                       && item == hero.weapon.GetComponent<WeaponItem>())
                        ToggleHero(hero, true);

                    else if (item.GetComponent<ArmorItem>()
                        && hero.EquipmentContainsItem(item))
                        ToggleHero(hero, true);

                    else
                        ToggleHero(hero, false);
                }
                
            }
        }
        void ToggleHero(HeroData hero, bool state)
        {
            if (hero == gameControl.heroList[0])
                ToggleButton(jethro, state);
            else if (hero == gameControl.heroList[1])
                ToggleButton(cole, state);
            else if (hero == gameControl.heroList[2])
                ToggleButton(eleanor, state);
            else if (hero == gameControl.heroList[3])
                ToggleButton(juliette, state);
            else
                Debug.LogError("Hero: " + hero.name + ", does not exist");
        }
        void ToggleButton(Button button, bool state)
        {
            button.interactable = state;
        }

        // button functions
        void OnJethro() { UseItem(gameControl.heroList[0]); }
        void OnCole() { UseItem(gameControl.heroList[1]); }
        void OnEleanor() { UseItem(gameControl.heroList[2]); }
        void OnJuliette() { UseItem(gameControl.heroList[3]); }

        void UseItem(HeroData hero)
        {
            Debug.Log("Before use -- quantity: " + item.quantity + ", uses: " + item.uses);
            switch (menuState)
            {
                case MenuState.Use:
                    if (item.quantity > 0)
                        item.GetComponent<ConsumableItem>().Use(hero);
                    break;

                case MenuState.Equip:
                    if (item.GetComponent<WeaponItem>())
                        item.GetComponent<WeaponItem>().Use(hero);
                    else if (item.GetComponent<ArmorItem>())
                        item.GetComponent<ArmorItem>().Use(hero);
                    break;

                case MenuState.Remove:
                    if (item.GetComponent<WeaponItem>())
                        item.GetComponent<WeaponItem>().Remove(hero);
                    else if (item.GetComponent<ArmorItem>())
                        item.GetComponent<ArmorItem>().Remove(hero);
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
            AssignTitleText();
            CheckIfHeroesAreElligible();
            SetButtonNavigation();
            RootButton = AssignRootButton();
            AssignEventToRoot();
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