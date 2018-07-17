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

        public Button doneButton;
        public Text totalGold;
        public Text earnedGold;
        float goldRate = 1f;

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { doneButton };
        }
        protected override void AddListeners()
        {
            base.AddListeners();
            doneButton.onClick.AddListener(OnDone);
        }
        public override void Init()
        {
            base.Init();
            battleManager = FindObjectOfType<BattleManager>();
            
            FindHeroes();
        }

        public override Button AssignRootButton()
        {
            return doneButton;
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

            StartCoroutine(WaitForCardsToFinish());
        }

        IEnumerator WaitForCardsToFinish()
        {
            while (!jethroCard.IsDone || !coleCard.IsDone || !eleanorCard.IsDone || !joulietteCard.IsDone)
                yield return null;

            StartCoroutine(IncreaseGold());
        }

        public void AddGold(int gold)
        {
            // set initial text
            totalGold.text = gameControl.totalGold.ToString();
            earnedGold.text = gold.ToString();

            // actually add winnings to gamecontrol
            gameControl.AddGold(gold);
        }

        IEnumerator IncreaseGold()
        {
            var totalInt = int.Parse(totalGold.text);
            var earnedInt = int.Parse(earnedGold.text);

            while(earnedInt > 0)
            {
                totalInt++;
                earnedInt--;

                totalGold.text = totalInt.ToString();

                if (earnedInt > 0)
                    earnedGold.text = earnedInt.ToString();
                else
                    earnedGold.gameObject.SetActive(false);

                yield return new WaitForSeconds(Time.deltaTime * goldRate);                
            }
            
            ActivateDoneButton();
        }

        void ActivateDoneButton()
        {
            doneButton.interactable = true;
        }

        void OnDone()
        {
            gameControl.ReturnFromBattle();
        }
    }
}