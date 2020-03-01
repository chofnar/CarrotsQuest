using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour {

    public Rigidbody2D bunnyBody;
    public bool isFlying = false;
    PlayerController controller;

    private void Start()
    {
        controller = bunnyBody.transform.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && !isFlying && !controller.grounded)
        {
            controller.canJump = false;
            if (bunnyBody.velocity.y > 0)
            {
                bunnyBody.velocity = new Vector2(0, 0);
            }
        }
    }
}
