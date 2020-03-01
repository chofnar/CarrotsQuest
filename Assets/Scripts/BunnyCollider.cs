using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyCollider : MonoBehaviour {

    public GameObject bunnyObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Spike")
        {
            bunnyObject.GetComponent<PlayerController>().levelManager.Kill();
        }
    }
}
