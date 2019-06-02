using UnityEngine;
using System.Collections;

public class KillAll : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<EnemyManager>() != null)
            {
                other.GetComponent<EnemyManager>().TakeDamage();
            }
        }

        else if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                other.GetComponent<PlayerController>().Death();
            }
        }
    }


}
