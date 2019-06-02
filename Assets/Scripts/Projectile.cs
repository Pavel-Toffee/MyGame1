using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    Rigidbody2D myRigidbody;
    public float movespeed;


	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.AddRelativeForce(Vector2.right* movespeed, ForceMode2D.Impulse);
    }

	void Update () {
	
	}
    
    void OnBecameInvisible ()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if(other.GetComponent<EnemyManager>() != null)
            {
                other.GetComponent<EnemyManager>().TakeDamage();
                Destroy(gameObject);
            }
        }
    }

}
