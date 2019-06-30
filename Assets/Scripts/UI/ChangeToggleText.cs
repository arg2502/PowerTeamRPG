using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeToggleText : MonoBehaviour {

    Toggle toggle;
    Text text;
	
    void Awake()
    {
        toggle = GetComponent<Toggle>();
        text = toggle.GetComponentInChildren<Text>();
        toggle.onValueChanged.AddListener((isOn) => { Bold(isOn); });
    }
    

    void WhiteOrBlack(bool isOn)
    {
        if(isOn)
        {
            text.color = Color.white;
        }
        else
        {
            text.color = Color.black;
        }
    }

    void Bold(bool isOn)
    {
        if(isOn)
        {
            text.fontStyle = FontStyle.Bold;
        }
        else
        {
            text.fontStyle = FontStyle.Normal;
        }
    }
}
