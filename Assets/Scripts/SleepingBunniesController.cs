using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SleepingBunniesController : MonoBehaviour {

    public SpriteRenderer black;
    public float deFadeDuration, fadeDuration, timeBetweenFades, zoomAmount, zoomDuration, cameraMoveDuration, delayBetweenZoomAndFade;
    bool loaded = false;
    public Camera thisCamera;
    public GameObject cameraPosition;
    Sprite blackImg;

	// Use this for initialization
	void Start () {
        black.DOFade(0, deFadeDuration).OnComplete(FinishedFade);
	}
	
    void FinishedFade()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        cameraPosition.transform.DOMove(gameObject.transform.position, cameraMoveDuration);
        thisCamera.DOOrthoSize(zoomAmount, zoomDuration);
        StartCoroutine(SecondDelay());
    }

    IEnumerator SecondDelay()
    {
        yield return new WaitForSecondsRealtime(delayBetweenZoomAndFade);
        black.DOFade(1, fadeDuration).OnComplete(GameLoader);
    }

    void GameLoader()
    {
        SceneManager.LoadScene("Hello");
    }

    private void Update()
    {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) && !loaded)
        {
            loaded = true;
            GameLoader();
        }
    }
}
