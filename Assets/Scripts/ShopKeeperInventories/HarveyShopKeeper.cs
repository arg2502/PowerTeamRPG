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

		typingSpeed = 0.0f;

		sceneName = "ShopKeeperInventory"; // TEMPORARY NAME

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
