using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    //public GameObject buttonPrefab;
    protected GameObject camera;
    protected int numOfRow;
    protected List<string> contentArray;
    protected GameObject[] buttonArray;
    //protected GameObject selectedButtonObj;
    protected int selectedIndex;
    protected int scrollIndex;


	// Use this for initialization
	protected void Start () { 
        // test
        // actual version should be parameterized
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        numOfRow = 4;
        buttonArray = new GameObject[numOfRow];
        selectedIndex = 0;
        scrollIndex = 0;
        
	}
    public virtual void ButtonAction(string label) { }

    public void StateChangeText()
    {
        // reset index nums back to zero
        selectedIndex = 0;
        scrollIndex = 0;

        ChangeText();
        
    }
    protected void ChangeText()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            // set all back to normal just in case
            buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;

            // set first button to hover
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

            // if the string has less than four, leave disabled buttons
            if (i >= contentArray.Count)
            {
                buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = "";
                buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled;
            }
            // print out first four items
            else
            {
                // decrease the font size if small
                if(contentArray[i + scrollIndex].Length > 10)
                {
                    buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().fontSize = 300 - ((contentArray[i + scrollIndex].Length - 10) * 10);
                }
                else // write normally
                {
                   buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().fontSize = 300;
                }
                
                buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = contentArray[i + scrollIndex];
            }
        }
    }
	// if they are disabled, show the buttons
	public void EnableMenu()
	{
		foreach (GameObject b in buttonArray)
		{			
			if(!b.GetComponent<Renderer>().enabled){ 
				b.GetComponent<Renderer>().enabled = true;
			}
			if (!b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled) {
				b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled = true;
			}
		}
	}

	// if they are enabled, hide the buttons
	public void DisableMenu()
	{
		foreach (GameObject b in buttonArray)
		{
			if(b.GetComponent<Renderer>().enabled){ 
				b.GetComponent<Renderer>().enabled = false;
			}
			if (b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled) {
				b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled = false;
			}
		}
	}
	// Update is called once per frame
	protected void Update () {
        // scroll through menu options
        if (Input.GetKeyDown(KeyCode.S))
        {
            // move down the list of buttons
            if (selectedIndex < buttonArray.Length - 1)
            {
                // if next is disabled, stop
                if (buttonArray[selectedIndex + 1].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.disabled) {return;}
                else
                {
                    selectedIndex++;
                    buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
                    buttonArray[selectedIndex - 1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
                }
                
            }
            // scroll down to see more
            else if (selectedIndex + scrollIndex < contentArray.Count - 1)
            {
                scrollIndex++;
                ChangeText();
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
                buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
                buttonArray[selectedIndex + 1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
            }
            else if (scrollIndex > 0)
            {
                scrollIndex--;
                ChangeText();
            }
        }

        // select option
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // change button to pressed/active when keystroke down
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.active;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

            // call change state and pass in text to select button action or change the specific menu state
            ButtonAction(buttonArray[selectedIndex].GetComponent<MyButton>().labelMesh.text);
        }
	}
}