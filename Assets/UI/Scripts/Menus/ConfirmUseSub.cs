namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;
    using System.Collections.Generic;

    public class ConfirmUseSub : Menu
    {
        public Button jethro, cole, eleanor, juliette;
        internal Item item;
        public Image icon;
        GameObject currentObj;

        public override void Init()
        {
            base.Init();
            
        }
        protected override void AddButtons()
        {
            base.AddButtons();

            listOfButtons = new List<Button>() { jethro, cole, eleanor, juliette };
        }
        public override Button AssignRootButton()
        {
            return jethro;
        }
        protected override void AddListeners()
        {
            base.AddListeners();

            //TEMP
            foreach (var button in listOfButtons)
                button.onClick.AddListener(() => Debug.Log(item.name));
        }

        void SetDescription()
        {
            if (currentObj == jethro.gameObject)
                //descriptionText.text = "Jethro";
                StatChangeDescription(gameControl.heroList[0]);
            else if (currentObj == cole.gameObject)
                //descriptionText.text = "Cole";
                StatChangeDescription(gameControl.heroList[1]);
            else if (currentObj == eleanor.gameObject)
                //descriptionText.text = "Eleanor";
                StatChangeDescription(gameControl.heroList[2]);
            else
                //descriptionText.text = "Juliette";
                StatChangeDescription(gameControl.heroList[3]);
        }

        void StatChangeDescription(HeroData currentHero)
        {
            descriptionText.text = currentHero.name;

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

        new void Update()
        {
            base.Update();

            if (currentObj == EventSystem.current.currentSelectedGameObject) return;

            currentObj = EventSystem.current.currentSelectedGameObject;
                        
            SetDescription();            

        }
    }
}