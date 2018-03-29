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

        Button currentButton;
        Button prevButton;
        int currentIndex = -1;

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
                {
                    return targetCursors[i];
                }
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

            currentButton = rootButton;

            //CheckTargetState();
        }
        
        void OnTarget(int pos)
        {
            List<Denigen> targets = new List<Denigen>();
            var mainTarget = currentTargets[pos];
            targets.Add(mainTarget);

            // determine if we need multiple targets
            if(battleManager.targetState == TargetType.ENEMY_SPLASH || battleManager.targetState == TargetType.HERO_SPLASH)
            {
                // add denigen below if not at very bottom
                if (pos > 0)
                    targets.Add(currentTargets[pos - 1]);

                // add denigen above if not at very top
                if (pos < currentTargets.Count - 1)
                    targets.Add(currentTargets[pos + 1]);
            
            }
            if(battleManager.targetState == TargetType.ENEMY_TEAM || battleManager.targetState == TargetType.HERO_TEAM)
            {
                // add the rest of the group to the target list
                for(int i = 0; i < currentTargets.Count; i++)
                {
                    // make sure we don't add the main target twice
                    if (i != pos)
                        targets.Add(currentTargets[i]);
                }
            }

            battleManager.TargetDenigen(targets);

            HideCards();
            prevButton = null;
            
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

        int FindIndexInButtonArray(Button button)
        {
            for (int i = 0; i < targetCursors.Count; i++)
            {
                if (button == targetCursors[i])
                    return i;
            }
            Debug.LogError("Could not find index");
            return -1;
        }

        Denigen FindDenigenFromButton(Button button)
        {
            currentIndex = FindIndexInButtonArray(button);
            return currentTargets[currentIndex];

            //Debug.LogError("Could not find Denigen from button provided: " + button.name);
            //return null;
        }
        void SwitchCards()
        {
            if (prevButton != null)
                FindDenigenFromButton(prevButton).statsCard.ShowShortCard();
            FindDenigenFromButton(currentButton).statsCard.ShowFullCard();
        }
        void HideCards()
        {
            if (prevButton != null)
                FindDenigenFromButton(prevButton).statsCard.ShowShortCard();
            FindDenigenFromButton(currentButton).statsCard.ShowShortCard();
        }

        void CheckTargetState()
        {
            // find index of the button we are currently on
            currentIndex = FindIndexInButtonArray(currentButton);
            int low;
            int high;
            float alpha = 0.5f;

            switch (battleManager.targetState)
            {
                case TargetType.HERO_SPLASH:
                case TargetType.ENEMY_SPLASH:
                    low = currentIndex - 1;
                    high = currentIndex + 1;
                    break;
                case TargetType.HERO_TEAM:
                case TargetType.ENEMY_TEAM:
                    low = 0;
                    high = targetCursors.Count - 1;
                    alpha = 1f;
                    break;
                default:
                    low = currentIndex;
                    high = currentIndex;
                    break;
            }

            ChangeButtonColors(low, high, alpha);
        }

        void ChangeButtonColors(int low, int high, float alpha)
        {
            // tell the buttons we want, to show their highlighted sprites
            // all others to show disabled
            for (int i = 0; i < targetCursors.Count; i++)
            {
                var button = targetCursors[i];
                if (i >= low && i <= high)
                {
                    var color = button.colors.highlightedColor;

                    // always have the current main target at full opacity
                    if (i == currentIndex)
                        color.a = 1f;
                    // if there are others, set their alphas accordingly:
                    // splash -- slightly transparent
                    // team -- fully opaque
                    else
                        color.a = alpha;

                    button.GetComponent<Image>().color = color;
                }
                else
                {
                    // turn all other cursor options invisible
                    var color = button.colors.normalColor;
                    color.a = 0f;
                    button.GetComponent<Image>().color = color;
                }

            }
        }
        new void Update()
        {
            base.Update();

            currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            CheckTargetState();
            if (currentButton == prevButton) return;

            SwitchCards();

            prevButton = currentButton;


        }
    }
}