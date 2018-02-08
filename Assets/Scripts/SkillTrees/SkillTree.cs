using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SkillTree {

   
    // true if player is sorting through skill tree
    bool insideTree;
    // 2D array of button layout
    // COLUMN MAJOR (i = column number, j = row number)
    public GameObject[,] button2DArray;
    int maxRows = 10;
    int maxCols = 10;


    // button states
    // inactive = player does not have the skill, but has the possibility to acquire it
    // disabled = skill is locked off to player/has to unlock an earlier skill to make it inactive

    public SkillTree()
    {

    }

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
    GameObject[] whichButton;
    int whichTree;
    public List<MyTree> listOfTrees;
    MyTree currentTree;
    MyTree prevTree; // to keep track of which buttons to turn on and off while switching

    // 2 ints to keep track of where you are in the button2DArray
    int columnIndex;
    int rowIndex;

    // keep track of the last place you were --- mainly for going to DONE and back
    int prevCol;
    int prevRow;

    // access to camera 
    protected GameObject camera;

    // denigen obj to access specific techniques
    protected HeroData hero;

    // previous button object to keep track of which button to set back to normal
    ButtonSkillTree prevButton;

    // see who else needs to level up
    protected bool levelUp = false;

    // technique description
    protected GameObject descriptionText;
    protected GameObject remainingPts;

    protected List<string[]> listOfTechniques;
    protected StreamReader readIn;

    //public void Start()
    //{     
    //    // for positioning
    //    camera = GameObject.FindGameObjectWithTag("MainCamera");

    //    // which skilltree to show
    //    whichButton = new GameObject[whichContent.Count];

    //    // create which skill tree buttons
    //    for(int i = 0; i < whichContent.Count; i++)
    //    {
    //        whichButton[i] = (GameObject)Instantiate(Resources.Load("Prefabs/SkillTreeButton"));
    //        whichButton[i].name = "WhichButton" + i.ToString();
    //        ButtonSkillTree b = whichButton[i].GetComponent<ButtonSkillTree>();
    //        whichButton[i].transform.position = new Vector2(camera.transform.position.x - 9.375f + (i * (b.width + b.width / 4)), camera.transform.position.y + 4.7f);

    //        // display technique's name - for right now
    //        b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
    //        b.textObject.name = "WhichButtonText" + i.ToString();
    //        b.labelMesh = b.textObject.GetComponent<TextMesh>();
    //        b.labelMesh.text = whichContent[i];
    //        b.labelMesh.transform.position = new Vector3(whichButton[i].transform.position.x, whichButton[i].transform.position.y, -1);

    //    }

    //    currentTree = listOfTrees[0];
    //    // sizes are set in child classes
    //    // currentTree.listOfContent is also set in child classes
    //    // then base.Start() is called
    //    //int rowSize = currentTree.numOfRow + 1;
    //    //NewTree();
    //    button2DArray = new GameObject[maxCols, maxRows + 1];

    //    // set indexs to start
    //    columnIndex = 0;
    //    rowIndex = 0;

    //    for (int col = 0; col < maxCols; col++)
    //    {
    //        for (int row = 0; row < maxRows; row++)
    //        {                
    //            button2DArray[col, row] = (GameObject)Instantiate(Resources.Load("Prefabs/SkillTreeButton"));
    //            button2DArray[col, row].name = "SkillTreeButton" + col + "," + row;
    //            ButtonSkillTree b = button2DArray[col, row].GetComponent<ButtonSkillTree>();
    //            button2DArray[col, row].transform.position = new Vector2(camera.transform.position.x - 9.375f + (col * (b.width + b.width / 4)), camera.transform.position.y + (3.9f - b.height) + (row * -(b.height + b.height / 2)));

    //            // display technique's name - for right now
    //            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
    //            b.textObject.name = "SkillTreeText" + col + "," + row;
    //            b.labelMesh = b.textObject.GetComponent<TextMesh>();
    //            // default buttons are inactive
    //            b.state = ButtonSkillTree.MyButtonTextureState.inactive;

    //            b.labelMesh.text = "";
    //            b.labelMesh.transform.position = new Vector3(button2DArray[col, row].transform.position.x, button2DArray[col, row].transform.position.y, -1);
    //            b.GetComponent<SpriteRenderer>().enabled = false;

    //        }
    //    }               
    //    foreach (Technique t in currentTree.listOfContent)
    //    {
    //        ButtonSkillTree b = button2DArray[t.ColPos, t.RowPos].GetComponent<ButtonSkillTree>();
    //        b.GetComponent<SpriteRenderer>().enabled = true;
    //        b.labelMesh.text = t.Name;

    //        // link technique to buttons
    //        t.Button = b;
    //        b.Technique = t;
    //        // setup next list
    //        //t.Button.ListNextButton = new List<ButtonSkillTree>();

    //        // check if the denigen has already learned the technique
    //        CheckForTechniques(t, b);
    //    }

    //    prevTree = currentTree;

    //    // set the next button states
    //   // UpdateButtons();

    //    // add on Done button at the very end
    //    button2DArray[0, maxRows] = (GameObject)Instantiate(Resources.Load("Prefabs/SkillTreeButton"));
    //    button2DArray[0, maxRows].name = "DoneButton";
    //    ButtonSkillTree button = button2DArray[0, maxRows].GetComponent<ButtonSkillTree>();
    //    button2DArray[0, maxRows].transform.position = new Vector2(camera.transform.position.x - 9.375f, camera.transform.position.y - 6.25f);

    //    // display "Done" text
    //    button.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
    //    button.textObject.name = "DoneText";
    //    button.labelMesh = button.textObject.GetComponent<TextMesh>();
    //    button.labelMesh.text = "Done";
    //    button.labelMesh.transform.position = new Vector3(button2DArray[0, maxRows].transform.position.x, button2DArray[0, maxRows].transform.position.y, -1);

    //    // set to normal
    //    button.state = ButtonSkillTree.MyButtonTextureState.normal;

    //    // set previous button to the current button
    //    prevButton = button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>();

    //    // set which tree button state
    //    whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;

    //    // description text
    //    descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
    //    descriptionText.name = "DescriptionText";
    //    descriptionText.GetComponent<TextMesh>().text = ""; // FormatText(button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().Technique.Description);
    //    descriptionText.transform.position = new Vector2(button2DArray[currentTree.numOfColumn - 1, 0].transform.position.x + 6.25f, button2DArray[currentTree.numOfColumn - 1, 0].transform.position.y + 0.78f);

    //    remainingPts = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
    //    remainingPts.name = "RemainingPointsText";
    //    remainingPts.GetComponent<TextMesh>().text = "Skill Points: " + hero.techPts;
    //    remainingPts.transform.position = new Vector2(camera.transform.position.x - 10.9f, button2DArray[0, currentTree.numOfRow].transform.position.y + 7f);


    //}
    void CheckForTechniques(Technique t, ButtonSkillTree b)
    {
        for (int i = 0; i < hero.skillsList.Count; i++)
        {
            if (t.Name == hero.skillsList[i].Name)
            {
                b.state = ButtonSkillTree.MyButtonTextureState.normal;
                t.Active = true;
                break;
            }
        }
        // if we still haven't found the technique, check spells
        if (b.state != MyButton.MyButtonTextureState.normal)
        {
            for (int i = 0; i < hero.spellsList.Count; i++)
            {
                if (t.Name == hero.spellsList[i].Name)
                {
                    b.state = ButtonSkillTree.MyButtonTextureState.normal;
                    t.Active = true;
                    break;
                }
            }
        }
        // still no luck? check passives
        if (b.state != MyButton.MyButtonTextureState.normal)
        {
            for (int i = 0; i < hero.passiveList.Count; i++)
            {
                if (t.Name == hero.passiveList[i].Name)
                {
                    b.state = ButtonSkillTree.MyButtonTextureState.normal;
                    t.Active = true;
                    break;
                }
            }
        }
    }
    void NewTree()
    {
        ButtonSkillTree b;
        for (int col = 0; col < maxCols; col++)
        {
            for (int row = 0; row < maxRows; row++)
            {
                b = button2DArray[col, row].GetComponent<ButtonSkillTree>();
                // default buttons are inactive
                b.state = ButtonSkillTree.MyButtonTextureState.inactive;
                if (b.labelMesh.text != "Done") { b.labelMesh.text = ""; }
                b.GetComponent<SpriteRenderer>().enabled = false;

            }
        }
        foreach (Technique t in currentTree.listOfContent)
        {
            b = button2DArray[t.ColPos, t.RowPos].GetComponent<ButtonSkillTree>();
            b.GetComponent<SpriteRenderer>().enabled = true;
            b.labelMesh.text = t.Name;

            // link technique to buttons & vice versa
            t.Button = b;
            b.Technique = t;

            // setup next list
            //if (t.Button.ListNextButton == null)
            // { t.Button.ListNextButton = new List<ButtonSkillTree>(); }



            // check if the denigen has already learned the technique
            CheckForTechniques(t, b);
        }
    }
    
    public void EndScene()
    {
        foreach (HeroData hd in GameControl.control.heroList)
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

        readIn = new StreamReader("./Assets/Scripts/SkillTrees/" + path);

        string line = readIn.ReadLine();

        // skip first line (contains "Key, Title, etc...")
        line = readIn.ReadLine();

        while (line != null)
        {
            listOfTechniques.Add(line.Split('\t')); // makes each line a string array and adds to list
            line = readIn.ReadLine();
        }
    }

    public string[] FindTechnique(string key)
    {
        // find the correct row
        //int row = 0;
        for (int i = 0; i < listOfTechniques.Count; i++)
        {
            if (listOfTechniques[i][0] == key)
            {
                //row = i;
                return listOfTechniques[i];
            }
        }
        return null;        
    }
    
}


