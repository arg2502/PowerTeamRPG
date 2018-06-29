namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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
        }
        protected override void AddListeners()
        {
            base.AddListeners();
        }
        public override void Init()
        {
            base.Init();
            battleManager = FindObjectOfType<BattleManager>();

            jethroCard.Init(battleManager.heroList[0]);
            coleCard.Init(battleManager.heroList[1]);
            eleanorCard.Init(battleManager.heroList[2]);
            joulietteCard.Init(battleManager.heroList[3]);
        }
    }
}