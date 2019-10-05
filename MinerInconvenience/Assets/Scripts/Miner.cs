using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{

    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isJumping;
    [SerializeField]private bool grounded;

    // Start is called before the first frame update
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        //doing a raycast to see if it is grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);

        //if it is then set grounded to true
        if(hit.collider!=null)
        {
            grounded = true;
        }

        else
        {
            grounded = false;
        }

        //if it is grounded then apply a jump force
        if (grounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidBody.AddForce(new Vector2(0, 8),ForceMode2D.Impulse);
            }
        }
    }
}
