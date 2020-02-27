using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class PauseCarousel : MonoBehaviour {

    List<GameObject> menus;
    UIManager uiManager;
    int currentIndex = -1;
    float xOffset = Screen.width;
    float xCenter = float.NaN;

    public void TurnOn(List<GameObject> _menus)
    {
        if(uiManager == null) uiManager = GameControl.UIManager;
        if (menus == null) menus = new List<GameObject>();
        
        
        for (int i = 0; i < _menus.Count; i++)
        {
            menus.Add(uiManager.PushMenu(_menus[i]));
            uiManager.CurrentMenu.inCarousel = true;
        }
        
        if (currentIndex < 0)
        {
            currentIndex = menus.Count / 2;
        }

        FocusCurrentMenu();

        gameObject.SetActive(true);
    }

    void TestPositionMenus()
    {
        if (float.IsNaN(xCenter))
            xCenter = menus[0].transform.position.x;

        for (int i = 0; i < menus.Count; i++)
        {
            float newX = xCenter + ((i - currentIndex) * xOffset);
            menus[i].transform.position = new Vector2(newX, menus[i].transform.position.y);
        }
    }

    void FocusCurrentMenu()
    {
        uiManager.SetToTop(menus[currentIndex]);
        menus[currentIndex].GetComponent<Menu>().SetSelectedObjectToRoot();
        menus[currentIndex].GetComponent<Menu>().Refocus();
        TestPositionMenus();
    }

    bool MenusInFocus()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i] == uiManager.CurrentMenu.gameObject)
                return true;
        }
        return false;

    }

    private void Update()
    {
        if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") > 0)
        {
            currentIndex++;
            if (currentIndex >= menus.Count) currentIndex = 0;
            FocusCurrentMenu();
        }
        else if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") < 0)
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = menus.Count - 1;
            FocusCurrentMenu();
        }
        if(Input.GetButtonDown("Back") || Input.GetButtonDown("Pause"))
        {
            if(MenusInFocus())
            {
                for(int i = 0; i < menus.Count; i++)
                {
                    uiManager.PopMenu();
                }
                menus.Clear();
                gameObject.SetActive(false);
            }
        }
    }
}
