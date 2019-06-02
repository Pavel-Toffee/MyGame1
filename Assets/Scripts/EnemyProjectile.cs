using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    Rigidbody2D myRigidbody;
    public float movespeed;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.AddRelativeForce(Vector2.left * movespeed, ForceMode2D.Impulse);
    }

    void Update()
    {

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
