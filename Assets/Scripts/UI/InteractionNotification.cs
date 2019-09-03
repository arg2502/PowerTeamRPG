using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionNotification : MonoBehaviour {

    Transform transformToFollow;
    Vector3 yPos;
    public Image image;
    public Text message;

	public void Init(Transform newTransform, string newText)
    {
        
        transformToFollow = newTransform;
        var box = newTransform.GetComponent<BoxCollider2D>();
        yPos = new Vector3(0, box.size.y/2f);
        message.text = newText;
    }
    
    private void Update()
    {
        if (transformToFollow != null)
            transform.position = (transformToFollow.position + yPos);
    }
}
