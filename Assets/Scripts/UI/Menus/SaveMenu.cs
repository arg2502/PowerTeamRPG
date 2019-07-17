namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class SaveMenu : Menu
    {
        public Button slot1, slot2, slot3;
        public Text text;

        public override Button AssignRootButton()
        {
            return slot1;
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>();
            listOfButtons.Add(slot1);
            listOfButtons.Add(slot2);
            listOfButtons.Add(slot3);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                // temp -- real version will be lambda that passes in int for which file to load
                var index = i + 1;
                listOfButtons[i].onClick.AddListener(() => Save(index));
            }
        }

        void Save(int index)
        {
            print("saving index: " + index);
            // TEMP -- FOR NOW, just load whatever file is saved
            GameControl.control.Save(index);
        }
    }
}