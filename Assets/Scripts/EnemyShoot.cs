using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {

    public float shootDelay = 1.5f;
    private float shootDelayCounter;

    private bool isActive = false;

    public GameObject projectile;
    public GameObject Dulo;


    void Start () {
	}
	
	
	void Update () {
        if (!isActive) return;
        Shoot();
        shootDelayCounter -= Time.deltaTime;   
    }

    void Shoot()
    {
        if (shootDelayCounter <= 0)
        {
            Instantiate(projectile, Dulo.transform.position, transform.rotation);
            shootDelayCounter = shootDelay;
        }
    }

    void OnBecameVisible()
    {
        isActive = true;
    }

    void OnBecameInvisible()
    {
        isActive = false;
    }
}
