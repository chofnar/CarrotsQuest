using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlayScroll : MonoBehaviour {

	public Sprite[] spImages;
	public Image uiImg;
    public Text tipNo;
	int ind=0;
	void Start () {
		if (spImages.Length > 0) {
			uiImg.sprite = spImages [0];
			uiImg.SetNativeSize ();
		}
	}

	public void IndexForward(){
		if (spImages.Length > 0 && ind < spImages.Length - 1) {
			ind++;
			uiImg.sprite = spImages [ind];
			uiImg.SetNativeSize ();
		} else {
			ind = 0;
			uiImg.sprite = spImages [ind];
			uiImg.SetNativeSize ();
		}
        tipNo.text = "Tip #" + (ind + 1);
	}

	public void IndexBackward(){
		if (spImages.Length > 0 && ind > 0) {
			ind--;
			uiImg.sprite = spImages [ind];
			uiImg.SetNativeSize ();
		} else {
			ind = spImages.Length - 1;
			uiImg.sprite = spImages [ind];
			uiImg.SetNativeSize ();
		}
        tipNo.text = "Tip #" + (ind + 1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
