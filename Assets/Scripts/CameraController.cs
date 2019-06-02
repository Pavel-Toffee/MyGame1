using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // границы движения камеры
    public BoxCollider2D cameraBounds;

    public bool isFollowing;

    public GameObject LeftBorder;

    private Transform player;

    // max и min значения камеры 
    private Vector2 min;
    private Vector2 max;

	
	void Start () {

        player = FindObjectOfType<PlayerController>().transform;
	
	}
		
	void Update () {

        // координаты углов области, куда камера не может выходить 
        min = cameraBounds.bounds.min;
        max = cameraBounds.bounds.max;

        var x = transform.position.x;

        // проверяем ушёл ли игрок дальше чем камера 
        if(isFollowing)
        {
            if(player.position.x > x)
            {
                x = player.position.x;
            }

           if (player.position.x < x)
            {
               x = player.position.x;
            }

        }

        var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float) Screen.width / Screen.height);
        x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);


        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        LeftBorder.transform.position = new Vector2(x - cameraHalfWidth, transform.position.y);

    }
}
