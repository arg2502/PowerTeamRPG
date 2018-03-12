namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class ListSub : Menu
    {
        BattleManager battleManager;

        public GameObject slotPrefab;
        public GameObject jethroSkillsContainers, jethroSpellsContainers,
            coleSkillsContainers, coleSpellsContainers,
            eleanorSkillsContainers, eleanorSpellsContainers,
            joulietteSkillsContainers, joulietteSpellsContainers;

        float slotDistance = 55f;

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            base.Init();
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();
            listOfButtons = new List<Button>();
            FillList(battleManager.CurrentHero);
            rootButton = listOfButtons[0];
            SetSelectedObjectToRoot();
        }
        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>();
        }

        void FillList(Hero currentHero) // SPELL IS TEMPORARY -- SHOULD BE ABLE TO HANDLE ANY TECHNIQUE
        {
            // set/find container
            GameObject container;
            if (currentHero is Jethro)
                container = jethroSpellsContainers;
            else if (currentHero is Cole)
                container = coleSpellsContainers;
            else if (currentHero is Eleanor)
                container = eleanorSpellsContainers;
            else
                container = joulietteSpellsContainers;

            container.SetActive(true);

            // if we already created this hero's specific technique buttons, just add them to the buttons list
            if (container.GetComponentsInChildren<Button>().Length > 0)
            {
                foreach (var b in container.GetComponentsInChildren<Button>())
                    listOfButtons.Add(b);
            }
            else
            {

                var category = currentHero.SpellsList;

                for (int i = 0; i < category.Count; i++)
                {
                    var item = Instantiate(slotPrefab);
                    item.name = category[i].Name + "_Button";
                    item.transform.SetParent(container.transform);
                    item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * -slotDistance);
                    item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                    var button = item.GetComponentInChildren<Button>();
                    button.GetComponentInChildren<Text>().text = category[i].Name;
                    listOfButtons.Add(button);

                    // assign attacks
                    //// remove spaces from attack name
                    //var attack = category[i].Name.Replace(" ", string.Empty);
                    var attack = category[i].Name;
                    button.onClick.AddListener(() => OnSelect(attack));
                }
            }
        }

        void OnSelect(string attack)
        {
            uiManager.HideAllMenus();
            battleManager.DetermineTargetType(attack);
            uiManager.PushMenu(uiDatabase.TargetMenu);
        }
    }
}