using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Sprite[] characters, checkbox;
    public Image sexChangeButton, checkboxObj;
    public Text highscore, carrotsNum;
    int currentSex;
    bool skipIntro;

    private void Start()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.HasKey("BoyOrGirl"))
            currentSex = PlayerPrefs.GetInt("BoyOrGirl");
        else
        {
            PlayerPrefs.SetInt("BoyOrGirl", 0);
            currentSex = 0;
        }
        sexChangeButton.sprite = characters[currentSex];

        if(!PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }

        if(!PlayerPrefs.HasKey("Carrots"))
        {
            PlayerPrefs.SetInt("Carrots", 0);
        }

        highscore.text = "Current Highscore: " + PlayerPrefs.GetInt("Highscore");
        carrotsNum.text = PlayerPrefs.GetInt("Carrots") + " Carrots Gathered";

        if(!PlayerPrefs.HasKey("SkipIntro"))
        {
            PlayerPrefs.SetInt("SkipIntro", 0);
            skipIntro = false;
        }

        if(PlayerPrefs.GetInt("SkipIntro") == 1)
        {
            checkboxObj.sprite = checkbox[1];
            skipIntro = true;
        }
        else
        {
            checkboxObj.sprite = checkbox[0];
            skipIntro = false;
        }
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("SkipIntro") == 0)
        {
            SceneManager.LoadScene("SleepinBunnies");
        }
        else
        {
            SceneManager.LoadScene("Hello");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void ChangeSex()
    {
        switch(currentSex)
        {
            case 0:
                {
                    sexChangeButton.sprite = characters[1];
                    PlayerPrefs.SetInt("BoyOrGirl", 1);
                    currentSex = 1;
                    break;
                }
            case 1:
                {
                    sexChangeButton.sprite = characters[0];
                    PlayerPrefs.SetInt("BoyOrGirl", 0);
                    currentSex = 0;
                    break;
                }
        }
    }

    public void ResetStuff()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        highscore.text = "Current Highscore: " + PlayerPrefs.GetInt("Highscore");

        PlayerPrefs.SetInt("Carrots", 0);
        carrotsNum.text = PlayerPrefs.GetInt("Carrots") + " Carrots Gathered";

        PlayerPrefs.SetInt("SkipIntro", 0);
        checkboxObj.sprite = checkbox[0];
        skipIntro = false;
    }

    public void SwitchIntroSkip()
    {
        switch (skipIntro)
        {
            case true:
                {
                    PlayerPrefs.SetInt("SkipIntro", 0);
                    checkboxObj.sprite = checkbox[0];
                    skipIntro = false;
                    break;
                }
            case false:
                {
                    PlayerPrefs.SetInt("SkipIntro", 1);
                    checkboxObj.sprite = checkbox[1];
                    skipIntro = true;
                    break;
                }
        }
    }
}
