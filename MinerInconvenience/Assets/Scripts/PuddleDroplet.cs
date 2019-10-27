using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleDroplet : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    Vector2 target;
    public float dropletForce = 1.0f;
    public float angle = 5f;
    Vector2 direction;
    Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(transform.position.x, transform.position.y) - lastPosition;
        var localDirection = transform.InverseTransformDirection(direction);
        lastPosition = direction;

        transform.right = direction * rigidBody.velocity.normalized;

        if (IsGrounded())
        {
            Destroy(this.gameObject);
        }
    }

    //Determine if marshmallow is on the ground
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,
            0.05f, Vector2.down, 0.7f, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }


}
