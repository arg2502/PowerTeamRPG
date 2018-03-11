namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

    public class BattleUI : MonoBehaviour
    {
        BattleManager battleManager;

        public List<GameObject> heroCursors;
        public List<GameObject> enemyCursors;

        public Text battleMessage;

        public GameObject HeroStats;

        // UI ELEMENTS
        [Header("Names")]
        public Text jethroName;
        public Text coleName, eleanorName, joulietteName;
        [Header("HP")]
        public Text jethroHP;
        public Text coleHP, eleanorHP, joulietteHP;
        [Header("PM")]
        public Text jethroPM;
        public Text colePM, eleanorPM, jouliettePM;
        int statsLength = 3;

        public void Init()
        {
            battleManager = FindObjectOfType<BattleManager>();

            InitUIStats();

            // turn off cursors
            TurnOffAllCursors();
        }
                
        void TurnOffAllCursors()
        {
            foreach (var cursor in heroCursors)
                cursor.SetActive(false);
            foreach (var cursor in enemyCursors)
                cursor.SetActive(false);
        }

        void InitUIStats()
        {
            // assign text to denigens

            // HEROES
            // (this function is called before the denigens are sorted by speed)
            var jethroText = new Denigen.StatsText();
            jethroText.NAME = jethroName;
            jethroText.HP = jethroHP;
            jethroText.PM = jethroPM;
            battleManager.heroList[0].statsText.NAME = jethroName;//jethroText;
            battleManager.heroList[0].statsText.HP = jethroHP;
            battleManager.heroList[0].statsText.PM = jethroPM;

            var coleText = new Denigen.StatsText();
            coleText.NAME = coleName;
            coleText.HP = coleHP;
            coleText.PM = colePM;
            battleManager.heroList[1].statsText = coleText;

            var eleanorText = new Denigen.StatsText();
            eleanorText.NAME = eleanorName;
            eleanorText.HP = eleanorHP;
            eleanorText.PM = eleanorPM;
            battleManager.heroList[2].statsText = eleanorText;

            var joulietteText = new Denigen.StatsText();
            joulietteText.NAME = joulietteName;
            joulietteText.HP = joulietteHP;
            joulietteText.PM = jouliettePM;
            battleManager.heroList[3].statsText = joulietteText;

            AssignStats();
        }

        void AssignStats()
        {
            foreach (var d in battleManager.heroList)
            {
                UpdateStats(d);
            }
        }

        public void UpdateStats(Denigen d)
        {
            d.statsText.NAME.text = d.DenigenName;
            d.statsText.HP.text = "HP: " + StatsToString(d.Hp) + " / " + StatsToString(d.HpMax);
            d.statsText.PM.text = "PM: " + StatsToString(d.Pm) + " / " + StatsToString(d.PmMax);
        }

        /// <summary>
        /// converts a stats to a string, and adds appropriate spaces beforehand for formatting purposes
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        string StatsToString(int stat)
        {
            var statString = stat.ToString();

            // add empty spaces at beginning if less than the desired length of string
            for (int strLength = statString.Length; strLength < statsLength; strLength++)
            {
                statString = " " + statString;
            }

            return statString;
        }
    }
}