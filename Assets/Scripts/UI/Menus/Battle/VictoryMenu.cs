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

        public Button skillTreeButton;
        public Button statsButton;
        public Button doneButton;
        public Text totalGold;
        public Text earnedGold;
        float goldRate = 1f;

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { skillTreeButton, statsButton, doneButton };
        }
        protected override void AddListeners()
        {
            base.AddListeners();
            skillTreeButton.onClick.AddListener(OnSkillTree);
            statsButton.onClick.AddListener(OnStats);
            doneButton.onClick.AddListener(OnDone);
        }
        
        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            FindHeroes();

            base.Init();
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
            
            ActivateButtons();
        }

        void ActivateButtons()
        {
            foreach (var b in listOfButtons)
                b.interactable = true;

            SetButtonNavigation();
        }

        void OnSkillTree()
        {
            uiManager.PushMenu(uiManager.uiDatabase.SkillTreeMenu);
            var skillTree = uiManager.CurrentMenu.GetComponent<SkillTreeMenu>();

            int heroIndex;

            if (jethroCard.LeveledUp)
                heroIndex = 0;
            else if (coleCard.LeveledUp)
                heroIndex = 1;
            else if (eleanorCard.LeveledUp)
                heroIndex = 2;
            else if (joulietteCard.LeveledUp)
                heroIndex = 3;
            else
                heroIndex = 0;

            skillTree.SetHero(heroIndex);
                
                
        }
        void OnStats()
        {
            uiManager.PushMenu(uiManager.uiDatabase.StatPointsMenu);
            var statPoints = uiManager.CurrentMenu.GetComponent<StatPointsMenu>();

            int heroIndex;

            if (jethroCard.LeveledUp)
                heroIndex = 0;
            else if (coleCard.LeveledUp)
                heroIndex = 1;
            else if (eleanorCard.LeveledUp)
                heroIndex = 2;
            else if (joulietteCard.LeveledUp)
                heroIndex = 3;
            else
                heroIndex = 0;

            statPoints.SetInitialHero(heroIndex);
        }
        void OnDone()
        {
            gameControl.ReturnFromBattle();
        }
    }
}