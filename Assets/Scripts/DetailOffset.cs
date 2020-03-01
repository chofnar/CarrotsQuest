using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailOffset : MonoBehaviour {

	// Use this for initialization
    public int detailType;
    Quaternion rotation;
    void Start()
    {
        switch (detailType)
        {
            case 1:
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                    transform.Rotate(0, 0, Random.Range(0, 360));
                    break;
                }
            case 2:
                {
                    //transform.position = new Vector3(transform.position.x, transform.position.y - 0.76f + Random.Range(0f, 0.18f), transform.position.z);
                    break;
                }
        }
    }
}
