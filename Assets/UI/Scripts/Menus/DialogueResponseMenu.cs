namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DialogueResponseMenu : Menu
    {
        public Button templateButton;
        public GameObject parentGroup;
        List<Dialogue.Response> responses;

        protected override void AddButtons()
        {
            base.AddButtons();
            listOfButtons = new List<Button>();

            if (responses == null || responses.Count <= 0) return;

            for(int i = 0; i < responses.Count; i++)
            {
                var b = GameObject.Instantiate(templateButton, parentGroup.transform);
                b.transform.position = templateButton.transform.position + new Vector3(0, i * -50f, 0);
                listOfButtons.Add(b);
            }
            templateButton.gameObject.SetActive(false);
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            if (responses == null || responses.Count <= 0) return;
            // set response text and assign onclick listeners
            for(int i = 0; i < responses.Count; i++)
            {
                listOfButtons[i].GetComponentInChildren<Text>().text = responses[i].playerResponse;
                var temp = responses[i].conversation;
                listOfButtons[i].onClick.AddListener(() => { OnStartNextConversation(temp); });
            }
        }

        public override Button AssignRootButton()
        {
            if (listOfButtons.Count > 0)
                return listOfButtons[0];
            else return null;
        }

        public void SetResponses(List<Dialogue.Response> _responses)
        {
            responses = _responses;
            AddButtons();
            AddListeners();
            rootButton = AssignRootButton();
            SetButtonNavigation();
            TurnOnMenu();
        }

        public delegate void OnStartNextConversationDelegate(Dialogue.Conversation newConvo);
        public event OnStartNextConversationDelegate OnStartNextConversation;
        
    }
}