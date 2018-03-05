namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class EnemyTargetMenu : Menu
    {        
        List<Button> enemyCursors = new List<Button>();

        BattleManager battleManager;

        protected override void AddButtons()
        {
            base.AddButtons();

            // add buttons programmatically rather than through inspector -- simpler
            var array = GetComponentsInChildren<Button>();
            foreach (var b in array)
                enemyCursors.Add(b);

            // order from top to bottom -- for button navigation purposes
            listOfButtons = new List<Button>();

            for(int i = enemyCursors.Count - 1; i >= 0; i--)
            {
                listOfButtons.Add(enemyCursors[i]);
            }
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            for (int i = 0; i < enemyCursors.Count; i++)
            {
                var tempNum = i;
                enemyCursors[i].onClick.AddListener(() => OnEnemy(tempNum));
            }
        }

        public override Button AssignRootButton()
        {
            for(int i = 0; i < battleManager.enemyList.Count; i++)
            {
                if (!battleManager.enemyList[i].IsDead)
                    return enemyCursors[i];
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
                enemyCursors[i].transform.position = pos;
            }
            
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            rootButton = AssignRootButton();
            SetSelectedObjectToRoot();

            CheckForDead();
        }
        
        void OnEnemy(int pos)
        {
            //print("Whoa, there. This function isn't done yet, sonny.");
            List<Denigen> targets = new List<Denigen>();
            var mainTarget = battleManager.enemyList[pos];
            targets.Add(mainTarget);

            // determine if we need multiple targets
            if(battleManager.targetState == TargetType.ENEMY_SPLASH)
            {
                print("You gotta do splash stuff here! Sorry, not programmed yet");
            }
            if(battleManager.targetState == TargetType.ENEMY_TEAM)
            {
                print("You gotta do team stuff here! Sorry, not programmed yet");
            }

            battleManager.TargetDenigen(targets);
        }

        void CheckForDead()
        {

            // check if the cursors are even linked to an enemy
            // i.e. if there's only one enemy in the battle, 3 cursors aren't pointing anywhere
            // if they are linked, check if the enemy is even alive
            for (int i = 0; i < enemyCursors.Count; i++)
            {
                // if we're out of range, turn off the cursor
                // also, check if alive
                if (i >= battleManager.enemyList.Count || battleManager.enemyList[i].IsDead)
                {
                    enemyCursors[i].gameObject.SetActive(false);
                    enemyCursors[i].interactable = false;
                    SetButtonNavigation();
                }
                else
                {
                    enemyCursors[i].gameObject.SetActive(true);
                    enemyCursors[i].interactable = true;
                }
            }
        }
    }
}