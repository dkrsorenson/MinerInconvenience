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
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);

        FlipSprite();
    }

    public void FixedUpdate()
    {
        Walk();

        //if it is grounded then apply a jump force
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * 12, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Determine if the player is grounded
    /// </summary>
    /// <returns>True if they player is grounded, false otherwise</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,
            0.1f, Vector2.down, 0.5f, groundLayer);
        if (hit.collider != null)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Moves the player in the direction of input
    /// </summary>
    public void Walk()
    {
        rigidBody.velocity = new Vector2(inputVector.x, rigidBody.velocity.y);
    }

    /// <summary>
    /// Flips the sprite to face the direction of player movement
    /// </summary>
    public void FlipSprite()
    {
        if(inputVector.x > 0 && didChangeDirection)
        {
            didChangeDirection = false;
        }
        else if (inputVector.x < 0 && !didChangeDirection)
        {
            didChangeDirection = true;
        }

        playerSpriteRenderer.flipX = didChangeDirection;
    }
}
