namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class BattleUI : MonoBehaviour
    {
        public Text jethroText;
        public Text coleText;
        public Text eleanorText;
        public Text joulietteText;

        public List<Text> enemyTextList;

        BattleManager battleManager;

        public void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();

            // assign text to denigens

            // HEROES
            // (this function is called before the denigens are sorted by speed)
            battleManager.heroList[0].statsText = jethroText;
            battleManager.heroList[1].statsText = coleText;
            battleManager.heroList[2].statsText = eleanorText;
            battleManager.heroList[3].statsText = joulietteText;

            // ENEMIES
            for(int i = 0; i < battleManager.enemyList.Count; i++)
            {
                battleManager.enemyList[i].statsText = enemyTextList[i];
            }

            // disable any unassigned enemy UIs
            for(int i = battleManager.enemyList.Count; i < enemyTextList.Count; i++)
            {
                enemyTextList[i].gameObject.SetActive(false);
            }
            
            AssignStats();
        }

        void AssignStats()
        {
            foreach(var d in battleManager.denigenList)
            {
                UpdateStats(d);
            }
        }

        public void UpdateStats(Denigen d)
        {
            d.statsText.text =
                    d.DenigenName + "\n" +
                    "HP: " + d.Hp + " / " + d.HpMax + "\n" +
                    "PM: " + d.Pm + " / " + d.PmMax;
        }

    }
}