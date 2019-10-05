using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that represents the miner   
public class Miner : MonoBehaviour
{
    //movement vectors
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    //mass for force
    [SerializeField] private float mass;

    private bool isJumping;
    private bool isGounded;
    //public float maxSpeed;

    // Start is called before the first frame update
    public void Start()
    {
        position = transform.position;
        isJumping = false;
    }

    // Update is called once per frame
    public void Update()
    {
        //processing all the keyboard input
        ProcessInput();

        //we have to choose an arbritrary value for gravity
        ApplyGravity(Vector3.down*5);
       
        //updating the velocity, position, and acceleration
        velocity += acceleration * Time.deltaTime;

        /*if(isJumping)
        {
            if(position.y<-1.0f)
            {
                velocity.y = 0.0f;
                isJumping = false;
            }
        }*/

        //placeholder for if anything is below the miner
        //in the final product we have to do a raycast

        if(position.y<-1.0f)
        {
            isGounded = true;
            isJumping = false;
        }

        else
        {
            isGounded = false;
            isJumping = true;
        }

        if(!isJumping&&isGounded)
        {
            velocity.y = 0.0f;
        }

        position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        transform.position = position;
    }

    //method to process keyboard input
    public void ProcessInput()
    {
        //applying an upward force on space bar
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumping == false)
            {
                Debug.Log(position.y);
                position.y += Time.deltaTime;
                ApplyForce(new Vector3(0.0f, 400.0f, 0.0f));
                isJumping = true;
            }
        }

        //forward and backward movement
        //remember to apply friction if user does not push a or d
    }

    //apply friction based on coefficient
    public void ApplyFriction(float coefficient)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction *= coefficient;
        acceleration += friction;
    }

    //function to apply force
    public void ApplyForce(Vector3 force)
    {
        //newton's law
        acceleration += force / mass;
    }

    //gravity is mass agnostic
    public void ApplyGravity(Vector3 gravity)
    {
        acceleration += gravity;
    }

    
}
