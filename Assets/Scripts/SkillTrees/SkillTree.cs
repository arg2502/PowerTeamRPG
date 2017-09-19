using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SkillTree : MonoBehaviour {

   
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



    // individual tree
    // for switching between trees on the fly
    protected struct MyTree{      

        // List containing all of a column's technique
        // Example contentArray[0] = {"Fire Breath Lvl 1", "Fire Breath Lvl 2", etc.}
        public List<Technique> listOfContent;

        // ints to give the size of the 2D Array
        public int numOfColumn;
        public int numOfRow;

        // starting root node
        public int rootCol;
        public int rootRow;        
    }

    // which skilltree to show
    protected List<string> whichContent;
    GameObject[] whichButton;
    int whichTree;
    protected List<MyTree> listOfTrees;
    MyTree currentTree;
    MyTree prevTree; // to keep track of which buttons to turn on and off while switching

    // 2 ints to keep track of where you are in the button2DArray
    int columnIndex;
    int rowIndex;

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

    public void Start()
    {     
        // for positioning
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        // which skilltree to show
        whichButton = new GameObject[whichContent.Count];

        // create which skill tree buttons
        for(int i = 0; i < whichContent.Count; i++)
        {
            whichButton[i] = (GameObject)Instantiate(Resources.Load("Prefabs/SkillTreeButton"));
            whichButton[i].name = "WhichButton" + i.ToString();
            ButtonSkillTree b = whichButton[i].GetComponent<ButtonSkillTree>();
            whichButton[i].transform.position = new Vector2(camera.transform.position.x - 9.375f + (i * (b.width + b.width / 4)), camera.transform.position.y + 4.7f);

            // display technique's name - for right now
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.textObject.name = "WhichButtonText" + i.ToString();
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = whichContent[i];
            b.labelMesh.transform.position = new Vector3(whichButton[i].transform.position.x, whichButton[i].transform.position.y, -1);

        }

        currentTree = listOfTrees[0];
        // sizes are set in child classes
        // currentTree.listOfContent is also set in child classes
        // then base.Start() is called
        //int rowSize = currentTree.numOfRow + 1;
        //NewTree();
        button2DArray = new GameObject[maxCols, maxRows + 1];

        // set indexs to start
        columnIndex = 0;
        rowIndex = 0;

        for (int col = 0; col < maxCols; col++)
        {
            for (int row = 0; row < maxRows; row++)
            {                
                button2DArray[col, row] = (GameObject)Instantiate(Resources.Load("Prefabs/SkillTreeButton"));
                button2DArray[col, row].name = "SkillTreeButton" + col + "," + row;
                ButtonSkillTree b = button2DArray[col, row].GetComponent<ButtonSkillTree>();
                button2DArray[col, row].transform.position = new Vector2(camera.transform.position.x - 9.375f + (col * (b.width + b.width / 4)), camera.transform.position.y + (3.9f - b.height) + (row * -(b.height + b.height / 2)));

                // display technique's name - for right now
                b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
                b.textObject.name = "SkillTreeText" + col + "," + row;
                b.labelMesh = b.textObject.GetComponent<TextMesh>();
                // default buttons are inactive
                b.state = ButtonSkillTree.MyButtonTextureState.inactive;

                b.labelMesh.text = "";
                b.labelMesh.transform.position = new Vector3(button2DArray[col, row].transform.position.x, button2DArray[col, row].transform.position.y, -1);
                b.GetComponent<SpriteRenderer>().enabled = false;

            }
        }               
        foreach (Technique t in currentTree.listOfContent)
        {
            ButtonSkillTree b = button2DArray[t.ColPos, t.RowPos].GetComponent<ButtonSkillTree>();
            b.GetComponent<SpriteRenderer>().enabled = true;
            b.labelMesh.text = t.Name;

            // link technique to buttons
            t.Button = b;
            b.Technique = t;
            // setup next list
            //t.Button.ListNextButton = new List<ButtonSkillTree>();

            // check if the denigen has already learned the technique
            for (int i = 0; i < hero.skillsList.Count; i++)
            {
                if (t.Name == hero.skillsList[i].Name)
                {
                    b.state = ButtonSkillTree.MyButtonTextureState.normal;
                    t.Active = true;
                }
            }
            for (int i = 0; i < hero.spellsList.Count; i++)
            {
                if (t.Name == hero.spellsList[i].Name)
                {
                    b.state = ButtonSkillTree.MyButtonTextureState.normal;
                    t.Active = true;
                }
            }
        }

        prevTree = currentTree;

        // set the next button states
       // UpdateButtons();

        // add on Done button at the very end
        button2DArray[0, maxRows] = (GameObject)Instantiate(Resources.Load("Prefabs/SkillTreeButton"));
        button2DArray[0, maxRows].name = "DoneButton";
        ButtonSkillTree button = button2DArray[0, maxRows].GetComponent<ButtonSkillTree>();
        button2DArray[0, maxRows].transform.position = new Vector2(camera.transform.position.x - 9.375f, camera.transform.position.y - 6.25f);

        // display "Done" text
        button.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        button.textObject.name = "DoneText";
        button.labelMesh = button.textObject.GetComponent<TextMesh>();
        button.labelMesh.text = "Done";
        button.labelMesh.transform.position = new Vector3(button2DArray[0, maxRows].transform.position.x, button2DArray[0, maxRows].transform.position.y, -1);

        // set to normal
        button.state = ButtonSkillTree.MyButtonTextureState.normal;

        // set previous button to the current button
        prevButton = button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>();

        // set which tree button state
        whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;

        // description text
        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        descriptionText.name = "DescriptionText";
        descriptionText.GetComponent<TextMesh>().text = ""; // FormatText(button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().Technique.Description);
        descriptionText.transform.position = new Vector2(button2DArray[currentTree.numOfColumn - 1, 0].transform.position.x + 6.25f, button2DArray[currentTree.numOfColumn - 1, 0].transform.position.y + 0.78f);

        remainingPts = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        remainingPts.name = "RemainingPointsText";
        remainingPts.GetComponent<TextMesh>().text = "Skill Points: " + hero.techPts;
        remainingPts.transform.position = new Vector2(camera.transform.position.x - 10.9f, button2DArray[0, currentTree.numOfRow].transform.position.y + 7f);


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
            for (int i = 0; i < hero.skillsList.Count; i++)
            {
                if (t.Name == hero.skillsList[i].Name)
                {
                    b.state = ButtonSkillTree.MyButtonTextureState.normal;
                    t.Active = true;
                }
            }
            for (int i = 0; i < hero.spellsList.Count; i++)
            {
                if (t.Name == hero.spellsList[i].Name)
                {
                    b.state = ButtonSkillTree.MyButtonTextureState.normal;
                    t.Active = true;
                }
            }   
        }
    }
    string FormatText(string str)
    {
        string formattedString = null;
        int desiredLength = 40;
        string[] wordArray = str.Split(' ');
        int lineLength = 0;
        foreach (string s in wordArray)
        {
            //if the current line plus the length of the next word and SPACE is greater than the desired line length
            if (s.Length + 1 + lineLength > desiredLength)
            {
                //go to new line
                formattedString += "\n" + s + " ";
                //starting a new line
                lineLength = s.Length;
            }
            else
            {
                formattedString += s + " ";
                lineLength += s.Length + 1;
            }
        }
        return formattedString;
    }
    void ScrollHorizontal(int col)
    {
        // stop if on DONE button
        if (rowIndex >= maxRows) return;

        // if row is greater, that means we're scrolling up, increase positive
        int iterator = (col > columnIndex ? 1 : -1);

        while(col < maxCols && col >= 0)
        {
            if (button2DArray[col, rowIndex].GetComponent<SpriteRenderer>().enabled == true)
            {
                columnIndex = col;
                ScrollChangeButtonState();
                return;
            }
            else
            {
                //int startingIndex = rowIndex; // start searching at this column
                //bool goDown = false; // search starts by going right, then if one is not found, go back to start and search left
                //for (int i = 0; i < maxCols; i++)
                //{
                //    if (button2DArray[col, startingIndex].GetComponent<SpriteRenderer>().enabled == true)
                //    {
                //        rowIndex = startingIndex;
                //        columnIndex = col;
                //        ScrollChangeButtonState();
                //        return;
                //    }

                //    // no match - search the next column (to the right)
                //    if (!goDown) startingIndex++;
                //    // still no match, and now we're searching left - go to the next column to the left
                //    else if (rowIndex != 0) startingIndex--;

                //    // we've reached the end at the right, go back to starting and search the left side now
                //    if (startingIndex >= maxRows && rowIndex != 0) { goDown = true; startingIndex = rowIndex - 1; }
                //}

                // just check immediately above or below, no further
                if (rowIndex > 0 && button2DArray[col, rowIndex - 1].GetComponent<SpriteRenderer>().enabled == true)
                {
                    rowIndex--;
                    columnIndex = col;
                    ScrollChangeButtonState();
                    return;
                }
                else if(rowIndex < maxRows - 1 && button2DArray[col, rowIndex + 1].GetComponent<SpriteRenderer>().enabled == true)
                {
                    rowIndex++;
                    columnIndex = col;
                    ScrollChangeButtonState();
                    return;
                }

                col += iterator;
            }
        }
        // if we have reached this point, this is the last in the row
        // TODO: figure out where else to go
            // Maybe look at the rows above and below and keep track of how far the next button is in that row,
            // Choose the row that requires the least distance to move
        
    }
    void ScrollVertical(int row)
    {
        // if col is greater, that means we're scrolling right, increase positive
        int iterator = (row > rowIndex ? 1 : -1);

        while (row < maxCols && row >= 0)
        {
            // if the button directly above/below is enabled, go to that one
            if (button2DArray[columnIndex, row].GetComponent<SpriteRenderer>().enabled == true)
            {
                rowIndex = row;
                ScrollChangeButtonState();
                return;
            }
            // otherwise, check the rest of the row for the next available button
            else
            {
                int startingIndex = columnIndex; // start searching at this column
                bool goLeft = false; // search starts by going right, then if one is not found, go back to start and search left
                for(int i = 0; i < maxRows; i++)
                {
                    if(button2DArray[startingIndex, row].GetComponent<SpriteRenderer>().enabled == true)
                    {
                        rowIndex = row;
                        columnIndex = startingIndex;
                        ScrollChangeButtonState();
                        return;
                    }

                    // no match - search the next column (to the right)
                    if (!goLeft) startingIndex++;
                    // still no match, and now we're searching left - go to the next column to the left
                    else if(columnIndex != 0) startingIndex--;

                    // we've reached the end at the right, go back to starting and search the left side now
                    if (startingIndex >= maxRows && columnIndex != 0) { goLeft = true; startingIndex = columnIndex - 1; }
                }

                // if the entire row is disabled, go to the next row and search again
                row += iterator;
            }
        }
        if (button2DArray[columnIndex, rowIndex + 1].GetComponent<SpriteRenderer>().enabled == false
                        || rowIndex + 1 == currentTree.numOfRow)
        {
            // automatically send them to DONE
            columnIndex = 0;
            rowIndex = maxRows;
            ScrollChangeButtonState();
        }

    }
    void ScrollChangeButtonState()
    {
        // check if the button is inactive
        // if it is, set it to inactiveHover
        if (button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.inactive)
        {
            button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.inactiveHover;
        }
        // if it's a disabled button, set disabled hover
        else if(button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.disabled)
        {
            button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.disabledHover;
        }
        // if it is not inactive, set it to normal hover
        else
        {
            button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;
        }

        // now change the previous button state
        // if the last button was inactiveHover, set it to inactive
        if (prevButton != button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>())
        {
            if (prevButton.state == ButtonSkillTree.MyButtonTextureState.inactiveHover)
            {
                prevButton.state = ButtonSkillTree.MyButtonTextureState.inactive;
            }
            // if it's disabled hover, set back to disabled
            else if (prevButton.state == ButtonSkillTree.MyButtonTextureState.disabledHover)
            {
                prevButton.state = ButtonSkillTree.MyButtonTextureState.disabled;
            }
            // if it was a normal button, set it back to normal        
            else if (prevButton.state == ButtonSkillTree.MyButtonTextureState.hover)
            {
                prevButton.state = ButtonSkillTree.MyButtonTextureState.normal;
            }
        }
        // set previous button to new current button
        prevButton = button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>();

        // text positions
        if (rowIndex < currentTree.numOfRow)
        {
            foreach(Technique t in currentTree.listOfContent)
            {
                if(columnIndex == t.ColPos
                    && rowIndex == t.RowPos)
                {
                    descriptionText.GetComponent<TextMesh>().text = FormatText(t.Description);
                    break;
                }

            }
            
        }
        else { descriptionText.GetComponent<TextMesh>().text = ""; }
    }
    public void ButtonAction() 
    {
        // show selected (or "active") sprite if inactive or "DONE" on KeyDown
        if(((button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.inactiveHover
            || button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.disabledHover)
            || (columnIndex == 0 && rowIndex == currentTree.numOfRow))
            && Input.GetKeyDown(GameControl.control.selectKey))
        {
            button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.active;
        }
                
        if (Input.GetKeyUp(GameControl.control.selectKey))
        {
            
            // check if done
            if (columnIndex == 0 && rowIndex == maxRows)
            {                
                EndScene();
            }
            // over an inactive button
            else if(button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.active)
            {
                // check for prerequisites if it has any
                bool pass = true;
                if (button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().Technique.Prerequisites != null)
                {
                    foreach (Technique t in button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().Technique.Prerequisites)
                    {
                        if (!t.Active)
                        {
                            pass = false;
                            break;
                        }
                    }
                }

                // if you have enough skill points, and you have all the prerequistites, add the technique
                if(hero.techPts >= button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().Technique.Cost && pass)
                {                    
                    AddTechnique(hero, button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().Technique);

                    // set button state to hover (normal)
                    button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;
                    button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().Technique.Active = true;
                    hero.techPts -= button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().Technique.Cost;
                    //UpdateButtons();
                }
                // change text to display failure
                // change button back to inactiveHover
                else
                {                    
                    if (!pass) { descriptionText.GetComponent<TextMesh>().text = "You don't have the proper prerequisites."; }
                    else if (hero.techPts <= button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().Technique.Cost) { descriptionText.GetComponent<TextMesh>().text = "You don't have enough points."; }
                    button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.inactiveHover;
                }
            }
        }
    
    }
    //public void UpdateButtons()
    //{        
    //    foreach(Technique t in currentTree.listOfContent)
    //    {
    //        // loop through all the nexts and add to the buttons next to keep track of states
    //        // if not already added
    //        if (t.ListNextTechnique != null)
    //        {
    //            if (t.Button.ListNextButton.Count <= 0)
    //            {
    //                foreach (Technique next in t.ListNextTechnique)
    //                {
    //                    t.Button.ListNextButton.Add(next.Button);
    //                }
    //            }

    //            // temp
    //            ButtonSkillTree curButton = t.Button;

    //            // loop through all next buttons
    //            for (int i = 0; i < t.Button.ListNextButton.Count; i++)
    //            {
    //                // temp
    //                ButtonSkillTree nextButton = t.Button.ListNextButton[i];

    //                if (t.Active && !t.ListNextTechnique[i].Active)
    //                {
    //                    nextButton.state = ButtonSkillTree.MyButtonTextureState.inactive;
    //                }
    //                // if the hero does not know the technique but can learn it (inactive), set next to disabled
    //                else if (curButton.state == ButtonSkillTree.MyButtonTextureState.inactive
    //                    || curButton.state == ButtonSkillTree.MyButtonTextureState.inactiveHover)
    //                {
    //                    nextButton.state = ButtonSkillTree.MyButtonTextureState.disabled;
    //                }
    //                // if the button is disabled, the rest should also be disabled
    //                else if (curButton.state == ButtonSkillTree.MyButtonTextureState.disabled)
    //                {
    //                    nextButton.state = ButtonSkillTree.MyButtonTextureState.disabled;
    //                }
    //            }

    //            // temp
    //            ButtonSkillTree b = t.Button;

    //            if (b.NextLine == null)
    //            {
    //                b.NextLine = new List<GameObject>();
    //                for (int i = 0; i < b.ListNextButton.Count; i++)
    //                {
    //                    b.NextLine.Add((GameObject)Instantiate(Resources.Load("Prefabs/NextLine")));
    //                    b.NextLine[i].name = "NextLine" + i.ToString();
    //                    // check if need to rotate
    //                    // rotate right
    //                    if (b.ListNextButton[i].transform.position.x > b.transform.position.x)
    //                    {
    //                        b.NextLine[i].transform.position = new Vector2(b.transform.position.x + b.width / 2, b.transform.position.y);
    //                        b.NextLine[i].transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 90.0f);
    //                    }
    //                    // rotate left
    //                    else if (b.ListNextButton[i].transform.position.x < b.transform.position.x)
    //                    {
    //                        b.NextLine[i].transform.position = new Vector2(b.transform.position.x - b.width / 2, b.transform.position.y);
    //                        b.NextLine[i].transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 90.0f);
    //                    }
    //                    // stay straight down
    //                    else
    //                    {
    //                        b.NextLine[i].transform.position = new Vector2(b.transform.position.x, b.transform.position.y - b.height / 2);
    //                    }

    //                    b.NextLine[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
    //                }
    //            }//b.NextLine = (GameObject)Instantiate(Resources.Load("Prefabs/NextLine")); }
    //             //b.NextLine.transform.position = new Vector2(b.transform.position.x, b.transform.position.y - b.height / 2);
    //             //b.NextLine.GetComponent<SpriteRenderer>().sortingOrder = 0;

    //            for (int i = 0; i < b.ListNextButton.Count; i++)
    //            {
    //                if (b.ListNextButton[i].state == ButtonSkillTree.MyButtonTextureState.inactive)
    //                {
    //                    b.NextLine[i].GetComponent<SpriteRenderer>().sprite = b.SolidLine;
    //                }
    //                else if (b.ListNextButton[i].state == ButtonSkillTree.MyButtonTextureState.disabled)
    //                {
    //                    b.NextLine[i].GetComponent<SpriteRenderer>().sprite = b.DottedLine;
    //                    //print(nextButton.labelMesh.text + " is disabled");
    //                }
    //            }
    //        }
    //    }
    //}
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
        if (levelUp == true) { UnityEngine.SceneManagement.SceneManager.LoadScene("LevelUpMenu"); }
        else { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }
    }
    public void Update()
    {        
        remainingPts.GetComponent<TextMesh>().text = "Technique Points: " + hero.techPts;

        // check for any button selections
        ButtonAction();
        // make sure space is not being held down before moving arrows
        if (!Input.GetKeyDown(GameControl.control.selectKey))
        {
            if (Input.GetKeyUp(GameControl.control.rightKey))
            {
                // not inside tree - choosing which skill tree
                if (!insideTree)
                {
                    if(columnIndex < whichContent.Count-1)
                    {
                        whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.normal;
                        columnIndex++;
                        whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;

                        // change which skill tree is shown - LATER
                        currentTree = listOfTrees[columnIndex];
                        NewTree();
                    }
                }
                else
                {
                    if (columnIndex < currentTree.numOfColumn - 1 && rowIndex != currentTree.numOfRow)
                    {                        
                        ScrollHorizontal(columnIndex + 1);
                    }
                }
            }
            if (Input.GetKeyUp(GameControl.control.leftKey))
            {
                if (!insideTree)
                {
                    if (columnIndex > 0)
                    {
                        whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.normal;
                        columnIndex--;
                        whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;

                        // change which skill tree is shown - LATER
                        currentTree = listOfTrees[columnIndex];
                        NewTree();
                    }
                }
                else
                {
                    if (columnIndex > 0 && rowIndex != currentTree.numOfRow)
                    {
                        ScrollHorizontal(columnIndex - 1);                       
                    }
                }
            }
            if (Input.GetKeyUp(GameControl.control.downKey))
            {
                if (!insideTree)
                {
                    insideTree = true;

                    // set all tree option buttons to inactive
                    foreach(GameObject g in whichButton)
                    {
                        g.GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.inactive;
                    }

                    // set chosen tree button to inactive hover
                    whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.inactiveHover;
                    whichTree = columnIndex;

                    // reset indices for skill tree
                    columnIndex = currentTree.rootCol;
                    rowIndex = currentTree.rootRow;

                    ScrollChangeButtonState();
                }
                else
                {
                    if (rowIndex < currentTree.numOfRow)
                    {
                        ScrollVertical(rowIndex + 1);
                    }
                }
            }
            if (Input.GetKeyUp(GameControl.control.upKey))
            {
                if (insideTree)
                {
                    if (rowIndex > 0)
                    {
                        // if on done button, find the next active button
                        //if (rowIndex == maxRows)
                        //{
                            ScrollVertical(rowIndex - 1);
                           // ScrollHorizontal(columnIndex);
                            
                        //}
                        //// if next button is disabled or nonexistant, don't go anywhere
                        //else 
                        //if (button2DArray[columnIndex, rowIndex - 1] == null)
                        //|| button2DArray[columnIndex, rowIndex - 1].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.disabled)
                        //{
                        //  return;
                        //}
                    }
                    else if(rowIndex == 0)
                    {
                        if (button2DArray[columnIndex,rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.inactiveHover)
                        {
                            button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.inactive;
                        }
                        // if it's disabled hover, set back to disabled
                        else if (button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state == ButtonSkillTree.MyButtonTextureState.disabledHover)
                        {
                            button2DArray[columnIndex, rowIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.disabled;
                        }
                        insideTree = false;
                        descriptionText.GetComponent<TextMesh>().text = "";
                        // set all tree option buttons to inactive
                        foreach (GameObject g in whichButton)
                        {
                            g.GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.normal;
                        }

                        // set chosen tree button to inactive hover
                        columnIndex = whichTree;
                        whichButton[columnIndex].GetComponent<ButtonSkillTree>().state = ButtonSkillTree.MyButtonTextureState.hover;
                        

                        
                        
                    }
                }
            }
        }
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

    protected void AddPrerequisites(List<Technique> content)
    {
        foreach (Technique tq in content)
        {
            if (tq != null && tq.Prerequisites != null)
            {
                tq.Description += "\n\nPrerequisites: ";
                foreach (Technique tqn in tq.Prerequisites)
                {
                    tq.Description += "\n" + tqn.Name;
                }
            }
        }
    }

    public void AddTechnique(HeroData heroToAddTo, Technique techniqueToAdd)
    {
        // check what kind of technique
        if (techniqueToAdd is Skill)
        {
            heroToAddTo.skillsList.Add((Skill)techniqueToAdd);
        }
        else if (techniqueToAdd is Spell)
        {
            heroToAddTo.spellsList.Add((Spell)techniqueToAdd);
        }
        else if (techniqueToAdd is Passive)
        {
            heroToAddTo.passiveList.Add((Passive)techniqueToAdd);
        }
        else
        {
            print("Technique not added.");
            return;
        }
    }
}


