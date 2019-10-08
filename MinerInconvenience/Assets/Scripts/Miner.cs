using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private LayerMask groundLayer;

    public float speed = 5f;
    private Vector2 inputVector;
    private bool didChangeDirection = false;
    private bool isWalking = false;

    // Start is called before the first frame update
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    public void Update()
    {
        // Get the direction of input on the horizontal axis
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        FlipSprite();
    }

    public void FixedUpdate()
    {

        //if it is grounded then apply a jump force
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * 12, ForceMode2D.Impulse);
        }

        Walk();

        FlipSprite();

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
        if (inputVector.x * rigidBody.velocity.x < 4)
        {
            rigidBody.AddForce(Vector2.right * inputVector.x * 280);
        }

        // add some velocity
        if (Mathf.Abs(rigidBody.velocity.x) > 4)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * 3, rigidBody.velocity.y);
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
        }

        // If they are jumping, keep moving them forward if they were walking before but let go of the key
        if (!grounded && isWalking && inputVector.x == 0)
        {
            if (!didChangeDirection) inputVector.x = 1 * speed;
            else inputVector.x = -1 * speed;
        }

        rigidBody.velocity = new Vector2(inputVector.x, rigidBody.velocity.y);
    }

    /// <summary>
    /// Flips the sprite to face the direction of player movement
    /// </summary>
    public void FlipSprite()
    {
        if (rigidBody.velocity.x < 0) playerSpriteRenderer.flipX = true;
        else playerSpriteRenderer.flipX = false;
    }
}
