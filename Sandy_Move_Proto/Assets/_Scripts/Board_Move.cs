﻿using System.Collections;
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
    public float MAX_VELOC = 30f; //This constant is used to determine the maximum movement speed in all directions punched in on the keyboard.
    public float ROTATE = 10f; //This constant is used to determine the rotate rate of the board.
    public float MAX_MAG = 30f; //This constant is used to determine the maximum magnitude of an object. 
    

    public bool controlEnable = true; //This constant is used to enable and disable controls when we are in the air for now.
   
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
            
    }

    //This is used for physics calculations.
    void FixedUpdate(){
        /*forwardVeloc = transform.forward * VELOC * Time.deltaTime;
        backwardVeloc = -transform.forward * VELOC * Time.deltaTime;     
        rightVeloc = transform.right * VELOC * Time.deltaTime;
        leftVeloc = -transform.right * VELOC * Time.deltaTime;
        */

        Move();
    }

    // A simple method whose entire purpose is to listen to the keyboard event handler to allow our board to move.
    void Move(){
        if(controlEnable){
        Vector3 currentVector = rb.velocity; //access the current vector that our board is traveling on.

        rb.AddForce((transform.forward * Input.GetAxis("Vertical") * VELOC)); //Testing vertical movements
        //rb.AddForce((transform.right * Input.GetAxis("Horizontal")  * VELOC)); //Test Horizontal.
        gameObject.transform.Rotate(new Vector3(0f, ROTATE * Input.GetAxis("Horizontal") * Time.deltaTime, 0f)); //Testing rotational movements.
        if(Input.GetAxis("Vertical") > -0.10f && Input.GetAxis("Vertical") < 0.10f){
            rb.AddForce((transform.right * Input.GetAxis("Horizontal") * VELOC * Time.deltaTime));
        }

        //TODO: Add rotational effects to this.

        //these two if statements handle forward motion and turns.
        /*
        if(Input.GetKey(KeyCode.A)){ 
            if(rb.velocity.magnitude < MAX_MAG){
                rb.velocity += leftVeloc;
            }
            gameObject.transform.Rotate(new Vector3(0f, -ROTATE * Time.deltaTime, 0f));
            print("ROTATE LEFT");
        }

        if(Input.GetKey(KeyCode.D)){
            //rb.velocity += new Vector3(VELOC, 0f, 0f);
            if(rb.velocity.magnitude < MAX_MAG){
                rb.velocity += rightVeloc;
            }
            gameObject.transform.Rotate(new Vector3(0f, ROTATE * Time.deltaTime, 0f));
            print("ROTATE RIGHT");
        }
        */
        } //Control enable close bracket.
        if(rb.velocity.magnitude == 0){
            //reset rotation
            child_trans.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            currentRotation = 0; //reset
        }
    }

    void CorrectFlip(){
        if(rb.rotation.z > 90){
            rb.transform.rotation = new Quaternion(rb.transform.rotation.x, rb.transform.rotation.y, 
            0f, rb.transform.rotation.w);
        }
    }

    //This code uses colliders to enable and disable controls until our boarder hits the ground.
    //This was commented out to permit for tricks, I believe.
    //void OnCollisionEnter(Collision collision){
    //    if(collision.gameObject.tag == "Ground"){
    //        controlEnable = true;
    //    }
    //}

    //void OnCollisionStay(Collision collision){
    //    if(collision.gameObject.tag == "Ground"){
    //        controlEnable = true;
    //    }
    //}

    //void OnCollisionExit(Collision collision){

    //    if(collision.gameObject.tag == "Ground"){
    //        controlEnable = false;
    //    }
    //}
}
