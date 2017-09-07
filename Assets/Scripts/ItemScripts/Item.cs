using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Item : MonoBehaviour {

    //Attributes
    public string name;
    public string description;
    public int quantity = 1;
    public int price;
    public Sprite sprite;

    // counts how many denigens were commanded to use this item, since simply decreasing quantity upon issuing an item's use
    // does not take into account what happens if the denigen dies before using said item, or if victory is acheived before then
    // out of battle it makes sure multiple heroes don't wear the same armor
    public int uses;

    // variables used to make effects easier to impliment
    public string statusChange;
    public int hpChange;
    public int pmChange;
    public int atkChange;
    public int defChange;
    public int mgkAtkChange;
    public int mgkDefChange;
    public int luckChange;
    public int evadeChange;
    public int spdChange;

    public List<HeroData> listOfHeroes = new List<HeroData>();

	// Use this for initialization
	protected void Start () {
	    
	}

    public void Use()
    {
        // items will have their own overrides
    }

    protected void AddHeroAndSortList(HeroData hd)
    {
        // add hero to list
        listOfHeroes.Add(hd);

        // bubble sort through heroes to display them in identity order
        // 
        // NOTE: this is not the greatest sorting algorithm as it is O(n^2),
        // but seeing as there will only ever be 4 elements max, this shouldn't be much cause for concern

        bool wasSwapped;
        do
        {
            wasSwapped = false;
            for (int i = 0; i < listOfHeroes.Count - 1; i++)
            {
                if (listOfHeroes[i + 1].identity < listOfHeroes[i].identity)
                {
                    HeroData temp = listOfHeroes[i];
                    listOfHeroes[i] = listOfHeroes[i + 1];
                    listOfHeroes[i + 1] = temp;
                    wasSwapped = true;
                }
            }
        }
        while (wasSwapped);
    }
}
