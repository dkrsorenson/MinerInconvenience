using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marshmallow : MonoBehaviour
{
    public float jumpForce;
    public float jumpCooldown;
    public bool onGround;
    public LayerMask groundLayer;
    [SerializeField] int health;
    Rigidbody2D body;
    Animator anim;
    float jumpTimer;

    void Start()
    {
        //Cache references to components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = 100;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        onGround = IsGrounded();
        if(onGround)
        {
            if(jumpTimer > jumpCooldown)
            {
                //trigger animator to jump
                anim.SetTrigger("Jump");
                jumpTimer = 0f;
            }
            jumpTimer += Time.deltaTime;
        }
    }

    //Determine if marshmallow is on the ground
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,
            0.1f, Vector2.down, 0.7f, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    //Jump method, triggered in jump animation
    public void Jump()
    {
        //Add the force in the correct direction to the object
        body.AddForce(new Vector2(-transform.localScale.x,5).normalized * jumpForce, ForceMode2D.Impulse);
        //For now, simply switch the scale x
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
