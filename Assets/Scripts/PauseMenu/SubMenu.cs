using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubMenu : Menu {

    public Transform parentPos; // Parent button -- the location for starting the subMenu
    public List<string> buttonDescription;
    public PauseMenu pm;

    // a variable to make sure there is at least one frame before input is read, else the first button in the submenu is always clicked when the submenu is enabled
    public float frameDelay = 0.0f; 

    public bool isVisible;

    public bool isActive;

    public int SelectedIndex { get { return selectedIndex; } set { selectedIndex = value; } }

	// Use this for initialization
	protected void Start () {

        // find the pause menu
        pm = GameObject.FindObjectOfType<PauseMenu>();

        base.Start();
        buttonArray = new GameObject[contentArray.Count];

        for (int i = 0; i < contentArray.Count; i++)
        {
            // create a button
            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(parentPos.position.x + b.width + 50, parentPos.position.y + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);

        }

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();

        //Change the description text
        if (pm != null) { pm.descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]); }

        DisableSubMenu(); // hide the menu until the player opens it
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

    public void EnableSubMenu()
    {
        frameDelay = 0.0f;
        isVisible = true;
        isActive = true;
        base.EnableMenu();
        if (pm != null) { pm.DeactivateMenu(); }
    }

    public void DisableSubMenu()
    {
        // reset the menu for the next use
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
        selectedIndex = 0;
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
        isVisible = false;
        isActive = false;

        //pm.isActive = true;
        //pm.EnablePauseMenu();

        base.DisableMenu();
        //descriptionText.GetComponent<Renderer>().enabled = false;
    }

    public void HighlightButton()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i != selectedIndex && !isActive) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
            else if (i != selectedIndex && isActive) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
            else if (i == selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover; }
        }
    }

	// Update is called once per frame
	protected void Update () {
        if (isVisible)
        {
            //update the position of the menu
            for (int i = 0; i < contentArray.Count; i++)
            {
                MyButton b = buttonArray[i].GetComponent<MyButton>();
                buttonArray[i].transform.position = new Vector2(parentPos.position.x + b.width + 50, parentPos.position.y + (i * -(b.height + b.height / 2)));
                b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
            }

            // Update the description
            if (pm != null) { pm.descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]); }
        }
        if(isVisible && frameDelay > 0)
        {
            base.Update();

            if (isActive)
            {
                PressButton(KeyCode.Space);

                // return to the previous menu
                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    DisableSubMenu();
                    //pm.ActivateMenu();
                }

                // unpause the game
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    DisableSubMenu();
                    if (pm != null)
                    {
                        pm.ActivateMenu();
                        GameControl.control.isPaused = false;
                        pm.DisablePauseMenu();
                    }
                }
            }
        }
        frameDelay += Time.deltaTime;
	}
}
