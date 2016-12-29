using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopKeeper : MonoBehaviour {
	
	public List<GameObject> inventory; // inventory for each shopkeeper
	public List<Sprite> portraitImages; // reaction images from the shopkeeper
	public List<string> flavorText; // what the shopkeeper has to say about the item
	public float typingSpeed;
	public string sceneName;

}
