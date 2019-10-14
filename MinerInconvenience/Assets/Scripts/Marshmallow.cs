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

    public GameObject weaponCollectible;

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
    
    /// <summary>
    /// Reduces the enemy's health
    /// </summary>
    public void TakeDamage()
    {
        // Reduce enemy health
        health -= 25;

        // Check to see if the enemy is dead
        if (IsDead()) Dead();
    }

    /// <summary>
    /// Checks if the enemy is dead
    /// </summary>
    /// <returns>True if the enemy has no remaining health, false otherwise</returns>
    public bool IsDead()
    {
        if (health <= 0) return true;
        return false;
    }

    /// <summary>
    /// Destroys the enemy game object and spawns a weapon collectible in its place
    /// </summary>
    public void Dead()
    {
        Destroy(this.gameObject);
        Instantiate(weaponCollectible, transform.position, Quaternion.identity);
    }
}
