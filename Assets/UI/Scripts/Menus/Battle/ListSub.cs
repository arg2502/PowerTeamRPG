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
        public GameObject slotsContainer;

        float slotDistance = 55f;

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            base.Init();
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>();
            FillList(battleManager.CurrentHero.SpellsList);
        }
        

        void FillList(List<Spell> category) // SPELL IS TEMPORARY -- SHOULD BE ABLE TO HANDLE ANY TECHNIQUE
        {
            for (int i = 0; i < category.Count; i++)
            {
                var item = Instantiate(slotPrefab);
                item.name = category[i].Name + "_Button";
                item.transform.SetParent(slotsContainer.transform);
                item.GetComponent<RectTransform>().localPosition = new Vector2(0, i * -slotDistance);
                item.GetComponent<RectTransform>().localScale = Vector3.one; // reset scale to match with parent

                var button = item.GetComponentInChildren<Button>();                
                button.GetComponentInChildren<Text>().text = category[i].Name;
                listOfButtons.Add(button);
            }
        }
    }
}