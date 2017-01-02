using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopKeeper : MonoBehaviour {

	public float typingSpeed;
	public string sceneName;

	// buying menu
	public List<GameObject> inventory; // inventory for each shopkeeper
	public List<Sprite> portraitImages; // reaction images from the shopkeeper
	public List<string> flavorText; // what the shopkeeper has to say about the item
	public string howMuchText; // asking "how much?"
	public string tooMuchText; // saying you can't buy anymore
	public string confirmationText; // make sure you really wanna buy it
	public string receiptText; // text after purchase

	// buying text
	public string howMuchBuying;
	public string tooMuchBuying;
	public string confirmationBuying;
	public string receiptBuying;

	// selling text
	public string sellingText; // standard selling menu text
	public string howMuchSelling; // asking "how much?"
	public string tooMuchSelling; // can't sell more than that
	public string confirmationSelling; // make sure you wanna sell it
	public string receiptSelling; // text after selling


	// selling percentages
	// to determine how well or how poorly a shopkeeper will pay for a certain item
	// should never be greater than 1
	public float consumablesPerc;
	public float weaponsPerc;
	public float equipPerc;
	public float reusePerc;

}
