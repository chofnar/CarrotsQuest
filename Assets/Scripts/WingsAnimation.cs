using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WingsAnimation : MonoBehaviour {
	[Range(0,500)]
	public float speed = 375;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, speed * Time.deltaTime, 0) ;
	}
}
