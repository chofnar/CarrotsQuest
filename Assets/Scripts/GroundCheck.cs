using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    public LevelManager levelManager;
    public GameObject player, grassParticles;
    public PlayerController myBool;
    public Animator animationController;
    Rigidbody2D playerBody;
    public bool detectLose = false;

    private void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            myBool.grounded = true;
            myBool.jumped = false;
            myBool.doubleJumped = false;
            myBool.spawnedLossPlatform = false;
            myBool.canJump = true;
            animationController.SetBool("Jumped", false);
            if (myBool.initialTouch && detectLose)
            {
                playerBody.velocity = new Vector2(myBool.moveSpeed, playerBody.velocity.y);
                Instantiate(grassParticles, player.transform).transform.parent = null;
                levelManager.canLose = false;
                levelManager.StartCoroutine("LossDelay");
                detectLose = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
            myBool.grounded = false;
    }
}
