using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingScreen : Menu
    {
        public Image blackBg;
        public Text loadingText;
        public Image jethroAnim;
        public Image coleAnim;
        public Image eleanorAnim;
        public Image joulietteAnim;
        int dotCount = 0;
        string loadingStr = "Loading";

        public override void Init()
        {
            listOfButtons = new List<Button>();
            base.Init();
        }

        public override void TurnOnMenu()
        {            
            base.TurnOnMenu();
            loadingText.text = loadingStr;
            StartCoroutine(AlternateText());
        }

        IEnumerator AlternateText()
        {
            while (true)
            {
                if (dotCount > 2)
                    dotCount = 0;
                loadingText.text = loadingStr;
                for (int i = 0; i < dotCount; i++)
                    loadingText.text += ".";
                dotCount++;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}