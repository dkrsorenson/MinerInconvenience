using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshmallowBoss : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private Animation anim;
    [SerializeField] LayerMask minerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int health;
    [SerializeField] Color flashColor;
    private float attackTimer;
    [SerializeField] float attackCooldown;
    [SerializeField] float flashTimer;
    [SerializeField] int numFlashes;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float knockbackForce;
    private Vector3 minerPosition;
    private bool minerFound;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackCooldown = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        LookForMiner();

        if(attackTimer>attackCooldown)
        {
            // TODO in future, probably update to anim.setTrigger("attack");
            Attack();
        }

    }

    private void FixedUpdate()
    {
        
    }

    void Attack()
    {

    }

    void LookForMiner()
    {
        //raycasting both sides to look for miner
        RaycastHit2D hitForward;
        RaycastHit2D hitBackward;

        //cast a ray forward and backward
        hitForward = Physics2D.CircleCast(transform.position, 0.1f, transform.right, 10f, minerLayer);
        hitBackward = Physics2D.CircleCast(transform.position, 0.1f, -transform.right, 10f, minerLayer);

        if (hitForward.collider != null)
        {
            minerFound = true;
            minerPosition = hitForward.collider.gameObject.transform.position;
        }

        else if (hitBackward.collider != null)
        {
            minerPosition = hitBackward.collider.gameObject.transform.position;
            minerFound = true;
        }

        else
        {
            minerFound = false;
        }
    }
}
