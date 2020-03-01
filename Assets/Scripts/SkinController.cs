using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour {


	public string spriteSheetName;
    Sprite[] subSprites;
    SpriteRenderer bunnyRenderer;

	void Start () {
        int choice = PlayerPrefs.GetInt("BoyOrGirl");
		switch (choice) {
		case 0:
			spriteSheetName = "RedBunny";
			break;
		case 1:
			spriteSheetName = "PurpleBunny";
			break;
		}
        subSprites = Resources.LoadAll<Sprite>("Skins/" + spriteSheetName);
        bunnyRenderer = GetComponent<SpriteRenderer>();
    }

	void LateUpdate () {
		string sprName = bunnyRenderer.sprite.name;
		var newSprite = Array.Find (subSprites, item => item.name == sprName);
        if (newSprite)
        {
            bunnyRenderer.sprite = newSprite;
        }
	}
}
