namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class TargetMenu : Menu
    {        
        List<Button> targetCursors = new List<Button>();
        List<Denigen> currentTargets = new List<Denigen>();

        BattleManager battleManager;

        ListSub listSub;

        protected override void AddButtons()
        {
            base.AddButtons();
            
            // add buttons programmatically rather than through inspector -- simpler
            var array = GetComponentsInChildren<Button>();
            foreach (var b in array)
                targetCursors.Add(b);

            // order from top to bottom -- for button navigation purposes
            listOfButtons = new List<Button>();

            for(int i = targetCursors.Count - 1; i >= 0; i--)
            {
                listOfButtons.Add(targetCursors[i]);
            }
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            for (int i = 0; i < targetCursors.Count; i++)
            {
                var tempNum = i;
                targetCursors[i].onClick.AddListener(() => OnTarget(tempNum));
            }
        }

        public override Button AssignRootButton()
        {
            for(int i = 0; i < currentTargets.Count; i++)
            {
                if (!currentTargets[i].IsDead)
                    return targetCursors[i];
            }
            return null;
        }

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            base.Init();

            // set positions of cursors
            // convert world coordinates to screen coordinates for UI
            //List<Vector2> screenPos = new List<Vector2>();
            for (int i = 0; i < battleManager.enemyPositions.Count; i++)
            {
                var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, battleManager.enemyPositions[i].transform.position);
                //screenPos.Add(pos);
                targetCursors[i].transform.position = pos;
            }

            listSub = uiManager.FindMenu(uiDatabase.ListSub) as ListSub;
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            currentTargets.Clear();

            if (battleManager.IsTargetEnemy)
            {
                foreach (var enemy in battleManager.enemyList)
                    currentTargets.Add(enemy);
            }
            else
            {
                foreach (var hero in battleManager.heroList)
                    currentTargets.Add(hero);
            }

            rootButton = AssignRootButton();
            SetSelectedObjectToRoot();

            CheckForDead();
        }
        
        void OnTarget(int pos)
        {
            List<Denigen> targets = new List<Denigen>();
            var mainTarget = currentTargets[pos];
            targets.Add(mainTarget);

            // determine if we need multiple targets
            if(battleManager.targetState == TargetType.ENEMY_SPLASH || battleManager.targetState == TargetType.HERO_SPLASH)
            {
                print("You gotta do splash stuff here! Sorry, not programmed yet");
            }
            if(battleManager.targetState == TargetType.ENEMY_TEAM || battleManager.targetState == TargetType.HERO_TEAM)
            {
                print("You gotta do team stuff here! Sorry, not programmed yet");
            }

            battleManager.TargetDenigen(targets);

            listSub.SetContainersToNull();
        }

        void CheckForDead()
        {

            // check if the cursors are even linked to an enemy
            // i.e. if there's only one enemy in the battle, 3 cursors aren't pointing anywhere
            // if they are linked, check if the enemy is even alive
            for (int i = 0; i < targetCursors.Count; i++)
            {
                // if we're out of range, turn off the cursor
                // also, check if alive
                if (i >= currentTargets.Count || currentTargets[i].IsDead)
                {
                    targetCursors[i].gameObject.SetActive(false);
                    targetCursors[i].interactable = false;
                    SetButtonNavigation();
                }
                else
                {
                    targetCursors[i].gameObject.SetActive(true);
                    targetCursors[i].interactable = true;
                }
            }
        }
    }
}