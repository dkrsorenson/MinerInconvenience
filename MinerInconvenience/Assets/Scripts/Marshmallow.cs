using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marshmallow : MonoBehaviour
{
    public float jumpForce;
    public float jumpCooldown;
    public bool onGround;
    public LayerMask groundLayer;
    [SerializeField] LayerMask minerLayer;
    [SerializeField]private float directionToMinerSign;
    private bool minerFound;
    private Vector3 minerPosition;
    public GameObject weaponCollectible;
    public Color flashColor;
    public float flashTimer;
    public int numFlashes;
    [SerializeField] int health;
    Rigidbody2D body;
    Animator anim;
    float jumpTimer;
    SpriteRenderer spriteRenderer;
    public float knockbackForce;

    void Start()
    {
        //Cache references to components
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        health = 100;
    }

    private void Update()
    {
        LookForMiner();

        if (body.velocity.y < -50)
        {
            Destroy(gameObject);
        }
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
                Jump();
            }
            jumpTimer += Time.deltaTime;
        }
    }

    //Determine if marshmallow is on the ground
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,
            0.2f, Vector2.down, 0.7f, groundLayer);
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
        if (minerFound)
        {
            Vector2 direction = (minerPosition - transform.position).normalized;
            directionToMinerSign = Mathf.Sign(Vector2.Dot(transform.right,direction));

            //adding a force in the direction of the miner
            body.AddForce(new Vector2(directionToMinerSign*Mathf.Abs(transform.localScale.x), 5).normalized * jumpForce, ForceMode2D.Impulse);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * directionToMinerSign, transform.localScale.y, transform.localScale.z);
            
        }
        else
        {
            body.AddForce(new Vector2(-transform.localScale.x, 5).normalized * jumpForce, ForceMode2D.Impulse);
            //For now, simply switch the scale x
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
    
    /// <summary>
    /// Reduces the enemy's health by damage amount
    /// </summary>
    public void TakeDamage(int amount)
    {
        // Reduce enemy health
        health -= amount;

        // Flash the sprite
        StartCoroutine(FlashOnDamage());

        // Check to see if the enemy is dead
        if (IsDead()) Dead();
    }

    void LookForMiner()
    {
        //raycasting both sides to look for miner
        RaycastHit2D hitForward;
        RaycastHit2D hitBackward;

        //cast a ray forward and backward
        hitForward = Physics2D.CircleCast(transform.position, 0.1f, transform.right, 10f, minerLayer);
        hitBackward = Physics2D.CircleCast(transform.position, 0.1f, -transform.right, 10f, minerLayer);

        if(hitForward.collider!=null)
        {
            minerFound = true;
            minerPosition = hitForward.collider.gameObject.transform.position;
        }

        else if(hitBackward.collider!=null)
        {
            minerPosition = hitBackward.collider.gameObject.transform.position;
            minerFound = true;
        }

        else
        {
            minerFound = false;
        }
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
        Destroy(gameObject);
        Instantiate(weaponCollectible, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Flashes the enemy sprite red when they take damage
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashOnDamage()
    {
        for (int i = 0; i < numFlashes; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashTimer);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashTimer);
        }
    }

    //Enemy Attack
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Miner>().StartCoroutine("TakeDamage");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.position - collision.transform.position).normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChocolatePuddle")
        {
            // Cut speed in half
            jumpForce = (jumpForce / 2);

            // Splash particles
            collision.gameObject.GetComponent<Puddle>().PlaySplashParticles(this.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChocolatePuddle")
        {
            // Reset speed
            jumpForce = 8.0f;
        }
    }
}
