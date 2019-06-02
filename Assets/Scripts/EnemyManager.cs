using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public int health;
    private int currenthealth;

    public GameObject deathEffect;

	void Start () {
        currenthealth = health;
	}
	
	void Update () {
	
	}


    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        if(deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }



}
