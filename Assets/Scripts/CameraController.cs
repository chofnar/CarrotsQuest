using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
    public float maxDeltaX, maxDeltaY, cameraOffsetX, cameraOffsetY;
    float xAux, yAux;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if(transform.position.x < player.position.x + maxDeltaX + cameraOffsetX)
        //{
        //    transform.position = new Vector3(Mathf.Clamp(xAux, player.position.x - maxDeltaX + cameraOffsetX, player.position.x + maxDeltaX + cameraOffsetX), transform.position.y, transform.position.z);
        //}
        //if (transform.position.y < Mathf.Abs(player.position.y) + maxDeltaY + cameraOffsetY)
        //{
        //    transform.position = new Vector3(transform.position.x, Mathf.Clamp(yAux, player.position.y - maxDeltaY + cameraOffsetY, player.position.y + maxDeltaY + cameraOffsetY), transform.position.z);
        //}
        transform.position = new Vector3(player.position.x - cameraOffsetX, player.position.y, transform.position.z);
    }
}
