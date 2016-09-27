using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTree : MonoBehaviour {

    // 2D array of button layout
    // COLUMN MAJOR (i = column number, j = row number)
    GameObject[,] button2DArray;
    
    // button states
    // inactive = player does not have the skill, but has the possibility to acquire it
    // disabled = skill is locked off to player/has to unlock an earlier skill to make it inactive

    // List containing all of a column's technique
    // Example contentArray[0] = {"Fire Breath Lvl 1", "Fire Breath Lvl 2", etc.}
    protected List<List<Technique>> content2DArray;

    // ints to give the size of the 2D Array
    public int numOfColumn;
    public int numOfRow;

    // 2 ints to keep track of where you are in the button2DArray
    int columnIndex;
    int rowIndex;

    // access to camera 
    protected GameObject camera;

    // denigen obj to access specific techniques
    protected HeroData hero;

    // previous button object to keep track of which button to set back to normal
    MyButton prevButton;

    public void Start()
    { 
        // for positioning
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        // sizes are set in child classes
        // content2DArray is also set in child classes
        // then base.Start() is called
        button2DArray = new GameObject[numOfColumn, numOfRow];

        // set indexs to start
        columnIndex = 0;
        rowIndex = 0;

        // nest for loop through 2D array
        // whereever there is content, create a button
        for(int col = 0; col < numOfColumn; col++)
        {
            for(int row = 0; row < numOfRow; row++)
            {
                // if we've surpassed the length of the content in this column,
                // break out of loop
                
                if(row >= content2DArray[col].Count)
                {
                    break;
                }
                // otherwise, create a button
                else
                {
                    button2DArray[col,row] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
                    MyButton b = button2DArray[col,row].GetComponent<MyButton>();
                    button2DArray[col,row].transform.position = new Vector2(camera.transform.position.x - 600 + (col * (b.width + b.width/2)), camera.transform.position.y - (250 - b.height) + (row * -(b.height + b.height / 2)));

                    // display technique's name
                    b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
                    b.labelMesh = b.textObject.GetComponent<TextMesh>();
                    b.labelMesh.text = content2DArray[col][row].Name;
                    b.labelMesh.transform.position = new Vector3(button2DArray[col,row].transform.position.x, button2DArray[col,row].transform.position.y, -1);
                    
                    
                    // default buttons are inactive
                    b.state = MyButton.MyButtonTextureState.inactive;

                    // check if the denigen has already learned the technique
                    for(int i = 0; i < hero.skillsList.Count; i++)
                    {
                        if(b.labelMesh.text == hero.skillsList[i].Name)
                        {
                            b.state = MyButton.MyButtonTextureState.normal;
                        }
                    }
                    for(int i = 0; i < hero.spellsList.Count; i++)
                    {
                        if (b.labelMesh.text == hero.spellsList[i].Name)
                        {
                            b.state = MyButton.MyButtonTextureState.normal;
                        }
                    }

                }
            }
        }
        // set next if need be
        for (int col = 0; col < numOfColumn; col++)
        {
            for (int row = 0; row < numOfRow; row++)
            {
                if (row >= content2DArray[col].Count)
                {
                    break;
                }
                else if(content2DArray[col][row].Next != null && row != content2DArray[col].Count - 1)
                {
                    button2DArray[col, row].GetComponent<MyButton>().next = button2DArray[col, row + 1].GetComponent<MyButton>();
                    print("set next: " + button2DArray[col, row].GetComponent<MyButton>().next.textObject.GetComponent<TextMesh>().text);
                    // set states of the next button in branch
                    // if the hero knows the current technique but not the next one, set next to inactive
                    if(button2DArray[col,row].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.normal
                        && button2DArray[col,row].GetComponent<MyButton>().next.state != MyButton.MyButtonTextureState.normal)
                    {
                        button2DArray[col, row].GetComponent<MyButton>().next.state = MyButton.MyButtonTextureState.inactive;
                        print("set to inactive: " + button2DArray[col, row].GetComponent<MyButton>().next.textObject.GetComponent<TextMesh>().text);
                    }
                    // if the hero does not know the technique but can learn it (inactive), set next to disabled
                    else if (button2DArray[col,row].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive)
                    {
                        button2DArray[col, row].GetComponent<MyButton>().next.state = MyButton.MyButtonTextureState.disabled;
                        print("set to disabled: " + button2DArray[col, row].GetComponent<MyButton>().next.textObject.GetComponent<TextMesh>().text);
                    }
                }
                //else { print("Else: " + content2DArray[col][row].Next.Name); }
            }
        }

        

        // set selected index to correct state
        if(button2DArray[columnIndex,rowIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.normal)
        {
            button2DArray[columnIndex, rowIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
        }
        else if(button2DArray[columnIndex, rowIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive)
        {
            button2DArray[columnIndex, rowIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
        }

        // set previous button to the current button
        prevButton = button2DArray[columnIndex, rowIndex].GetComponent<MyButton>();

    }
    void ScrollHorizontal(int col)
    {
        for (int row = rowIndex - 1; row >= 0; row--)
        {
            // if the button exists, and it's either normal or inactive, set to that button
            if (button2DArray[col, row] != null
                && (button2DArray[col, row].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.normal
                || button2DArray[col, row].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive))
            {
                rowIndex = row;
                break;
            }
        }
    }
    void ScrollChangeButtonState()
    {
        // check if the button is inactive
        // if it is, set it to inactiveHover
        if (button2DArray[columnIndex, rowIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive)
        {
            button2DArray[columnIndex, rowIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
        }
        // if it is not inactive, set it to normal hover
        else
        {
            button2DArray[columnIndex, rowIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
        }

        // now change the previous button state
        // if the last button was inactiveHover, set it to inactive
        if (prevButton.state == MyButton.MyButtonTextureState.inactiveHover)
        {
            prevButton.state = MyButton.MyButtonTextureState.inactive;
        }
        // if it was a normal button, set it back to normal
        else
        {
            prevButton.state = MyButton.MyButtonTextureState.normal;
        }

        // set previous button to new current button
        prevButton = button2DArray[columnIndex, rowIndex].GetComponent<MyButton>();
    }
    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.D))
        {
            if(columnIndex < numOfColumn - 1)
            {
                // if the next button is disabled or doesn't exist, find the next lowest button in the column
                if (button2DArray[columnIndex + 1, rowIndex] == null
                    || button2DArray[columnIndex + 1, rowIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.disabled) 
                {
                    ScrollHorizontal(columnIndex + 1);
                }
                // increase index to hover over next button
                columnIndex++;
                ScrollChangeButtonState();
                    
            }
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            if (columnIndex > 0)
            {
                // if the next button is disabled or doesn't exist, find the next lowest button in the column
                if (button2DArray[columnIndex - 1, rowIndex] == null
                    || button2DArray[columnIndex - 1, rowIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.disabled)
                {
                    ScrollHorizontal(columnIndex - 1);
                }
                // increase index to hover over next button
                columnIndex--;
                ScrollChangeButtonState();

            }
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            if(rowIndex < numOfRow - 1)
            {
                // if next button is disabled or nonexistant, don't go anywhere
                if(button2DArray[columnIndex, rowIndex + 1] == null
                    || button2DArray[columnIndex, rowIndex + 1].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.disabled)
                {
                    return;
                }
                rowIndex++;
                ScrollChangeButtonState();
            }
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            if (rowIndex > 0)
            {
                // if next button is disabled or nonexistant, don't go anywhere
                if (button2DArray[columnIndex, rowIndex - 1] == null
                    || button2DArray[columnIndex, rowIndex - 1].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.disabled)
                {
                    return;
                }
                rowIndex--;
                ScrollChangeButtonState();
            }
        }
    }
}
