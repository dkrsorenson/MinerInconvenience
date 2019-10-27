using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Miner : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask minerLayer;
    private MinerHealthManager healthUIManager;
    [SerializeField]float speed = 5f;
    [SerializeField] float moveForce = 300f;
    [SerializeField] float jumpForce = 11f;
    private Vector2 inputVector;
    private bool didChangeDirection = false;
    private bool isWalking = false;
    int weaponPiecesCounter;
    int collectibleCounter;
    private bool movedInAir = false;
    private float lives;
    private const int maxLives = 5;
    public Text weaponCollectibleTextBox;
    public Color flashColor;
    [SerializeField] Transform canvas;
    private bool minerFound;
    private int giveDamageAmount;
    private bool upgraded;

    // Start is called before the first frame update
    public void Start()
    {
        weaponPiecesCounter = 0;
        collectibleCounter = 0;
        lives = 3;

        if (SceneManager.GetActiveScene().name == "BossScene")
        {
            lives = PlayerPrefs.GetFloat("playerHealth", 3);
            weaponPiecesCounter = PlayerPrefs.GetInt("marshmallowCount", 0);
            upgraded = PlayerPrefs.GetInt("isUpgraded", 0) == 1 ? true : false;
        }

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        healthUIManager = GameObject.Find("Health Manager").GetComponent<MinerHealthManager>();

        giveDamageAmount = 25;
        upgraded = false;

        healthUIManager.UpdateLifeDisplay(lives);
    }

    // Update is called once per frame
    public void Update()
    {
        // Get the direction of input on the horizontal axis
        //inputVector = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (Input.GetMouseButtonUp(0))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MinerAttack"))
            {
                Attack();
                // set is attacking bool
                animator.SetTrigger("attack");
            }
        }

        FlipSprite();

        if(rigidBody.velocity.y < -50)
        {
            SceneManager.LoadScene("GameOver");
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvas.gameObject.activeInHierarchy == false)
            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        if(!upgraded && weaponPiecesCounter >= 7)
        {
            giveDamageAmount = 50;
            upgraded = true;
        }
    }

    public void FixedUpdate()
    {
        //WalkALt();

        //if it is grounded then apply a jump force
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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

        // set animation speed
        animator.SetFloat("speed", Mathf.Abs(inputVector.x));
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
            if(lives<=maxLives)
            {
                lives += 0.5f;
                healthUIManager.UpdateLifeDisplay(lives);
            }
            collectibleCounter++;
        }
        else if(collision.gameObject.tag == "WeaponCollectible")
        {
            Destroy(collision.gameObject);
            weaponPiecesCounter++;
            weaponCollectibleTextBox.text = weaponPiecesCounter.ToString();
        }
        else if (collision.gameObject.tag == "BossSceneTrigger")
        {
            //PlayerPrefs.SetFloat("playerHealth", lives);
            //PlayerPrefs.SetInt("marshmallowCount", weaponPiecesCounter);
            //PlayerPrefs.SetInt("isUpgraded", upgraded ? 1 : 0);
            //SceneManager.LoadScene("BossScene");
            SceneManager.LoadScene("GameWon");
        }
        else if(collision.gameObject.tag == "EndSceneTrigger")
        {
            SceneManager.LoadScene("GameWon");
        }
        else if(collision.gameObject.tag == "ChocolatePuddle")
        {
            // Cut players speed in half
            speed = (speed / 2);

            // Cut player jump in half
            jumpForce = 7.5f;

            // Play splash droplets
            collision.GetComponent<Puddle>().Splash(this.transform);

            animator.SetFloat("animationSpeed", Mathf.Abs(0.5f));

            //animator.speed = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChocolatePuddle")
        {
            // Reset speed
            speed = 5.0f;

            // Reset jump
            jumpForce = 11f;

            animator.SetFloat("animationSpeed", Mathf.Abs(1.0f));
        }
    }

    /// <summary>
    /// Attacking the enemies
    /// </summary>
    public void Attack()
    {
        float rayCastSign = playerSpriteRenderer.flipX ? -1.0f : 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * rayCastSign, 2.0f, enemyLayer);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponentInParent<Marshmallow>().TakeDamage(giveDamageAmount);
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

    /// <summary>
    /// Reduces the player's lives
    /// </summary>
    public IEnumerator TakeDamage()
    {
        // Reduce the player's lives
        lives -= 0.50f;

        /*
        if (tag == "Enemy")
        {
            
        }
        */

        playerSpriteRenderer.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        playerSpriteRenderer.color = Color.white;

        

        // Update the UI to display how many lives the player now has
        healthUIManager.UpdateLifeDisplay(lives);

        // Check to see if the player is dead
        if (IsDead()) Dead();
    }

    /// <summary>
    /// Checks if the player is dead
    /// </summary>
    /// <returns>True if the player has no lives remaining, false otherwise</returns>
    public bool IsDead()
    {
        if (lives <= 0) return true;
        return false;
    }

    public float GetHealth()
    {
        return lives;
    }

    public int GetMarshmallowCount()
    {
        return weaponPiecesCounter;
    }

    public bool HasPickaxe()
    {
        return upgraded;
    }

    /// <summary>
    /// Called when player dies
    /// </summary>
    public void Dead()
    {
        SceneManager.LoadScene("GameOver");
    }
}
