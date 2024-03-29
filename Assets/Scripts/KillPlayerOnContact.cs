﻿using UnityEngine;
using System.Collections;

public class KillPlayerOnContact : MonoBehaviour {

    public bool killSelf;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerController>().Death();
            if (killSelf)
            {
                Destroy(gameObject);
            }
        }
    }
}
