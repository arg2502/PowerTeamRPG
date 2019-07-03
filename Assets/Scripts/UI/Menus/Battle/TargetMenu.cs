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
                
        float splashScale = 0.5f;

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
            if (!battleManager.IsTargetEnemy)
            {
                // finds the current hero and returns the button
                for (int i = 0; i < currentTargets.Count; i++)
                {
                    if (currentTargets[i] == battleManager.CurrentDenigen)
                        return targetCursors[i];

                }
            }
            else
            {
                for (int i = currentTargets.Count - 1; i >= 0; i--)
                {
                    if (targetCursors[i].interactable)
                    {
                        return targetCursors[i];
                    }
                }
            }
            return null;
        }

        public override void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();
            base.Init();
            
            listSub = uiManager.FindMenu(uiDatabase.ListSub) as ListSub;
        }

        public override void SetButtonNavigation()
        {
            SetButtonNavigationHorizontal();
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            currentTargets.Clear();

            if (battleManager.IsTargetEnemy)
            {
                for (int i = battleManager.enemyList.Count - 1; i >= 0; i--)
                {
                    currentTargets.Add(battleManager.enemyList[i]);
                }

                // setting cursor positions
                var tempList = new List<Denigen>();
                foreach (var h in currentTargets)
                    tempList.Add(h as Denigen);
                SetCursorPositions(tempList);
                SetCursorIcons(battleManager.damageIcon);
            }
            else
            {
                // heroes are backwards for menu navigation purposes (index 0 is Jethro (bottom))
                // (from right to left)
                // JOULIETTE -- JETHRO -- ELEANOR -- COLE (start with joules then go left)
                
                var jethro = battleManager.heroList.Find(hero => hero.Data.identity == 0);
                var cole = battleManager.heroList.Find(hero => hero.Data.identity == 1);
                var eleanor = battleManager.heroList.Find(hero => hero.Data.identity == 2);
                var jouliette = battleManager.heroList.Find(hero => hero.Data.identity == 3);

                if (jouliette) currentTargets.Add(jouliette);
                if (jethro) currentTargets.Add(jethro);
                if (eleanor) currentTargets.Add(eleanor);
                if (cole) currentTargets.Add(cole);

                // setting cursor positions
                var tempList = new List<Denigen>();
                foreach (var h in currentTargets)
                    tempList.Add(h as Denigen);

                SetCursorPositions(tempList);
                SetCursorIcons(battleManager.healIcon);
            }

            CheckForDead();
            rootButton = AssignRootButton();

            // if rootButton is null, then that means there are no available targets and we've most likely already popped out of the menu            
            if (rootButton == null)
                return;
            SetSelectedObjectToRoot();
            currentButton = rootButton;
            CheckTargetState();

        }
        
        void SetCursorPositions(List<Denigen> targets)
        {
            // set positions of cursors
            // convert world coordinates to screen coordinates for UI
            for (int i = 0; i < targets.Count; i++)
            {
                var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targets[i].transform.position);
                targetCursors[i].transform.position = pos;
            }
        }

        void SetCursorIcons(Sprite icon)
        {
            foreach(var button in targetCursors)
            {
                button.GetComponent<Image>().sprite = icon;
            }
        }

        void OnTarget(int pos)
        {
            // turn off all turn order starburst
            battleManager.TurnOffAllHighlightStarburstTurnOrder();

            List<Denigen> targets = new List<Denigen>();
            var mainTarget = currentTargets[pos];
            targets.Add(mainTarget);
            battleManager.CurrentHero.MainTargetIndex = pos;

            // determine if we need multiple targets
            if(battleManager.targetState == TargetType.ENEMY_SPLASH || battleManager.targetState == TargetType.HERO_SPLASH)
            {
                // add denigen below if not at very bottom (and make sure they're still alive)
                if (pos > 0 && !currentTargets[pos - 1].IsDead)
                    targets.Add(currentTargets[pos - 1]);

                // add denigen above if not at very top (and make sure they're still alive)
                if (pos < currentTargets.Count - 1 && !currentTargets[pos + 1].IsDead)
                    targets.Add(currentTargets[pos + 1]);
            
            }
            if(battleManager.targetState == TargetType.ENEMY_TEAM || battleManager.targetState == TargetType.HERO_TEAM)
            {
                // add the rest of the group to the target list
                for(int i = 0; i < currentTargets.Count; i++)
                {
                    // make sure we don't add the main target twice (and make sure they're still alive)
                    if (i != pos && !currentTargets[i].IsDead)
                        targets.Add(currentTargets[i]);
                }
            }

            // increase item's inUse if we're using an item
            if(battleManager.menuState == MenuState.ITEMS)
            {
                // we should have like a find item function ---- TODO
                foreach(var item in gameControl.consumables)
                {
					if (item.name == battleManager.CurrentDenigen.CurrentAttackName)
                    {
                        battleManager.CurrentDenigen.UsingItem = true;
                    }
                }
            }

            battleManager.TargetDenigen(targets);

            prevButton = null;

        }

        void CheckForDead()
        {

            // check if the cursors are even linked to an enemy
            // i.e. if there's only one enemy in the battle, 3 cursors aren't pointing anywhere
            // if they are linked, check if the enemy is even alive
            for (int i = 0; i < targetCursors.Count; i++)
            {
                if (battleManager.menuState != MenuState.ITEMS)
                {
                    // if we're out of range, turn off the cursor
                    // also, check if alive
                    if (i >= currentTargets.Count || currentTargets[i].IsDead)
                        ToggleCursorActivation(index: i, active: false);
                    else
                        ToggleCursorActivation(index: i, active: true);
                }
                else
                {
                    // deactivate if out of range
                    if (i >= currentTargets.Count)
                    {
                        ToggleCursorActivation(index: i, active: false);
                        continue;
                    }
                    // if dead, check if the item can be used on them
                    var itemForTheLiving = GameControl.itemManager.ItemForLiving(battleManager.CurrentDenigen.CurrentAttackName);
                    if (currentTargets[i].IsJustDead)
                    {
                        ToggleCursorActivation(i, !itemForTheLiving);
                    }
                    else
                    {
                        ToggleCursorActivation(i, itemForTheLiving);
                    }
                }
            }

            // check if all are inactive
            int active = 0;
            for(int i = 0; i < targetCursors.Count; i++)
            {
                if (targetCursors[i].interactable)
                    active++;
            }

            // if we don't have any active, GTFO
            if (active <= 0)
            {
                uiManager.PopMenu();
            }
        }

        

        void ToggleCursorActivation(int index, bool active)
        {
            targetCursors[index].gameObject.SetActive(active);
            targetCursors[index].interactable = active;
            SetButtonNavigation();
        }

        int FindIndexInButtonArray(Button button)
        {
            for (int i = 0; i < targetCursors.Count; i++)
            {
                if (button == targetCursors[i])
                    return i;
            }
            return -1;
        }

        Denigen FindDenigenFromButton(Button button)
        {
            currentIndex = FindIndexInButtonArray(button);

            if (currentIndex >= 0)
                return currentTargets[currentIndex];
            else
                return null;
        }
        
        void CheckTargetState()
        {
            // find index of the button we are currently on
            currentIndex = FindIndexInButtonArray(currentButton);
            int low;
            int high;

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
                    //alpha = 1f;
                    break;
                default:
                    low = currentIndex;
                    high = currentIndex;
                    break;
            }

            ChangeButtonSizes(low, high);
        }

        void ChangeButtonSizes(int low, int high)
        {
            // tell the buttons we want, to show their highlighted sprites
            // all others to show disabled
            for (int i = 0; i < targetCursors.Count; i++)
            {
                var button = targetCursors[i];
                var image = button.GetComponent<Image>();

                if (i >= low && i <= high)
                {
                    var color = image.color;
                    color.a = 1f;
                    image.color = color;

                    button.transform.localScale = Vector3.one;


                    // if splash damage, set all but the main target to a smaller size
                    if(battleManager.targetState == TargetType.ENEMY_SPLASH
                        || battleManager.targetState == TargetType.HERO_SPLASH)
                    {
                        if(i != currentIndex)
                        {
                            button.transform.localScale = Vector3.one * splashScale;
                        }
                    }                    
                }
                else
                {
                    //// turn all other cursor options invisible
                    var color = image.color;
                    color.a = 0f;
                    image.color = color;
                }

            }
        }

        public override void Close()
        {
            base.Close();

            prevButton = null;
            currentButton = null;
            
            // turn off all turn order starburst
            battleManager.TurnOffAllHighlightStarburstTurnOrder();

        }

        new void Update()
        {
            base.Update();
            if (uiManager.menuInFocus != this.gameObject) return;
             
            currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            CheckTargetState();
            if (currentButton == prevButton) return;
            
            prevButton = currentButton;
        }
    }
}