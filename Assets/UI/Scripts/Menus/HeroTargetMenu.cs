namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class HeroTargetMenu : Menu
    {
        public Button jethroCursor, coleCursor, eleanorCursor, joulietteCursor;

        BattleManager battleManager;

        protected override void AddButtons()
        {
            base.AddButtons();

            // order from top to bottom -- for button navigation purposes
            listOfButtons = new List<Button>() { eleanorCursor, joulietteCursor, coleCursor, jethroCursor };
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            jethroCursor.onClick.AddListener(OnJethro);
            coleCursor.onClick.AddListener(OnCole);
            eleanorCursor.onClick.AddListener(OnEleanor);
            joulietteCursor.onClick.AddListener(OnJouliette);
        }

        public override Button AssignRootButton()
        {
            for(int i = 0; i < gameControl.heroList.Count; i++)
            {
                if(!gameControl.heroList[i].IsDead)
                {
                    if (i == 0)
                        return jethroCursor;
                    else if (i == 1)
                        return coleCursor;
                    else if (i == 2)
                        return eleanorCursor;
                    else
                        return joulietteCursor;
                }
            }
            return null;
        }

        public override void Init()
        {
            base.Init();

            battleManager = FindObjectOfType<BattleManager>();

            // set positions of cursors
            // convert world coordinates to screen coordinates for UI
            List<Vector2> screenPos = new List<Vector2>();
            for(int i = 0; i < battleManager.heroPositions.Count; i++)
            {
                var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, battleManager.heroPositions[i].transform.position);
                screenPos.Add(pos);
            }

            jethroCursor.transform.position = screenPos[0];
            coleCursor.transform.position = screenPos[1];
            eleanorCursor.transform.position = screenPos[2];
            joulietteCursor.transform.position = screenPos[3];
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

            rootButton = AssignRootButton();
            
        }

        void OnJethro()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnCole()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnEleanor()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
        void OnJouliette()
        {
            print("Whoa, there. This function isn't done yet, sonny.");
        }
    }
}