namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class tempControl : MonoBehaviour {
        
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Q))
            {
                GameControl.UIManager.PushMenu(GameControl.UIManager.UIDatabase.PauseMenu);
            }
        }

    }
}