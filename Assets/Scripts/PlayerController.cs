using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    Rigidbody2D playerBody;
	[Header("Jump Particle System")]
	public ParticleSystem psDoubleJump;
	[Space(10)]
    public LevelManager levelManager;
    public float jumpSpeed, moveSpeed, rotationDuration, walkBoingDuration;
    int jumpCounter;
    Animator animationController;
    public bool grounded, doubleJumped, jumped, stop = false, spawnedLossPlatform = false, initialTouch = false, canJump = true, isInPauseMenu, isNotOnPauseButton;
    public GroundCheck playerGroundCheck;
    public Image tapToStartImg;

    private void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        animationController = gameObject.GetComponent<Animator>();
    }

    void Update () {
        //force move right if game started
        if (initialTouch && !stop)
        {
            playerBody.velocity = new Vector2(moveSpeed, playerBody.velocity.y);
            stop = true;
        }
        if(!grounded)
        {
            animationController.SetBool("Jumped", true);
        }
        //Jump
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            //test if not over pause menu
            if (Input.touchCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    isNotOnPauseButton = false;
                }
                else
                    isNotOnPauseButton = true;
            }
            else
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    isNotOnPauseButton = false;
                }
                else
                    isNotOnPauseButton = true;
            }
            //
            if (grounded && !jumped && initialTouch)
            {
                Jump(true);
            }
            else
            {
                if (jumped && !doubleJumped && initialTouch)
                {
                    Jump(false);
                }
                if(!jumped && !doubleJumped && initialTouch)
                {
                    Jump(true);
                }
            }

            if (!initialTouch && isNotOnPauseButton)
            {
                //playerGroundCheck.detectLose = true;
                tapToStartImg.enabled = false;
                StartCoroutine(StartLossDelay());
            }
        }
        //Walk animation && lose
        if (playerBody.velocity.x > 0f)
            animationController.SetBool("IsWalking", true);
        else
        {
            if (initialTouch && !spawnedLossPlatform)
            {
                levelManager.GoDownLevel();
                spawnedLossPlatform = true;
                playerGroundCheck.detectLose = true;
            }
        }
    }

    void Jump(bool jumpType)//true = first jump , false = second jump
    {
        if (playerBody.velocity.x > 0 && canJump && !isInPauseMenu && isNotOnPauseButton)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
			if (jumpType) {
				jumped = true;
			} else {
				doubleJumped = true;
				//psDoubleJump.Play ();
			}
        }
    }

    IEnumerator StartLossDelay()
    {
        yield return new WaitForSeconds(0.1f);
        levelManager.canLose = true;
        spawnedLossPlatform = false;
        initialTouch = true;
    }
}
