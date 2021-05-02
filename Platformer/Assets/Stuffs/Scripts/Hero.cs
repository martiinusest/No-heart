using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Damage   
{
    public float speed = 3f;
    public int lives = 5;
    public float jumpForce = 0.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGround = false;
    private Animator anim;

    public static Hero Instance { get; set; }

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 5) lives = value;
            healthBar.Refresh();
        }
    }

    private HealthBar healthBar;


    public override void GetDamage()
    {
        Lives -= 1; 
        
        if (lives < 1)
        {
            Die();
        }
        
    }


    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        healthBar = FindObjectOfType<HealthBar>();
        Instance = this;

    }

    private void Run()
    {
        if (isGround) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void checkGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGround = collider.Length > 1;

        if (!isGround) State = States.jump;
    }

    private void FixedUpdate()
    {
        checkGround();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGround) State = States.idle;

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }

        if (isGround && Input.GetKeyDown("up") || (isGround && Input.GetKeyDown("w")))
        {
            Jump();
        }

        
    }

}

public enum States
{
    idle,
    run,
    jump
}