using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : Menu {

    protected List<string> buttonDescription;
    protected GameObject descriptionText;

    public bool isVisible; // variable to hide pause menu
    protected characterControl player;

	// Use this for initialization
	void Start () {

        // find the player
        player = GameObject.FindObjectOfType<characterControl>();

        base.Start();
        buttonArray = new GameObject[contentArray.Count];
        buttonDescription = new List<string>();

        buttonDescription.Add("View your team's stats, status ailments, and assign them equipment.");
        buttonDescription.Add("Access your entire inventory.");
        buttonDescription.Add("Save your game.");
        buttonDescription.Add("Load a previously saved game.");
        buttonDescription.Add("Return to the game.");

        for (int i = 0; i < contentArray.Count; i++)
        {
            // create a button
            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(player.transform.position.x - 600, player.transform.position.y + (250 + b.height) + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);

        }

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        //Create the description text object
        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]);
        descriptionText.transform.position = new Vector2(player.transform.position.x + 200, buttonArray[0].transform.position.y + 15);

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();

        DisablePauseMenu(); // hide the menu until the player opens it
        isVisible = false;
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
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Team Info":
                break;
            case "Inventory":
                break;
            case "Save":
                // set the current scene variable
                GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                GameControl.control.RecordRoom();
                //current position is at the entrance, unless a savestatue is tagged
                if (GameControl.control.taggedStatue == false)
                {
                    GameControl.control.currentPosition = GameControl.control.areaEntrance;
                }
                // Save the game
                GameControl.control.Save();
                break;
            case "Load":
                //Maybe at some point there will be multiple saves, but for now, there's just one
                GameControl.control.Load();
                break;
            case "Exit Menu":
                DisablePauseMenu();
                break;
        }
    }

    public void EnablePauseMenu()
    {
        base.EnableMenu();
        descriptionText.GetComponent<Renderer>().enabled = true;
    }

    public void DisablePauseMenu()
    {
        // reset the menu for the next use
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
        selectedIndex = 0;
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        // hide the menu
        isVisible = false;
        //DisablePauseMenu();

        // release the overworld objects
        if (player.canMove == false) { player.ToggleMovement(); }
        
        base.DisableMenu();
        descriptionText.GetComponent<Renderer>().enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if (isVisible)
        {
            base.Update();

            //update the position of the menu
            for (int i = 0; i < contentArray.Count; i++)
            {
                MyButton b = buttonArray[i].GetComponent<MyButton>();
                buttonArray[i].transform.position = new Vector2(player.transform.position.x - 600, player.transform.position.y + (250 + b.height) + (i * -(b.height + b.height / 2)));
                b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
            }
            descriptionText.transform.position = new Vector2(player.transform.position.x + 200, buttonArray[0].transform.position.y + 15);

            // Update the description
            descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]);

            PressButton(KeyCode.Space);
        }
	}
}
