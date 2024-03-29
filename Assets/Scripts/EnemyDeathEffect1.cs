﻿using UnityEngine;
using System.Collections;

public class EnemyDeathEffect1 : MonoBehaviour {

    public float height;
    private int scale;



	void Start () {
        scale = Mathf.Clamp((int)(FindObjectOfType<PlayerController>().transform.position.x * 10000 - transform.position.x * 10000), -1, 1);
        transform.localScale = new Vector3(scale, 1, 1);
        if(GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-scale * 3, height);
        }
	}
	
	
	void Update () {
	
	}
}
