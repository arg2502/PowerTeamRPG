namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class tempControl : MonoBehaviour {

        public static UIManager UIManager;

        // Use this for initialization
        void Start() {
            UIManager = new UIManager();
        }

        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Q))
            {
                UIManager.PushMenu(UIManager.UIDatabase.PauseMenu);
            }
        }

    }
}