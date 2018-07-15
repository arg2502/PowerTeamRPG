namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class VictoryMenu : Menu
    {
        public VictoryCard jethroCard;
        public VictoryCard coleCard;
        public VictoryCard eleanorCard;
        public VictoryCard joulietteCard;
        BattleManager battleManager;

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>();
        }
        protected override void AddListeners()
        {
            base.AddListeners();
        }
        public override void Init()
        {
            base.Init();
            battleManager = FindObjectOfType<BattleManager>();

            FindHeroes();
        }

        void FindHeroes()
        {
            for(int i = 0; i < battleManager.heroList.Count; i++)
            {
                var currentDenigen = battleManager.heroList[i];
                if (currentDenigen.Data.identity == 0)
                    jethroCard.Init(currentDenigen);
                else if (currentDenigen.Data.identity == 1)
                    coleCard.Init(currentDenigen);
                else if (currentDenigen.Data.identity == 2)
                    eleanorCard.Init(currentDenigen);
                else if (currentDenigen.Data.identity == 3)
                    joulietteCard.Init(currentDenigen);
                
            }
        }

        public void LevelUp(int exp)
        {
            jethroCard.LevelUp(exp);
            coleCard.LevelUp(exp);
            eleanorCard.LevelUp(exp);
            joulietteCard.LevelUp(exp);
        }
    }
}