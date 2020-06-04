using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionNotification : MonoBehaviour {

    Transform transformToFollow;
    Vector3 yPos;
    float yOffset = 1f;
    public Image image;
    public Text message;

	public void Init(Transform newTransform, string newText)
    {
        
        transformToFollow = newTransform;
        yPos = new Vector3(0, yOffset);
        SetText(newText);
    }
    
    public void SetText(string messageText)
    {
        message.text = messageText;
    }

    private void Update()
    {
        if (transformToFollow != null)
            transform.position = (transformToFollow.position + yPos);
    }
}
