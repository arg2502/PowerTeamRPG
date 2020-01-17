using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase {

	static private List<ScriptableItem> _items;

	static private bool _isDatabaseLoaded = false;

	// Checks that the list is not null, and if it is, populate it
	static private void ValidateDatabase(){
		if (_items == null) { _items = new List<ScriptableItem>();}
		if (!_isDatabaseLoaded) { LoadDatabase();}
	}

	//Checks if database has been loaded or not, and if not, call LoadDatabaseForce()
	static public void LoadDatabase(){
		if (_isDatabaseLoaded) {
			return;
		} else {
			_isDatabaseLoaded = true;
			LoadDatabaseForce();
		}
	}

	//Makes sure that the database is loaded
	static public void LoadDatabaseForce(){
		ValidateDatabase ();
		ScriptableItem[] resources = Resources.LoadAll<ScriptableItem> (@"Items");
		foreach (ScriptableItem item in resources) {
			if(!_items.Contains(item)){
				_items.Add(item);
			}
		}
		for (int i = 0; i < resources.Length; i++) {
			Debug.Log(resources[i].name + " is in the database");
		}

	}

	//clears the database when we're done with it to free up memory
	static public void ClearDatabase(){
		_isDatabaseLoaded = false;
		_items.Clear ();
	}

    static public ScriptableItem GetItem(InventoryItem _item)
    {
        return GetItem(_item.type, _item.name);
    }

	static public ScriptableItem GetItem(string itemType, string id){
		itemType = itemType.ToLower ();
		ValidateDatabase (); //make sure database is populated before searching
		foreach (ScriptableItem item in _items) {
			if(item.name == id) {
				//make sure to return a clone of the obj to avoid editing the original
				if(itemType == "consumable"){
					return ScriptableObject.Instantiate(item) as ScriptableConsumable;
				}
				else if(itemType == "augment"){
					return ScriptableObject.Instantiate(item) as ScriptableAugment;
				}
				else if(itemType == "armor"){
					return ScriptableObject.Instantiate(item) as ScriptableArmor;
				}
				else{
					return ScriptableObject.Instantiate(item) as ScriptableKey;
				}
			}
		}

		Debug.Log("No item named " + id + " of type " + itemType + " has been found");
		return null;
	}

	static public string GetItemDescription(string id){
		ValidateDatabase (); //make sure database is populated before searching
		foreach (ScriptableItem item in _items) {
			if(item.name == id) {
				return item.description;
			}
		}
		Debug.Log("No item named " + id + " has been found");
		return null;
	}

	static public Sprite GetItemSprite(string id){
		ValidateDatabase (); //make sure database is populated before searching
		foreach (ScriptableItem item in _items) {
			if(item.name == id) {
				return item.sprite;
			}
		}
		Debug.Log("No item named " + id + " has been found");
		return null;
	}
}
