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

    public void Start()
    {
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
                    print("break");
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
                }
                print(row);
            }
        }


    }

    public void Update()
    {
        
    }
}
