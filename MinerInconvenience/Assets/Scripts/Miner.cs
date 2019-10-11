using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField]float speed = 5f;
    [SerializeField] float moveForce = 300f;
    private Vector2 inputVector;
    private bool didChangeDirection = false;
    private bool isWalking = false;
    int marshmallowCounter;
    int collectibleCounter;
    private bool movedInAir = false;

    // Start is called before the first frame update
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        marshmallowCounter = 0;
        collectibleCounter = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        // Get the direction of input on the horizontal axis
        //inputVector = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (Input.GetMouseButtonUp(0))
        {
            Attack();
        }



        FlipSprite();
    }

    public void FixedUpdate()
    {
        //WalkALt();

        //if it is grounded then apply a jump force
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * 11, ForceMode2D.Impulse);
        }

        Walk();
    }

    /// <summary>
    /// Determine if the player is grounded
    /// </summary>
    /// <returns>True if they player is grounded, false otherwise</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,
            0.1f, Vector2.down, 0.7f, groundLayer);

        if (hit.collider != null)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Walk
    /// </summary>
    public void Walk()
    {
        if (inputVector.x * rigidBody.velocity.x < speed)
        {
            rigidBody.AddForce(Vector2.right * inputVector.x * moveForce);
        }

        // add some velocity
        if (Mathf.Abs(rigidBody.velocity.x) > speed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * speed, rigidBody.velocity.y);
        }
    }

    /// <summary>
    /// Moves the player in the direction of input
    /// </summary>
    public void WalkALt()
    {
        bool grounded = IsGrounded();

        // If they are grounded, keep track of if they are moving
        if (grounded)
        {
            if (inputVector.x == 0) isWalking = false;
            else isWalking = true;

            movedInAir = false;
        }
        else // Keep track of if they move while in the air
        {
            if ((inputVector.x > 0 || inputVector.x < 0) && !movedInAir) movedInAir = true;
        }

        // Keep moving the player forward when they jump
        if(!grounded && inputVector.x == 0)
        {
            if (isWalking || movedInAir)
            {
                if (!didChangeDirection) inputVector.x = 0.5f * speed;
                else inputVector.x = -0.5f * speed;
            }
        }
        else if(!grounded && inputVector.x != 0)
        {
            if (!didChangeDirection) inputVector.x = 0.75f * speed;
            else inputVector.x = -0.75f * speed;
        }

        rigidBody.velocity = new Vector2(inputVector.x, rigidBody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HealthCollectible")
        {
            Destroy(collision.gameObject);
            collectibleCounter++;
        }
    }

    /// <summary>
    /// Attacking the enemies
    /// </summary>
    public void Attack()
    {
        float rayCastSign = playerSpriteRenderer.flipX ? -1.0f : 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * rayCastSign, 10f,enemyLayer);

        if(hit.collider!=null)
        {
            Debug.Log("hit");
        }
    }

    /// <summary>
    /// Flips the sprite to face the direction of player movement
    /// </summary>
    public void FlipSprite()
    {
        if (inputVector.x > 0) didChangeDirection = false;
        else if (inputVector.x < 0) didChangeDirection = true;

        playerSpriteRenderer.flipX = didChangeDirection;
    }
}
