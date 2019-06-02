using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private GameObject currentProjectile;
    public GameObject basicProjectile;

    public float moveSpeed;
    public float jumpHeight;
    public float gravity;
    public float shootDelay;

    public float pixelSize;

    public float[] shootAngles;
    private Quaternion rot;

    private Transform currentShootPoint;
    public Transform[] shootPoints;

    public int direction;

    public LayerMask Solid;
    public LayerMask OneWay;

    private float hsp;
    private float vsp;

    private float shootDelayCounter;

    private bool KeyLeft;
    private bool KeyRight;
    private bool KeyUp;
    private bool KeyDown;
    private bool KeyJump;
    private bool KeyAction;
    private bool KeyJumpOff;

    private bool onGround;
    private bool jumped;
    private bool moving;
    private bool onPlatform;
    private bool obsticleOnRight;
    private bool obsticleOnLeft;

    // координаты box коллайдера
    private Vector2 botLeft;
    private Vector2 botRight;
    private Vector2 topLeft;
    private Vector2 topRight;

    Animator animators;

    private bool isActive;
    private bool isDead;
    public float invincibilityTime;
    public float inactivityTime;
    private float invincCounter;
    private float inactCounter;
    public GameObject SpawnPoint;


    void Start()
    {
        currentProjectile = basicProjectile;
        animators = GetComponent<Animator>();
        shootDelayCounter = 0;
        rot = new Quaternion(0,0,0,0);
    }


    void Update()
    {

        if (!isActive)
        {
            inactCounter -= Time.deltaTime;
            if(inactCounter < 0)
            {
                isActive = true;
                Spawn();
            }
            return;
        }

        if(invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }
        //

        CalculateBounds();

        onGround = 
            CheckCollision(botLeft, Vector2.down, pixelSize, Solid) ||
            CheckCollision(botRight, Vector2.down, pixelSize, Solid) ||
            CheckCollision(botLeft, Vector2.down, pixelSize, OneWay) ||
            CheckCollision(botRight, Vector2.down, pixelSize, OneWay);

        onPlatform = 
            CheckCollision(botLeft, Vector2.down, pixelSize, OneWay) ||
            CheckCollision(botRight, Vector2.down, pixelSize, OneWay);

        obsticleOnRight = (CheckCollision(topRight, Vector2.right, pixelSize, Solid) || CheckCollision(botRight, Vector2.right, pixelSize, Solid));
        obsticleOnLeft = (CheckCollision(topLeft, Vector2.left, pixelSize, Solid) || CheckCollision(botLeft, Vector2.left, pixelSize, Solid));

        GetInput();
        CalculateDirection();
        CalculateShootAngles();
        CalculateShootPoint();
        Move();
        Shoot();

        // анимация
        if(onGround == false || jumped == true || vsp > 0.1f)
        {
            animators.Play("Player_jump");
        }
        if ((onGround == true) && ((!KeyLeft && !KeyRight) || (KeyLeft && KeyRight)))
        {
            animators.Play("Player_idle");
        }
        if (((KeyLeft && !obsticleOnLeft) || (KeyRight && !obsticleOnRight)) && onGround == true)
        {
            animators.Play("Player_run");
        }
        if(KeyAction)
        {
            animators.Play("Player_shoot");
        }

        Reload();
    }


    void GetInput ()
    {
        KeyLeft = Input.GetKey(KeyCode.LeftArrow);
        KeyRight = Input.GetKey(KeyCode.RightArrow);
        KeyUp = Input.GetKey(KeyCode.UpArrow);
        KeyDown = Input.GetKey(KeyCode.DownArrow);
        KeyJump = Input.GetKeyDown(KeyCode.X);
        KeyAction = Input.GetKeyDown(KeyCode.Z);
        KeyJumpOff = KeyDown && KeyJump;
    }


    void Move()
    {
        if (KeyLeft && !obsticleOnLeft)
        {
            hsp = -moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
            
        }
        if (KeyRight && !obsticleOnRight)
        {
            hsp = moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(1, 1, 1);

        }

        if (KeyRight || KeyLeft)
        {
            moving = true;
        }

        if ((!KeyLeft && !KeyRight) || (KeyLeft && KeyRight))
        {
            moving = false;
            hsp = 0;
        }

       

        if (onPlatform && KeyJumpOff)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - pixelSize);
            onGround = false;
        }


        if (KeyJump && onGround)
        {
            jumped = true;
            vsp = jumpHeight;
            onGround = false; 
        }


        if (!onGround)
            vsp -= gravity * Time.deltaTime;



        if ((vsp < 0) && (CheckCollision(botLeft, Vector2.down, Mathf.Abs(vsp), Solid) || CheckCollision(botRight, Vector2.down, Mathf.Abs(vsp), Solid)) )
        {
            float dist1 = CheckCollisionDistance(botLeft, Vector2.down, Mathf.Abs(vsp), Solid);
            float dist2 = CheckCollisionDistance(botRight, Vector2.down, Mathf.Abs(vsp), Solid);
            if (dist1 <= dist2) vsp = -dist1;
            else vsp = -dist2;
            transform.position = new Vector2(transform.position.x, transform.position.y + vsp + pixelSize/2);
            vsp = 0;
        }

        if ((vsp < 0) && (CheckCollision(botLeft, Vector2.down, Mathf.Abs(vsp), OneWay) || CheckCollision(botRight, Vector2.down, Mathf.Abs(vsp), OneWay)))
        {
            float dist1 = CheckCollisionDistance(botLeft, Vector2.down, Mathf.Abs(vsp), OneWay);
            float dist2 = CheckCollisionDistance(botRight, Vector2.down, Mathf.Abs(vsp), OneWay);
            if (dist1 <= dist2) vsp = -dist1;
            else vsp = -dist2;
            transform.position = new Vector2(transform.position.x, transform.position.y + vsp + pixelSize / 2);
            vsp = 0;
        }

        if ((vsp > 0) && (CheckCollision(topLeft, Vector2.up, vsp, Solid) || CheckCollision(topRight, Vector2.up, vsp, Solid)) )
        {
            float dist1 = CheckCollisionDistance(topLeft, Vector2.up, vsp, Solid);
            float dist2 = CheckCollisionDistance(topRight, Vector2.up, vsp, Solid);
            if (dist1 <= dist2) vsp = dist1;
            else vsp = dist2;
            transform.position = new Vector2(transform.position.x, transform.position.y + vsp + pixelSize / 2);
            vsp = 0;
        }

        if ((hsp > 0) && (CheckCollision(topRight, Vector2.right, hsp, Solid) || CheckCollision(botRight, Vector2.right, hsp, Solid)) )
        {
            float dist1 = CheckCollisionDistance(topRight, Vector2.right, hsp, Solid);
            float dist2 = CheckCollisionDistance(botRight, Vector2.right, hsp, Solid);
            if (dist1 <= dist2) hsp = dist1;
            else hsp = dist2;
            transform.position = new Vector2(transform.position.x + hsp, transform.position.y);
            hsp = 0;
        }

        if ((hsp < 0) && (CheckCollision(topLeft, Vector2.left, Mathf.Abs(hsp), Solid) || CheckCollision(botLeft, Vector2.left, Mathf.Abs(hsp), Solid)) )
        {
            float dist1 = CheckCollisionDistance(topLeft, Vector2.left, Mathf.Abs(hsp), Solid);
            float dist2 = CheckCollisionDistance(botLeft, Vector2.left, Mathf.Abs(hsp), Solid);
            if (dist1 <= dist2) hsp = -dist1;
            else hsp = -dist2;
            transform.position = new Vector2(transform.position.x + hsp, transform.position.y);
            hsp = 0;
        }

        if (vsp == 0) jumped = false;
    
        transform.position = new Vector2(transform.position.x + hsp, transform.position.y + vsp);
    }


    private void Shoot()
    {
        if(KeyAction && shootDelayCounter <= 0)
        {
            if((currentProjectile == basicProjectile) && FindObjectsOfType<Projectile>().Length < 3)
            {
                Instantiate(currentProjectile, currentShootPoint.position, rot);
                shootDelayCounter = shootDelay;
            }
           
        }
        shootDelayCounter -= Time.deltaTime;
    }

    // направление стрельбы
     void CalculateDirection()
    {
        if (transform.localScale.x > 0)
        {
            if (KeyRight) direction = 6;
        }
        else if (transform.localScale.x < 0)
        {
            if (KeyLeft) direction = 4;
        }
    }

    //вычислить точку для стрельбы
    void CalculateShootPoint()
    {
        if (direction == 4 || direction == 6)
        {
            currentShootPoint = shootPoints[0];
        }
    }

    void CalculateShootAngles()
    {
        if (direction == 6) rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootAngles[0]);
        if (direction == 4) rot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -shootAngles[1]);
    } 

    


    // проверка столкновения
    private bool CheckCollision(Vector2 raycastOrigin,Vector2 direction,float distance,LayerMask layer)
    {
      return Physics2D.Raycast(raycastOrigin, direction, distance, layer);
    }

    private float CheckCollisionDistance(Vector2 raycastOrigin, Vector2 direction, float distance, LayerMask layer)
    {
        int i = 0;
        while(Physics2D.Raycast(raycastOrigin, direction, distance, layer))
        {
            i++;
            if (distance > pixelSize)
                distance -= pixelSize;
            else distance = pixelSize;  //// возможно баг
            if (i > 1000) return 0;
        }
        return distance;
    }

    private void CalculateBounds()
    {
        Bounds b = GetComponent<BoxCollider2D>().bounds;
        topLeft = new Vector2(b.min.x, b.max.y);
        botLeft = new Vector2(b.min.x, b.min.y);
        topRight = new Vector2(b.max.x, b.max.y);
        botRight = new Vector2(b.max.x, b.min.y);
    }

    private void Spawn()
    {
        invincCounter = invincibilityTime;
    }

    public void Death()
    {
        if (invincCounter > 0) return;
        isDead = true;
        isActive = false;
        inactCounter = inactivityTime;
        transform.position = SpawnPoint.transform.position;
    }

    void Reload()
    {
         if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Level-1");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }


}
