using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour {

    public int type;
    public LevelManager manager;
    public GameObject destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Character")
        {
            manager.AddCollectible(type);
            transform.DOMove(new Vector3(destination.transform.position.x, destination.transform.position.y, 0), 0.25f).OnComplete(Disappear);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
