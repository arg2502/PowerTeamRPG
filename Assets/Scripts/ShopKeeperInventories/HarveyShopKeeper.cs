using UnityEngine;
using System.Collections;

public class HarveyShopKeeper : ShopKeeper {

	Sprite image;

	// Use this for initialization
	void Start () {
		image = (Resources.Load ("Sprites/tempGoikko", typeof(Sprite)) as Sprite);
		inventory.Add((GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserRestorative")));
		inventory.Add((GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserElixir")));
		inventory.Add((GameObject)Instantiate(Resources.Load("Prefabs/Items/HelmetOfFortitude")));


		portraitImages.Add(image);
		portraitImages.Add(image);
		portraitImages.Add(image);

		flavorText.Add ("that's a ta lesser restorative");
		flavorText.Add ("Good on ya, lesser elixer");
		flavorText.Add ("That just looks stupid.");

		//howMuchText = "How much you want?";
		//tooMuchText = "Whoa, you can't afford any more than that.";
		//confirmationText = "Are ya sure you wanna buy that?";
		//receiptText = "Great choice!";

		howMuchBuying = "How much you want?";
		tooMuchBuying = "Whoa, you can't afford any more than that.";
		confirmationBuying = "Are ya sure you wanna buy that?";
		receiptBuying = "Great choice!";

		howMuchSelling = "How much you wanna sell?";
		tooMuchSelling = "Looks like that's all you got.";
		confirmationSelling = "Are ya sure you wanna sell that?";
		receiptSelling = "Thanks for that!";

		sellingText = "What do ya got for me?";

		typingSpeed = 0.0f;

		sceneName = "ShopKeeperInventory"; // TEMPORARY NAME

		consumablesPerc = 0.9f;
		weaponsPerc = 0.6f;
		equipPerc = 0.4f;
		reusePerc = 0.75f;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
