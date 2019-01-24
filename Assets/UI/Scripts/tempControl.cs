namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class tempControl : MonoBehaviour {
        
        void Update()
        {
            if(Input.GetButton("Pause"))
            {
                GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.PauseMenu);
            }
        }

    }
}