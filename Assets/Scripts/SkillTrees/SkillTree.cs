using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SkillTree : MonoBehaviour {

   
    // true if player is sorting through skill tree
    bool insideTree;
    // 2D array of button layout
    // COLUMN MAJOR (i = column number, j = row number)
    public GameObject[,] button2DArray;

    // button states
    // inactive = player does not have the skill, but has the possibility to acquire it
    // disabled = skill is locked off to player/has to unlock an earlier skill to make it inactive

    public SkillTree() { }

    // individual tree
    // for switching between trees on the fly
    public struct MyTree{      

        // List containing all of a column's technique
        // Example contentArray[0] = {"Fire Breath Lvl 1", "Fire Breath Lvl 2", etc.}
        public List<Technique> listOfContent;

        // ints to give the size of the 2D Array
        public int numOfColumn;
        public int numOfRow;

        // starting root node
        public int rootCol;
        public int rootRow;

        public Dictionary<Technique, List<Technique>> treeLinesDictionary;
              
    }

    // which skilltree to show
    protected List<string> whichContent;    
    
    public List<MyTree> listOfTrees; 

    // denigen obj to access specific techniques
    protected DenigenData hero;

    // see who else needs to level up
    protected bool levelUp = false;

    // technique description
    protected GameObject descriptionText;
    protected GameObject remainingPts;

    protected List<string[]> listOfTechniques;
    
    protected TextAsset readIn;
    
    public List<Technique> startingTechs;

    public void EndScene()
    {
        foreach (DenigenData hd in GameControl.control.heroList)
        {
            if (hd.identity == hero.identity)
            {
                hd.skillsList = hero.skillsList;
                hd.spellsList = hero.spellsList;
                hd.passiveList = hero.passiveList;
                hd.techPts = hero.techPts;
            }
            // also use this loop to figure out if we need to level up another hero
            if (hd.statBoost) { levelUp = true; }
        }

        // Either go back to current room, or move to level up the next hero
        // This should be in the skills area, but it is here since I haven't done the skills yet
        if (levelUp == true) { SceneManager.LoadScene("LevelUpMenu"); }
        else { SceneManager.LoadScene(GameControl.control.currentScene); }
    }
    
    public void ReadInfo(string path)
    {
        // read in info
        listOfTechniques = new List<string[]>();

        readIn = Resources.Load<TextAsset>("SkillTrees/" + path);
        
        string[] lines = readIn.text.Split('\n');

        // skip first line (contains "Key, Title, etc...")
        if (lines.Length <= 1) return;

        for (int i = 1; i < lines.Length; i++)
            listOfTechniques.Add(lines[i].Split('\t'));// makes each line a string array and adds to list
    }

    public string[] FindTechnique(string key)
    {
        return listOfTechniques.Find((technique) => technique[0] == key);
    }
    
}


