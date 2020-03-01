using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderplatformCollider : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Character" && collision.transform.position.y > transform.position.y)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
