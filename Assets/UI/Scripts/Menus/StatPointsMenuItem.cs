namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class StatPointsMenuItem : MonoBehaviour
    {
        public Button statButton;
        public Text statValue;
        public Image decreaseArrow;
        public Image increaseArrow;
        // current stat value ???
        int currentStat = -1;

        // holds the changes made to the heroes stats before confirming
        // 4 slots for each hero
        int[] statChanges = { 0, 0, 0, 0 };
        public int[] StatChanges { get { return statChanges; } }

        // sets the text value based on the passed in stat of the current hero
        public void SetHeroStatValue(int hero, int stat)
        {
            currentStat = stat;
            var newStat = currentStat + statChanges[hero];
            statValue.text = newStat.ToString();
        }

        //// returns the new stat based off the current stat and the changes made to it
        //// this should be called when confirming the changes
        //public int GetHeroStatValue(int hero, int stat)
        //{
        //    return stat + statChanges[hero];
        //}

        public void ChangeStat(int hero, int increment)
        {
            statChanges[hero] += increment;
            SetHeroStatValue(hero, currentStat);
        }

        public void ShouldDecreaseBeActive(int hero)
        {
            if (statChanges[hero] > 0)
                ToggleDecreaseArrow(true);
            else if (decreaseArrow.gameObject.activeSelf)
                ToggleDecreaseArrow(false);                
        }

        public void ToggleDecreaseArrow(bool active)
        {
            decreaseArrow.gameObject.SetActive(active);
        }

        public void ToggleIncreaseArrow(bool active)
        {
            increaseArrow.gameObject.SetActive(active);
        }

        public void ClearArray()
        {
            System.Array.Clear(statChanges, 0, statChanges.Length);
        }
    }
}