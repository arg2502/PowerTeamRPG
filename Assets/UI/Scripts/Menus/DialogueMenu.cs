namespace UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class DialogueMenu : Menu
    {
        public Text speakerText;
        public Text dialogueText;
        public Image portraitImage;
        public Button continueButton; // invisible
        public RectMask2D textMask;

        string dialogueStr; // the full string that dialogueText will print out
        float typingSpeed = 0.01f;
        Action nextDialogue; // function that occurs when Continue button is pressed, set in Dialogue.cs

        // testing out event triggers
        bool readyForNextDialogue = true;
        public bool ReadyForNextDialogue
        {
            get { return readyForNextDialogue; }
            set
            {
                readyForNextDialogue = value;

                if(readyForNextDialogue && OnNextDialogue != null)
                {
                    OnNextDialogue.Invoke();
                }
            }
        }
        //public delegate void OnNextDialogueDelegate();
        public UnityEvent OnNextDialogue;


        float currentYBoundary;
        Vector3 origTextPos;

        //bool isThereResponse = false;

        public override void TurnOnMenu()
        {
            RootButton = AssignRootButton();
            //SetSelectedObjectToRoot();
            currentYBoundary = textMask.GetComponent<RectTransform>().sizeDelta.y;
            origTextPos = dialogueText.transform.localPosition;
            base.TurnOnMenu();
        }
        public override Button AssignRootButton()
        {
            return continueButton;
        }

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>() { continueButton };
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            continueButton.onClick.RemoveAllListeners();

            if (nextDialogue != null)
                continueButton.onClick.AddListener(Continue);
        }

        public override void Close()
        {
            dialogueText.transform.localPosition = origTextPos;
            base.Close();
        }

        void Continue()
        {
            // text has finished writing by this point, go to next dialogue
            if (readyForNextDialogue)
            {
                dialogueText.transform.localPosition = origTextPos; // reset to starting pos
                nextDialogue.Invoke();
            }
            // Otherwise, the text is still typing. Press select to skip to the end of the typing
            else
            {
                EndTyping();
            }
        }

        /// <summary>
        /// Set what function occurs when the continue button is pressed. Used outside of this class.
        /// </summary>
        /// <param name="continueAction"></param>
        public void SetContinue(Action continueAction)
        {
            nextDialogue = continueAction;
            AddListeners();
        }

        /// <summary>
        /// Setting the speaker text, dialogue text, and portrait image for each part of conversation.
        /// Set in Dialogue.cs
        /// </summary>
        /// <param name="speaker"></param>
        /// <param name="dialogue"></param>
        /// <param name="portrait"></param>
        public void SetText(string speaker, string dialogue, Sprite portrait)//, bool isResponse)
        {
            speakerText.text = speaker;
            dialogueStr = dialogue;
            portraitImage.sprite = portrait;
            //isThereResponse = isResponse;
            StartCoroutine(TypeDialogue());
        }

        /// <summary>
        /// Coroutine that types out the dialogue one letter at a time based on the typing speed
        /// </summary>
        /// <returns></returns>
        IEnumerator TypeDialogue()
        {
            readyForNextDialogue = false;
            dialogueText.text = "";
            for(int i = 0; i < dialogueStr.Length; i++)
            {
                dialogueText.text += dialogueStr[i];
                CheckIfTextOutOfBounds();
                yield return new WaitForSeconds(typingSpeed);
            }
            EndTyping();
        }

        void CheckIfTextOutOfBounds()
        {
            if(dialogueText.preferredHeight > currentYBoundary)
            {           
                var yPos = dialogueText.preferredHeight - currentYBoundary;
                dialogueText.transform.localPosition += new Vector3(0, yPos);
                currentYBoundary += yPos;
            }
        }

        /// <summary>
        /// Stops typing coroutine, sets text to full text, and sets ready for next line of dialogue
        /// </summary>
        void EndTyping()
        {
            StopAllCoroutines();
            dialogueText.text = dialogueStr;
            CheckIfTextOutOfBounds();
            ReadyForNextDialogue = true; // invokes event
        }
    }
}