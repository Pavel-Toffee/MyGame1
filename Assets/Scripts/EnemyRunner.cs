using UnityEngine;
using System.Collections;

public class EnemyRunner : MonoBehaviour {

    private bool onGround;
    public Transform groundSensor;

    private bool cliffAhead;
    public Transform cliffSensor;

    private bool solid;
    public Transform solidSensor;

    public LayerMask ground;

    private Rigidbody2D mybody;
    private Animator myAnimator;

    public float moveSpeed;
    public float jumpHeight;

    private bool reacted;

    private bool isActive = false;

    void Start () {
        mybody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}
	



	void Update () {

        if (!isActive) return;

        onGround = Physics2D.OverlapCircle(groundSensor.position, 0.1f, ground); // на земле ли
        cliffAhead = !Physics2D.OverlapCircle(cliffSensor.position, 0.1f, ground); // есть ли впереди обрыв
        solid = Physics2D.OverlapCircle(solidSensor.position, 0.1f, ground); // есть ли впереди обрыв

        // отреагировать на обрыв
        if (onGround && cliffAhead && !reacted)
        {
            ReactToCliff(Random.Range(0,4));
        }
        if (onGround && !cliffAhead && reacted)
        {
            reacted = false;
        }
        if (onGround && solid)
        {
            mybody.velocity = new Vector2(0, mybody.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }


        // двигаться
        mybody.velocity = new Vector2(moveSpeed *transform.localScale.x, mybody.velocity.y);

        myAnimator.SetBool("OnGround", onGround);
    }

    // реакция на обрыв
    void ReactToCliff(float r)
    {
        if(r == 0) // прыгнуть
        {
            mybody.velocity = new Vector2(mybody.velocity.x, jumpHeight);       
        }
        if (r == 1) // упасть 
        {
            mybody.velocity = new Vector2(mybody.velocity.x, jumpHeight/3);    
        }
        if (r > 1) // развернуться
        {
            mybody.velocity = new Vector2(0, mybody.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
        reacted = true;
    }


    void OnBecameVisible()
    {
        isActive = true;
    }

  /*  void OnBecameInvisible()
    {
        isActive = false;
    }
    */


}
