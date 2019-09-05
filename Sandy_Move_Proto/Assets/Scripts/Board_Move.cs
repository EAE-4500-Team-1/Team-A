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
    public float VELOC = .5f; //This constant is used to determine the movement speed in all directions punched in on the keyboard.
    public float ROTATE = 10f; //This constant is used to determine the rotate rate of the board. 
   
   //GameObject assignables: We use this area to track what elements of the game object we are changing.
   //The rigidbody (physics!) of the board. We use this to access the board's attributes.
    Rigidbody rb;
   
    //A slight rotation to the board in the clockwise rotation.
    Quaternion rotationClock; //Clockwise rotation.
    Vector3 rotationClockVeloc;

    //A slight rotation to the board in the counter clockwise rotation.
    Quaternion rotationCounterClock; //Counterclockwise rotation.
    Vector3 rotationCounterClockVeloc;
    // Start is called before the first frame update
    //We can use this for a lot of setup before the scene even starts.

    void Start()
    {   //since we can't do static assignments for gameobject members, we have to assign them here.
        rb = gameObject.GetComponent<Rigidbody>(); //The rigid body for the board, assigned on start of game.
        //Rotational assignments.
        rotationClockVeloc = new Vector3(0f, 0f, ROTATE);
        rotationCounterClockVeloc = new Vector3(0f, 0f, ROTATE * -1);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //This is used for physics calculations.
    void FixedUpdate(){
        Quaternion rotationClock = Quaternion.Euler(rotationClockVeloc * Time.deltaTime);
        Quaternion rotationCounterClock = Quaternion.Euler(rotationCounterClockVeloc * Time.deltaTime);
    }

    // A simple method whose entire purpose is to listen to the keyboard event handler to allow our board to move.
    void Move(){
        Vector3 currentVector = rb.velocity; //access the current vector that our board is traveling on.
        
        if(Input.GetKey(KeyCode.W)){
            
            rb.velocity += new Vector3(0.0f, 0.0f, VELOC);
            print("MoveForward!");
        }

        if(Input.GetKey(KeyCode.S)){
            rb.velocity -= new Vector3(.0f, .0f, VELOC);
            print("MoveBack!");
        }

        //TODO: Add rotational effects to this.
        if(Input.GetKey(KeyCode.A)){ 
            rb.velocity -= new Vector3(VELOC, 0f, 0f);
            if(rb.rotation.z >= -10f){
                rb.MoveRotation(rb.rotation * rotationCounterClock);
            }
        }

        if(Input.GetKey(KeyCode.D)){
            rb.velocity += new Vector3(VELOC, 0f, 0f);
            if(rb.rotation.z <= 10f){
                rb.MoveRotation(rb.rotation * rotationClock);
            }
        }
        if(!Input.anyKeyDown){
            //reset rotation
            if(rb.rotation.z > 1){
                rb.MoveRotation(rb.rotation * rotationCounterClock);
            }
        
            if(rb.rotation.z < -1){
                rb.MoveRotation(rb.rotation * rotationClock);
            }
        }
    }
}
