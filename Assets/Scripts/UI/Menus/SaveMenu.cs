namespace UI
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class SaveMenu : Menu
    {
        public Button slot1, slot2, slot3;

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
                listOfButtons[i].onClick.AddListener(() => OnSave(index));
            }
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();
            SetButtonText();
        }

        
        void SetButtonText()
        {
            for (int i = 0; i < listOfButtons.Count; i++)
            {
                var textObj = listOfButtons[i].GetComponentInChildren<Text>();
                var filePath = Application.persistentDataPath + "/playerInfo" + (i + 1).ToString() + ".dat";
                //var filePath = Path.Combine(Application.persistentDataPath, "/playerInfo", (i + 1).ToString(), ".dat");
                if (File.Exists(filePath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(filePath, FileMode.Open);
                    PlayerData data = (PlayerData)bf.Deserialize(file);
                    file.Close();

                    textObj.text = data.heroList[0].name + "\n" + data.currentScene + "\n" + "Gold: " + data.totalGold;
                }
                else
                {
                    textObj.text = "Empty";
                }
            }
        }

        void OnSave(int index)
        {
            var filePath = Application.persistentDataPath + "/playerInfo" + (index).ToString() + ".dat";
            //var filePath = Path.Combine(Application.persistentDataPath, "/playerInfo", (index + 1).ToString(), ".dat");
            if (File.Exists(filePath))
            {
                uiManager.PushConfirmationMenu("Are you sure you want to overwrite this save file?", ()=> { Save(index); });
            }
            else
            {
                Save(index);
            }
        }

        void Save(int index)
        {
            GameControl.control.Save(index);
            Refresh();
        }
    }
}