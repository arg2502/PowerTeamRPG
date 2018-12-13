namespace UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class DialogueResponseMenu : Menu
    {
        float distanceBetweenButtons = -50f;
        public Button templateButton;
        public GameObject parentGroup;
        List<Dialogue.Response> responses;

        public ConversationEvent OnStartNextConversation;

        public override void Init()
        {
            if(OnStartNextConversation == null)
            OnStartNextConversation = new ConversationEvent();
            base.Init();

        }

        protected override void AddButtons()
        {
            base.AddButtons();
            if (listOfButtons == null)
                listOfButtons = new List<Button>();

            if (responses == null) return;

            for(int i = listOfButtons.Count; i < responses.Count; i++)
            {
                var b = GameObject.Instantiate(templateButton, parentGroup.transform);
                b.transform.localPosition = templateButton.transform.localPosition + new Vector3(0, i * distanceBetweenButtons, 0);
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
                listOfButtons[i].onClick.RemoveAllListeners();                
                listOfButtons[i].onClick.AddListener(() => { OnStartNextConversation.Invoke(temp); });
                listOfButtons[i].onClick.AddListener(GameControl.UIManager.PopMenu);
            }
        }

        public override Button AssignRootButton()
        {
            if (listOfButtons.Count > 0)
                return listOfButtons[0];
            else return null;
        }

        public override void TurnOnMenu()
        {
            base.TurnOnMenu();

        }

        public override void Close()
        {
            base.Close();

            OnStartNextConversation.RemoveAllListeners();
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

        
    }

    public class ConversationEvent : UnityEvent<Dialogue.Conversation> { }
}