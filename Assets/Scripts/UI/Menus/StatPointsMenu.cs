namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class StatPointsMenu : Menu
    {
        
        public StatPointsMenuItem hp, pm, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
        public Text pointsRemaining;
        int[] originalPoints = new int[4]; // The amount of points each hero had when opening the menu
        int currentHeroIndex;
        HeroData CurrentHero { get { return gameControl.heroList[currentHeroIndex]; } }
        StatPointsMenuItem CurrentStat { get { return EventSystem.current.currentSelectedGameObject.GetComponentInParent<StatPointsMenuItem>(); } }

        bool canSwitchCharacters = true;
        public GameObject heroListGroup;
        public Toggle[] heroListToggles;

        protected override void AddButtons()
        {
            base.AddButtons();

            listOfButtons = new List<Button>()
            {
                hp.statButton,
                pm.statButton,
                atk.statButton,
                def.statButton,
                mgkAtk.statButton,
                mgkDef.statButton,
                luck.statButton,
                evasion.statButton,
                spd.statButton
            };
            
        }

        public override Button AssignRootButton()
        {
            rootButton = hp.statButton;
            return base.AssignRootButton();
        }

        public override void PrePop()
        {
            base.PrePop();

            ConfirmPoints();
        }

        void SetHero(int heroIndex)
        {
            // instantly move list to chosen hero
            MoveHeroList(heroIndex, true);

            currentHeroIndex = heroIndex;
            AssignStats();
            ToggleTextChange();
        }
                
        // Called when first opening menu. (Mainly so we only set the Original Points once.
        // Normal SetHero is called every time the hero switches, and we only want original point set once.        
        public void SetInitialHero(int heroIndex)
        {
            SetOriginalPoints();
            SetHero(heroIndex);
        }

        void AssignStats()
        {
            hp.SetHeroStatValue(currentHeroIndex, CurrentHero.hpMax);
            pm.SetHeroStatValue(currentHeroIndex, CurrentHero.pmMax);
            atk.SetHeroStatValue(currentHeroIndex, CurrentHero.atk);
            def.SetHeroStatValue(currentHeroIndex, CurrentHero.def);
            mgkAtk.SetHeroStatValue(currentHeroIndex, CurrentHero.mgkAtk);
            mgkDef.SetHeroStatValue(currentHeroIndex, CurrentHero.mgkDef);
            luck.SetHeroStatValue(currentHeroIndex, CurrentHero.luck);
            evasion.SetHeroStatValue(currentHeroIndex, CurrentHero.evasion);
            spd.SetHeroStatValue(currentHeroIndex, CurrentHero.spd);

            UpdatePoints();
            UpdateArrowStates();
        }

        /// <summary>
        /// When first opening the menu, record how many level up points each hero had, to compare when closing
        /// </summary>
        void SetOriginalPoints()
        {
            for(int i = 0; i < gameControl.heroList.Count; i++)
            {
                originalPoints[i] = gameControl.heroList[i].levelUpPts;
            }
        }

        void UpdatePoints()
        {
            pointsRemaining.text = "Points: " + gameControl.heroList[currentHeroIndex].levelUpPts.ToString();
        }

        // HERO LIST ANIMATION FUNCTIONS ------------
        #region
        /// <summary>
        /// Called when switching between heroes
        /// </summary>
        /// <param name="hero">GameControl hero index</param>
        void HeroSelect(int hero)
        {
            if (!canSwitchCharacters) return;

            // move hero list before changing currentHero
            MoveHeroList(hero);

            // set new hero stats
            SetHero(hero);

            // refresh button -- to refresh button sprite state
            var buttonObj = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttonObj);
            
            // show hero list change
            ToggleTextChange();

        }
        void ToggleTextChange()
        {
            foreach (var toggle in heroListToggles)
                toggle.isOn = false;

            heroListToggles[currentHeroIndex].isOn = true;
        }
        void IncreaseHero()
        {
            if (currentHeroIndex < gameControl.heroList.Count - 1)
                HeroSelect(currentHeroIndex + 1);
        }
        void DecreaseHero()
        {
            if (currentHeroIndex > 0)
                HeroSelect(currentHeroIndex - 1);
        }
        void MoveHeroList(int newHeroPos, bool instant = false)
        {
            // find where the text needs to go next
            var desiredPosition = heroListToggles[newHeroPos].transform.position;
            var currentPosition = heroListToggles[currentHeroIndex].transform.position;
            var differenceX = -(desiredPosition.x - currentPosition.x);
            var newPosition = new Vector3(heroListGroup.transform.position.x + differenceX, heroListGroup.transform.position.y);

            // move the text over a short period of time -- lerp it
            if (!instant)
                StartCoroutine(LerpText(newPosition));
            else
                heroListGroup.transform.position = newPosition;

        }

        IEnumerator LerpText(Vector3 newPosition)
        {
            // while we're moving the text, disable the ability to switch to another character's tree
            // this is to avoid a bug where the text gets stuck and doesn't move
            canSwitchCharacters = false;

            var startTime = Time.time;
            var lerpTime = 10f;
            var originalPosition = heroListGroup.transform.position;
            while ((newPosition - heroListGroup.transform.position).magnitude >= 0.1f)
            {
                heroListGroup.transform.position = Vector3.Lerp(originalPosition, newPosition, (Time.time - startTime) * lerpTime);
                yield return null;
            }

            // now that the text is where it needs to be, reenable the ability to switch between trees
            canSwitchCharacters = true;
        }
        #endregion
        // END ANIMATION FUNCTIONS -------------- 

        void ChangeCurrentStat(int increment)
        {
            CurrentStat.ChangeStat(currentHeroIndex, increment);
            CurrentHero.levelUpPts += -increment;
        }

        void UpdateArrowStates()
        {
            // Increase Arrow Checks
            // Sets arrows to true if there are more level up points to use
            // Turns them all off if there are no more points to give
            bool increaseActive = (CurrentHero.levelUpPts > 0);            
            
            hp.ToggleIncreaseArrow(increaseActive);
            pm.ToggleIncreaseArrow(increaseActive);
            atk.ToggleIncreaseArrow(increaseActive);
            def.ToggleIncreaseArrow(increaseActive);
            mgkAtk.ToggleIncreaseArrow(increaseActive);
            mgkDef.ToggleIncreaseArrow(increaseActive);
            luck.ToggleIncreaseArrow(increaseActive);
            evasion.ToggleIncreaseArrow(increaseActive);
            spd.ToggleIncreaseArrow(increaseActive);

            // Decrease Arrow Checks
            // Logic checks done in each stat item
            hp.ShouldDecreaseBeActive(currentHeroIndex);
            pm.ShouldDecreaseBeActive(currentHeroIndex);
            atk.ShouldDecreaseBeActive(currentHeroIndex);
            def.ShouldDecreaseBeActive(currentHeroIndex);
            mgkAtk.ShouldDecreaseBeActive(currentHeroIndex);
            mgkDef.ShouldDecreaseBeActive(currentHeroIndex);
            luck.ShouldDecreaseBeActive(currentHeroIndex);
            evasion.ShouldDecreaseBeActive(currentHeroIndex);
            spd.ShouldDecreaseBeActive(currentHeroIndex);

            hp.UpdateChangeText(currentHeroIndex);
            pm.UpdateChangeText(currentHeroIndex);
            atk.UpdateChangeText(currentHeroIndex);
            def.UpdateChangeText(currentHeroIndex);
            mgkAtk.UpdateChangeText(currentHeroIndex);
            mgkDef.UpdateChangeText(currentHeroIndex);
            evasion.UpdateChangeText(currentHeroIndex);
            luck.UpdateChangeText(currentHeroIndex);
            spd.UpdateChangeText(currentHeroIndex);
        }
        
        /// <summary>
        /// Compare the current level up points of each hero with the original points when opening the menu
        /// If the current points are less than original, then there has been a change: Push a confirmation message and change stats if YES.
        /// If the current points are the same, then no changes have been made: Pop menu as would normally occur.
        /// </summary>
        /// <returns></returns>
        bool PointsHaveChanged()
        {
            for(int i = 0; i < gameControl.heroList.Count; i++)
            {
                if (gameControl.heroList[i].levelUpPts < originalPoints[i])
                    return true;
            }

            return false;
        }

        void ConfirmPoints()
        {
            // if we added points, push confirmation menu
            // If yes, save changes and pop menu
            // If no, discard changes and pop menu
            if (PointsHaveChanged())
            {
                uiManager.PushConfirmationMenu("Save changes?", AddPoints, ResetPoints);
            }
            // otherwise, no changes were made. Nothing to save so just pop
            else
            {
                uiManager.PopMenu();
            }
        }

        void AddPoints()
        {
            for(int i = 0; i < gameControl.heroList.Count; i++)
            {
                gameControl.heroList[i].hp += hp.StatChanges[i];
                gameControl.heroList[i].hpMax += hp.StatChanges[i];
                gameControl.heroList[i].pm += pm.StatChanges[i];
                gameControl.heroList[i].pmMax += pm.StatChanges[i];
                gameControl.heroList[i].atk += atk.StatChanges[i];
                gameControl.heroList[i].def += def.StatChanges[i];
                gameControl.heroList[i].mgkAtk += mgkAtk.StatChanges[i];
                gameControl.heroList[i].mgkDef += mgkDef.StatChanges[i];
                gameControl.heroList[i].luck += luck.StatChanges[i];
                gameControl.heroList[i].evasion += evasion.StatChanges[i];
                gameControl.heroList[i].spd += spd.StatChanges[i];
            }

            // Once we've added all the points, reset the changes and pop the menu
            ClearAndPop();
        }

        void ResetPoints()
        {
            // set the heroes' points back to their original
            for(int i = 0; i < gameControl.heroList.Count; i++)
            {
                gameControl.heroList[i].levelUpPts = originalPoints[i];
            }

            ClearAndPop();
        }

        void ClearAndPop()
        {
            // clear all the placeholder changes made to the stat items
            // so that they will be zero again when we return
            hp.ClearArray();
            pm.ClearArray();
            atk.ClearArray();
            def.ClearArray();
            mgkAtk.ClearArray();
            mgkDef.ClearArray();
            luck.ClearArray();
            evasion.ClearArray();
            spd.ClearArray();

            uiManager.PopMenu();
        }

        private new void Update()
        {
            base.Update();

            if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") > 0)
            {
                IncreaseHero();
                UpdateArrowStates();
            }
            else if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") < 0)
            {
                DecreaseHero();
                UpdateArrowStates();
            }

            if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && CurrentHero.levelUpPts > 0)
            {
                ChangeCurrentStat(1);
                UpdatePoints();
                UpdateArrowStates();
            }

            else if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && CurrentStat.StatChanges[currentHeroIndex] > 0)
            {
                ChangeCurrentStat(-1);
                UpdatePoints();
                UpdateArrowStates();
            }

        }
    }
}