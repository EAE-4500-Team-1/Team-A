using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    CLASS: Board_Move
    DESC: This class is used to access the physics and visual model of the board.
    This class is responsible for
    A) Rudimentary movement
    B) Rudimentary rotational, such as when one turns on the board.
    C) Storing constant movement values that can easily be adjusted at the top of the .cs file.


 */
public class Board_Move : MonoBehaviour
{

    //Constant members of the class.

    //Movement constants
    public float VELOC = 10f; //This constant is used to determine the movement speed in all directions punched in on the keyboard.
    public float ROTATE = 10f; //This constant is used to determine the rotate rate of the board. 
   
   //GameObject assignables: We use this area to track what elements of the game object we are changing.
   //The rigidbody (physics!) of the board's master object. The master object controls the collider, offsetting the physical board off of the collider for drawing purposes.
    Rigidbody rb; //master object's rigid body. We use this to move the entire board.
    Transform child_trans; //this child transform is used to manipulate the rotation of the board. This is just good for tilting the board physically, giving it a realistic look.
   
    
    //A slight rotation to the board in the clockwise rotation.
    Quaternion rotationClock; //Clockwise rotation.
    Vector3 rotationClockVeloc;

    //A slight rotation to the board in the counter clockwise rotation.
    Quaternion rotationCounterClock; //Counterclockwise rotation.
    Vector3 rotationCounterClockVeloc;
    // Start is called before the first frame update
    //We can use this for a lot of setup before the scene even starts.

    //RigidBody Acceleration Foward and Back
    public Vector3 forwardVeloc;
    public Vector3 backwardVeloc;

    //RigidBody right and left rotations
    public Vector3 rightVeloc;
    public Vector3 leftVeloc;

    private float currentRotation = 0f; //poll for our current rotation.

    void Start()
    {   //since we can't do static assignments for gameobject members, we have to assign them here.
        rb = gameObject.GetComponent<Rigidbody>(); //The rigid body for the board, assigned on start of game.
        child_trans = this.transform.GetChild(0).transform; //get the transform of the board's physical representation.
        //Rotational assignments.
        rotationClockVeloc = new Vector3(0f, ROTATE, 0f);
        forwardVeloc = transform.forward * VELOC * Time.deltaTime;
        backwardVeloc = -transform.forward * VELOC * Time.deltaTime;
        rightVeloc = transform.right * VELOC * Time.deltaTime;
        leftVeloc = -transform.right * VELOC * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //This is used for physics calculations.
    void FixedUpdate(){
        forwardVeloc = transform.forward * VELOC * Time.deltaTime;
        backwardVeloc = -transform.forward * VELOC * Time.deltaTime;     
        rightVeloc = transform.right * VELOC * Time.deltaTime;
        leftVeloc = -transform.right * VELOC * Time.deltaTime;  
    }

    // A simple method whose entire purpose is to listen to the keyboard event handler to allow our board to move.
    void Move(){
        Vector3 currentVector = rb.velocity; //access the current vector that our board is traveling on.
        if(Input.GetKey(KeyCode.W)){
            if(rb.velocity.magnitude < 20f)
            rb.velocity += transform.forward * VELOC;
            //rb.transform.localPosition += new Vector3(0.0f, 0.0f, VELOC);
            if(currentRotation >= -2.0f){
                child_trans.Rotate(-1f, 0, 0);
                currentRotation -= 1f;
            }
            else{

            }
            print("MoveForward!");
        }

        if(Input.GetKey(KeyCode.S)){
            if(rb.velocity.magnitude < 20f){
                rb.velocity += -transform.forward * VELOC;
            }
            if(currentRotation <= 2.0f){
                child_trans.Rotate(new Vector3(1f, 0, 0));
                currentRotation += 1f;
            }
            else {
                
            }
            print("MoveBack!");
        }

        //TODO: Add rotational effects to this.

        //these two if statements handle forward motion and turns.
        if(Input.GetKey(KeyCode.A)){ 
            if(rb.velocity.magnitude < 20f){
                //rb.velocity += (-transform.right * VELOC);
            }
            gameObject.transform.Rotate(new Vector3(0f, -ROTATE * Time.deltaTime, 0f));
            print("ROTATE LEFT");
        }

        if(Input.GetKey(KeyCode.D)){
            //rb.velocity += new Vector3(VELOC, 0f, 0f);
            if(rb.velocity.magnitude < 20f){
                //rb.velocity += (transform.right * VELOC);
            }
            gameObject.transform.Rotate(new Vector3(0f, ROTATE * Time.deltaTime, 0f));
            print("ROTATE RIGHT");
        }
        /*
        //backward motion and turn handler
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S)){ 
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, -ROTATE, 0f));
            rb.rotation = rotationCounterClock;
            //child_trans.Rotate(new Vector3(2f, -1f, -2f));
        }

        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)){
            rb.velocity += new Vector3(VELOC, 0f, 0f);
            rb.rotation = rotationCounterClock;
        }
        */
        if(!Input.anyKeyDown && rb.velocity.magnitude == 0){
            //reset rotation
            child_trans.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            currentRotation = 0; //reset
        }
    }
}
