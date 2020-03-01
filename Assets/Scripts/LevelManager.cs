using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour {

    public Color[] levelSliderColors;
    public ParticleSystem[] sparks;
    public Vector3 goUpLevelLocation;
    public float[] levelMultipliers, blackAlphas, rainbowAlphas, halfCollider;
    public float goUpLevelDuration, upPlatformYOffset, upPlatformXOffset, minValX, maxValX, minValY, maxValY, fallDistance, spikeChance, goldenCarrotChance, scorePerDistance, rainbowFadeSpeed, blackFadeSpeed, pulseVisibleAlpha, pulseDuration, sliderTransitionTime, progressTransitionTime, spikeAdjustY, spikeAdjustX, cloudMoveSpeed;
    public int carrotScoreValue, goldenCarrotScoreValue, goldenCarrotCarrotsValue, startLevel;
    public SpriteRenderer black, rainbow;
    public Image pulse, levelSliderFiller, counter, highscore, soundImage;
    public Sprite[] countdown, soundSprites;
    public WallCheck wallChecker;
    public GameObject goldenCarrot, carrot, player, spikeObject, objectMoveDestination, endGamePlat, wings, halo, xCarrots;
    GameObject newPlatform, newSpike;
    public GameObject[] detailDatabase, platformDatabase, clouds;
    public Transform lastPlatformPosition;
    public CanvasGroup pauseMenu, lossMenu, preLoseMenu;
    public Slider slider, levelSlider;
    float lastPlatformXOffset, fallDownDupe, x, advanceScore, advanceScoreUnchanged, carrotRandom, scoreAmount = 0, spikeAdjustYDuplicate, spikeAdjustXDuplicate, timerStartTime, timeLeft, startY, startZ;
    int indicePlatforma, carrotNumber = 0, randomIncrement, carrotRandomIncrement, gameState = 1, currentLevel = 6, lastX = 0, carrotLocation, soundState, buyLifeAmount = 50, buyLifeTimes = 1, indiceUltimaPlatforma, auxCarrot, doubleCarrot = 1;
    public Text score, carrots, buyLifeNum, noHighscore, currentTotalCarrots;
    Platform ultimaPlatforma;
    PlayerController controller;
    Collectible thisCollectible;
    bool lost = false, isInLossMenu = false, canSpawnSpike, spawnedSpike = false, isLosePlatform, immune = false, updatedLevel = true, isInPreLoseMenu = false, flying = false;
    public bool canLose;
    public Button buyLifeButton;
    float[] startX, finalX;
    Animator playerAnimations;
    Rigidbody2D playerRB;
    [HideInInspector]
    public float scoreMultiplier = 1;

	void Start () {
        startX = new float[2];
        finalX = new float[2];
        startY = clouds[0].transform.position.y;
        startZ = clouds[0].transform.position.z;
        for (int i = 0; i < 2; i++)
        {
            startX[i] = clouds[i].transform.localPosition.x;
        }
        finalX[0] = -startX[1];
        finalX[1] = startX[0];

        controller = player.GetComponent<PlayerController>();
        levelSliderFiller.color = levelSliderColors[startLevel];
        levelSlider.value = 120 - currentLevel * 10;
        ColorConverter();
        currentLevel = startLevel;
        ResetLevelScore();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerAnimations = player.GetComponent<Animator>();
        buyLifeNum.text = "Buy a life: " + buyLifeAmount + " carrots";
        soundState = PlayerPrefs.GetInt("Sound",1);
        if(soundState == 1)
        {
            AudioListener.volume = 1;
            soundImage.sprite = soundSprites[0];
        }
        else
        {
            AudioListener.volume = 0;
            soundImage.sprite = soundSprites[1];
        }
        Time.timeScale = 1;
        if (GameObject.Find("RestartedMusic") != null)
            Destroy(GameObject.Find("Music"));

        indiceUltimaPlatforma = 0;

        MoveCloudOne();
        MoveCloudTwo();
	}
	
	// Update is called once per frame
	void Update () {
        //level update
        if(!updatedLevel)
        {
            levelSlider.DOValue(120 - currentLevel * 10, sliderTransitionTime);
            levelSliderFiller.DOColor(levelSliderColors[currentLevel], sliderTransitionTime);
            updatedLevel = true;
        }

        //score update
        if((int) player.transform.position.x > lastX)
        {
            lastX = (int)player.transform.position.x; //---------------------------------------------------------------------
            scoreAmount = scoreAmount + scorePerDistance * scoreMultiplier;
            advanceScore -= scorePerDistance * scoreMultiplier;
        }
        score.text = scoreMultiplier + "x Score: " + (int)scoreAmount;

        //score to advance update
        if (advanceScoreUnchanged != 0)
        {
            slider.DOValue(100 - (advanceScore / advanceScoreUnchanged) * 100, progressTransitionTime);
            //slider.value = 100 - (advanceScore / advanceScoreUnchanged) * 100;
        }
        else
        {
            slider.value = 100;
        }

        if (advanceScore <= 0 && currentLevel != 1)
        {
            GoUpLevel();
        }

        //generate platforms
        x = lastPlatformPosition.transform.position.x - player.transform.position.x ;
        if(x < 15)
        {
            GeneratePlatform(false, true, false);

        }
        if(isInPreLoseMenu)
        {
            timeLeft = 10 - (int)(timerStartTime - Time.time) * (-1);
            if ((int)timeLeft > -1)
            {
                counter.sprite = countdown[10 - (int)(timerStartTime - Time.time) * (-1)];
            }
            else
            {
                Nope();
            }
        }
	}

    public void AddCollectible(int type)
    {
        switch (type)
        {
            case 0:
                {
                    carrotNumber = carrotNumber + doubleCarrot;
                    carrots.text = "" + carrotNumber; //-------------------------------------------------
                    scoreAmount += carrotScoreValue * scoreMultiplier * doubleCarrot;
                    advanceScore -= carrotScoreValue * scoreMultiplier * doubleCarrot;
                    break;
                }
            case 1:
                {
                    scoreAmount += goldenCarrotScoreValue * scoreMultiplier;
                    carrotNumber += goldenCarrotCarrotsValue * doubleCarrot;
                    carrots.text = "" + carrotNumber;
                    if (currentLevel != 1 && !flying)
                    {
                        flying = true;
                        GoUpLevel();
                    }
                    break;
                }
        }
    }

    public void PauseGame()
    {
        if (!isInLossMenu)
        {
            switch (gameState)
            {
                case 1:
                    {
                        gameState = 0;
                        Time.timeScale = 0;
                        pauseMenu.interactable = true;
                        pauseMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);
                        playerAnimations.updateMode = AnimatorUpdateMode.Normal;
                        controller.isInPauseMenu = true;
                        break;
                    }
                case 0:
                    {
                        gameState = 1;
                        Time.timeScale = 1;
                        pauseMenu.interactable = false;
                        pauseMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5000, -40);
                        playerAnimations.updateMode = AnimatorUpdateMode.UnscaledTime;
                        controller.isInPauseMenu = false;
                        break;
                    }
            }
        }
    }

    public void Restart()
    {
        if (GameObject.Find("RestartedMusic") == null)
        {
            DontDestroyOnLoad(GameObject.Find("Music"));
            GameObject.Find("Music").name = "RestartedMusic";
        }
        SceneManager.LoadScene("Hello");
    }

    public void GoToMenu()
    {
        if (GameObject.Find("RestartedMusic") != null)
            Destroy(GameObject.Find("RestartedMusic"));

        if (PlayerPrefs.GetInt("Highscore") < (int)scoreAmount)
            PlayerPrefs.SetInt("Highscore", (int)scoreAmount);

        PlayerPrefs.SetInt("Carrots", PlayerPrefs.GetInt("Carrots") + carrotNumber);

        SceneManager.LoadScene("MainMenu");
    }


    public void GoDownLevel()
    {
        if (canLose && !lost)
        {
            xCarrots.SetActive(false);
            doubleCarrot = 1;
            currentLevel += 1;
            scoreMultiplier = levelMultipliers[currentLevel];
            GeneratePlatform(true, false, false);
            ResetLevelScore();
            black.DOFade(blackAlphas[currentLevel], blackFadeSpeed);
            rainbow.DOFade(rainbowAlphas[currentLevel], rainbowFadeSpeed);
            updatedLevel = false;
        }
    }

    void GeneratePlatform(bool down, bool canSpawnGolden, bool up)
    {
        indicePlatforma = (int)Random.Range(0, platformDatabase.Length);
        if (!up)
        {
            if (!down)
            {
                if (spawnedSpike)
                {
                    spikeAdjustYDuplicate = spikeAdjustY;
                    spikeAdjustXDuplicate = spikeAdjustX;
                }
                else
                {
                    spikeAdjustYDuplicate = 0;
                    spikeAdjustXDuplicate = 0;
                }

                canSpawnSpike = true; //---------------------------------------------------------------------------------------------------------------------
                newPlatform = Instantiate(platformDatabase[indicePlatforma], new Vector3(lastPlatformPosition.position.x + Random.Range(minValX, maxValX - spikeAdjustXDuplicate) + halfCollider[indicePlatforma] + halfCollider[indiceUltimaPlatforma] + 1.5f, lastPlatformPosition.position.y + Random.Range(minValY, maxValY - spikeAdjustYDuplicate), 0), Quaternion.identity);
                indiceUltimaPlatforma = indicePlatforma;
            }
            else
            {
                if (currentLevel != 12)
                {
                    newPlatform = Instantiate(platformDatabase[0], new Vector3(player.transform.position.x + 1, player.transform.position.y - fallDistance, 0), Quaternion.identity);
                    indiceUltimaPlatforma = 0;
                    canSpawnSpike = false;
                }
                else
                {
                    newPlatform = Instantiate(endGamePlat, new Vector3(player.transform.position.x + 1, player.transform.position.y - fallDistance, 0), Quaternion.identity);
                    canSpawnSpike = false;
                    indiceUltimaPlatforma = 0;
                }
            }
        }
        else
        {
            canSpawnSpike = false;
            newPlatform = Instantiate(platformDatabase[0], new Vector3(player.transform.position.x + goUpLevelLocation.x + upPlatformXOffset, player.transform.position.y + goUpLevelLocation.y + upPlatformYOffset, 0), Quaternion.identity);
            indiceUltimaPlatforma = 0;
        }

        lastPlatformPosition = newPlatform.transform;

        //tepi --------------------------------------------------------------------------------------------------------------------------
        spawnedSpike = false;
        if ((Random.Range(1, 100) < spikeChance) && indicePlatforma == 0 && canSpawnSpike)
        {
            newSpike = Instantiate(spikeObject, newPlatform.GetComponent<Platform>().detailsLocations[2]);
            if(Random.Range(0,100)%2==0)
            {
                newSpike.transform.eulerAngles = new Vector3(0, 180, 0);
            }

            spawnedSpike = true;
        }

        //detaliile
        ultimaPlatforma = newPlatform.GetComponent<Platform>();
        randomIncrement = 10;
        for (int i = 0; i < ultimaPlatforma.detailsLocations.Length; i++)
        {
            if (Random.Range(1, 100) > 40 + randomIncrement)
            {
                if (i != 2 || !spawnedSpike)
                {
                    Instantiate(detailDatabase[(int)Random.Range(0, detailDatabase.Length)], ultimaPlatforma.detailsLocations[i]);
                    randomIncrement -= 10;
                }
            }
            else
            {
                randomIncrement += 10;
            }
        }

        //morcovi && morcoviori aurii
        carrotRandom = goldenCarrotChance;
        if (canSpawnGolden)
        {
            for (int i = 0; i < ultimaPlatforma.coinLocations.Length; i++)
            {
                if (Random.Range(0, 100) < carrotRandom)
                {
                    if (i != 2 || !spawnedSpike)
                    {
                        thisCollectible = Instantiate(goldenCarrot, ultimaPlatforma.coinLocations[i]).GetComponent<Collectible>();
                        thisCollectible.manager = gameObject.GetComponent<LevelManager>();
                        thisCollectible.destination = objectMoveDestination;
                        carrotLocation = i;
                        break;
                    }
                }
            }
        }
        carrotRandomIncrement = 10;

        for (int i = 0; i < ultimaPlatforma.coinLocations.Length; i++)
        {
            if (Random.Range(1, 100) > 40 + carrotRandomIncrement && i != carrotLocation)
            {
                if (i != 2 || !spawnedSpike)
                {
                    thisCollectible = Instantiate(carrot, ultimaPlatforma.coinLocations[i]).GetComponent<Collectible>();
                    thisCollectible.manager = gameObject.GetComponent<LevelManager>();
                    thisCollectible.destination = objectMoveDestination;
                    carrotRandomIncrement += 10;
                }
            }
            else
            {
                carrotRandomIncrement -= 5;
                thisCollectible = null;
            }
        }

        carrotLocation = -1;

    }

    IEnumerator LossDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canLose = true;
    }

    public void ResetLevelScore()
    {
        //advanceScore = (int)Mathf.Pow(currentLevel, 4) + 300 ; //advance level score ---------------------------------------------------------------------------
        advanceScore = (130 - currentLevel * 10) * 8 * levelMultipliers[currentLevel];
        advanceScoreUnchanged = advanceScore;
    }

    public void SoundSwitch()
    {
        switch (soundState)
        {
            case 1:
                {
                    soundState = 0;
                    PlayerPrefs.SetInt("Sound", 0);
                    AudioListener.volume = 0;
                    soundImage.sprite = soundSprites[1];
                    break;
                }
            case 0:
                {
                    soundState = 1;
                    PlayerPrefs.SetInt("Sound", 1);
                    AudioListener.volume = 1;
                    soundImage.sprite = soundSprites[0];
                    break;
                }
        }
    }

    void ZeroY()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
		wings.SetActive (false);
        for (int i = 0; i < 2; i++)
        {
            ParticleSystem.EmissionModule em = sparks[i].emission;
            em.enabled = false;
        }
        wallChecker.isFlying = false;
        halo.SetActive(false);
        flying = false;
    }

    IEnumerator ImmuneDelay(float howMuch)
    {
        yield return new WaitForSecondsRealtime(howMuch);
        immune = false;
    }

    void GoUpLevel()
    {
        GeneratePlatform(false, false, true);
		wings.SetActive (true);
        for(int i=0; i<2; i++)
        {
            ParticleSystem.EmissionModule em = sparks[i].emission;
            em.enabled = true;
        }

		playerRB.DOMove(player.transform.position + goUpLevelLocation, goUpLevelDuration).OnComplete(ZeroY);
        currentLevel -= 1;
        if (currentLevel == 1)
        {
            xCarrots.SetActive(true);
            doubleCarrot = 2;
        }

        updatedLevel = false;
        if (currentLevel != 1)
        {
            ResetLevelScore();
        }
        else
        {
            advanceScore = 0;
        }
        if (currentLevel < startLevel)
        {
            Pulsate();
        }
        immune = true;
        StartCoroutine(ImmuneDelay(1f));
        scoreMultiplier = levelMultipliers[currentLevel];
        black.DOFade(blackAlphas[currentLevel], blackFadeSpeed);
        rainbow.DOFade(rainbowAlphas[currentLevel], rainbowFadeSpeed);
        wallChecker.isFlying = true;
    }

    void ColorConverter()
    {
        pulseVisibleAlpha = pulseVisibleAlpha / 255;
        for(int i=0; i<blackAlphas.Length; i++)
        {
            rainbowAlphas[i] = rainbowAlphas[i] / 255;
            blackAlphas[i] = blackAlphas[i] / 255;
        }
    }

    void Pulsate()
    {
        pulse.DOFade(pulseVisibleAlpha, pulseDuration / 2).OnComplete(DePulse);
    }
    
    void DePulse()
    {
        pulse.DOFade(0, pulseDuration / 2);
    }
    public void Kill()
    {
        if (!lost && !immune)
        {
            black.DOFade(blackAlphas[startLevel], blackFadeSpeed);
            playerAnimations.SetBool("Died", true);
            lost = true;
            //Time.timeScale = 0;
            timerStartTime = Time.time;
            playerRB.bodyType = RigidbodyType2D.Static;
            preLoseMenu.interactable = true;
            preLoseMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            isInLossMenu = true;
            isInPreLoseMenu = true;

            auxCarrot = carrotNumber + PlayerPrefs.GetInt("Carrots");
            PlayerPrefs.SetInt("Carrots", auxCarrot);
            currentTotalCarrots.text = "Total Carrots: " + auxCarrot;
            if (auxCarrot >= buyLifeAmount)
                buyLifeButton.interactable = true;
            else
                buyLifeButton.interactable = false;
        }
    }

    public void Nope()
    {
        preLoseMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5500, 0);
        preLoseMenu.interactable = false;
        lossMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        lossMenu.interactable = true;
        isInPreLoseMenu = false;
        if (PlayerPrefs.GetInt("Highscore") < (int)scoreAmount)
        {
            highscore.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Highscore", (int)scoreAmount);
        }
        else
        {
            noHighscore.enabled = true;
            highscore.enabled = false;
        }
    }

    public void MoveCloudOne()
    {
        clouds[0].transform.localPosition = new Vector3(startX[0], startY, startZ);
        clouds[0].transform.DOLocalMoveX(finalX[0], cloudMoveSpeed).OnComplete(MoveCloudOne).SetEase(Ease.Flash);
    }
    
    public void MoveCloudTwo()
    {
        clouds[1].transform.localPosition = new Vector3(startX[1], startY, startZ);
        clouds[1].transform.DOLocalMoveX(finalX[1], cloudMoveSpeed).OnComplete(MoveCloudTwo).SetEase(Ease.Flash);
    }

    public void BuyALife()
    {
        currentLevel = startLevel + 1;
        GoUpLevel();
        flying = true;
        doubleCarrot = 1;
        xCarrots.SetActive(false);
        if(carrotNumber <= buyLifeAmount)
        {
            carrotNumber = 0;
        }
        else
        {
            carrotNumber -= buyLifeAmount;
        }
        auxCarrot = PlayerPrefs.GetInt("Carrots");
        PlayerPrefs.SetInt("Carrots", auxCarrot - buyLifeAmount);
        carrots.text = "" + carrotNumber;
        buyLifeTimes++;
        buyLifeAmount = 50 * buyLifeTimes;
        buyLifeNum.text = "Buy a life: " + buyLifeAmount + " carrots";
        //Time.timeScale = 1;
        playerRB.bodyType = RigidbodyType2D.Dynamic;
        lost = false;
        isInLossMenu = false;
        isInPreLoseMenu = false;
        lossMenu.interactable = false;
        lossMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5000, 0);
        preLoseMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5500, 0);
        preLoseMenu.interactable = false;
        playerAnimations.SetBool("Died", false);
        immune = true;
        StartCoroutine(ImmuneDelay(2.5f));
        updatedLevel = false;
        halo.SetActive(true);
    }
}
