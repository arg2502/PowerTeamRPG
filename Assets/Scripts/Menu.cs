﻿//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class Menu : MonoBehaviour {

//    //public GameObject buttonPrefab;
//    protected GameObject camera;
//    protected int numOfRow;
//    public List<string> contentArray;
//    protected GameObject[] buttonArray;
//    //protected GameObject selectedButtonObj;
//    protected int selectedIndex;
//    protected int scrollIndex;

//	// sub menu attributes
//	public bool isActive;
//	public GameObject descriptionText;
//	public bool isVisible; // variable to hide pause menu
//	public characterControl player;

//	// Use this for initialization
//	protected void Start () { 
//        // test
//        // actual version should be parameterized
//        camera = GameObject.FindGameObjectWithTag("MainCamera");
//        //numOfRow = 4;
//        buttonArray = new GameObject[numOfRow];
//        selectedIndex = 0;
//        scrollIndex = 0;
//		isActive = false;
//	}
//    public virtual void ButtonAction(string label) { }
//	public void DeactivateMenu()
//	{
//		isActive = false;
//		for(int i = 0; i < buttonArray.Length; i++)
//		{			
//			if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
//		}
//	}
//	public void ActivateMenu()
//	{
//		isActive = true;
//		for (int i = 0; i < buttonArray.Length; i++) {
//			if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
//		}

//	}
//    public void StateChangeText()
//    {
//        // reset index nums back to zero
//        selectedIndex = 0;
//        scrollIndex = 0;

//		// set all back to normal so a random button in the list won't be highlighted
//		for (int i = 0; i < buttonArray.Length; i++) {
//			buttonArray [i].GetComponent<MyButton> ().state = MyButton.MyButtonTextureState.normal;
//		}

//		// set first button to hover
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

//        ChangeText();
        
//    }
//    protected void ChangeText()
//    {
//        for (int i = 0; i < buttonArray.Length; i++)
//        {
            

//            // if the string has less than four, leave disabled buttons
//            if (i >= contentArray.Count)
//            {
//                buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = "";
//                buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled;
//            }
//            // print out first four items
//            else
//            {
//                // decrease the font size if small
//                if(contentArray[i + scrollIndex].Length > 10)
//                {
//                    buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().fontSize = (500- ((contentArray[i + scrollIndex].Length - 10) * 10))/5;
//                }
//                else // write normally
//                {
//                   buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().fontSize = 500/5;
//                }
                
//                buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text = contentArray[i + scrollIndex];
//            }
//        }
//    }
//	// if they are disabled, show the buttons
//	public void EnableMenu()
//	{
//		foreach (GameObject b in buttonArray)
//		{			
//			if(!b.GetComponent<Renderer>().enabled){ 
//				b.GetComponent<Renderer>().enabled = true;
//			}
//			if (!b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled) {
//				b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled = true;
//			}
//		}
//	}

//	// if they are enabled, hide the buttons
//	public void DisableMenu()
//	{
//		foreach (GameObject b in buttonArray)
//		{
//			if(b.GetComponent<Renderer>().enabled){ 
//				b.GetComponent<Renderer>().enabled = false;
//			}
//			if (b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled) {
//				b.GetComponent<MyButton> ().textObject.GetComponent<Renderer> ().enabled = false;
//			}
//		}
//	}
//	public void DisableSubMenu()
//	{
//		// reset the menu for the next use
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
//		selectedIndex = 0;
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
//		isVisible = false;
//		isActive = false;

//		//pm.isActive = true;
//		//pm.EnablePauseMenu();

//		DisableMenu ();
//		//base.DisableMenu();
//		//descriptionText.GetComponent<Renderer>().enabled = false;
//	}
//	public void DisablePauseMenu()
//	{
//		// reset the menu for the next use
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
//		selectedIndex = 0;
//		buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

//		// hide the menu
//		isVisible = false;
//		//GameControl.control.isPaused = false;
//		GetComponent<SpriteRenderer>().enabled = false;

//		// release the overworld objects
//		if (player.canMove == false) { player.ToggleMovement(); }

//		//base.DisableMenu();
//		DisableMenu();
//		descriptionText.GetComponent<Renderer>().enabled = false;
//	}
//	// Update is called once per frame
//	protected void Update () {
       
//            // scroll through menu options
//            if (Input.GetKeyDown(GameControl.control.downKey))
//            {
//                // move down the list of buttons
//                if (selectedIndex < buttonArray.Length - 1)
//                {
//                    // if next is disabled, stop
//                    if (buttonArray[selectedIndex + 1].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.disabled) { return; }
//                    else
//                    {
//                        // increase index to hover over next button
//                        selectedIndex++;

//                        // check if the button is inactive
//                        // if it is, set it to inactiveHover
//                        if (buttonArray[selectedIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive)
//                        {
//                            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
//                        }
//                        // if it is not inactive, set it to normal hover
//                        else
//                        {
//                            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
//                        }

//                        // now change the previous button state
//                        // if the last button was inactiveHover, set it to inactive
//                        if (buttonArray[selectedIndex - 1].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactiveHover)
//                        {
//                            buttonArray[selectedIndex - 1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
//                        }
//                        // if it was a normal button, set it back to normal
//                        else
//                        {
//                            buttonArray[selectedIndex - 1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
//                        }
//                    }

//                }
//                // scroll down to see more
//                else if (selectedIndex + scrollIndex < contentArray.Count - 1)
//                {
//                    scrollIndex++;
//                    ChangeText();
//                }
//            }
//            else if (Input.GetKeyDown(GameControl.control.upKey))
//            {
//                if (selectedIndex > 0)
//                {
//                    selectedIndex--;
//                    // check if the button is inactive
//                    // if it is, set it to inactiveHover
//                    if (buttonArray[selectedIndex].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactive)
//                    {
//                        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
//                    }
//                    // if it is not inactive, set it to normal hover
//                    else
//                    {
//                        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
//                    }

//                    // now change the previous button state
//                    // if the last button was inactiveHover, set it to inactive
//                    if (buttonArray[selectedIndex + 1].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.inactiveHover)
//                    {
//                        buttonArray[selectedIndex + 1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
//                    }
//                    // if it was a normal button, set it back to normal
//                    else
//                    {
//                        buttonArray[selectedIndex + 1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal;
//                    }
//                }
//                else if (scrollIndex > 0)
//                {
//                    scrollIndex--;
//                    ChangeText();
//                }
//            }
//            // sort buttons to front of screen
//            for (int i = 0; i < buttonArray.Length; i++)
//            {
//                buttonArray[i].GetComponent<MyButton>().sr.sortingOrder = 9900;
//                buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<Renderer>().sortingOrder = 9900;
//            }
//	}

//    protected void PressButton(KeyCode button)
//    {
//        // select option if active
//        if (buttonArray[selectedIndex].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive
//            && buttonArray[selectedIndex].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover)
//        {
//            if (Input.GetKeyDown(button))
//            {
//                // change button to pressed/active when keystroke down
//                buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.active;
//            }
//            else if (Input.GetKeyUp(button))
//            {
//                buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

//                // call change state and pass in text to select button action or change the specific menu state
//                ButtonAction(buttonArray[selectedIndex].GetComponent<MyButton>().labelMesh.text);
//            }
//        }
//    }
//}