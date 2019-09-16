using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public bool isInAggroRange = false;
    public bool grabbedPlayer;

    public GameObject Aggro_Target;
    public GameObject Claw_Pos;
    public GameObject Grabbed_Target;

    public float minDistance;
    public float moveSpeed;
    public float turnSpeed;
    public float throwDistance;
    public float throwHeight;
    public float grabTargetTime;

    public AI_State Current_State;

    [HideInInspector]
    public enum AI_State { Idle, Walking, Throwing } // State machines for the different phases the crab is in.
    private Animator animController;
    private Vector3 turnDirection;
    private Rigidbody rigidBody;
    private System.DateTime grabStartTime;

    // Start is called before the first frame update
    void Start()
    {
        Current_State = AI_State.Idle;
        animController = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Used for calclulating the current crab state, distance from player, etc.
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * minDistance;
        Debug.DrawRay(transform.position, forward, Color.red);

        if (Aggro_Target != null)
        {
            if (grabbedPlayer)
            {
                Grabbed_Target.transform.position = Claw_Pos.transform.position;
                Current_State = AI_State.Throwing;
            }
            else
            {
                Current_State = AI_State.Walking;
            }

        }
        else
        {
            Current_State = AI_State.Idle;
        }
        
    }

    // For handling the different states the crab is in and calling the appropriate animation / actions.
    private void FixedUpdate()
    {
        switch (Current_State)
        {
            case AI_State.Idle:
                Idle();
                break;
            case AI_State.Walking:
                Chase();
                break;
            case AI_State.Throwing:
                Throw(Grabbed_Target);
                break;
        }
        // Updates the animator, make sure states are in order. Ints are set to the corresponding index in the AI_State initializer.
        animController.SetInteger("Current_State", (int)Current_State); 
    }

    void Idle()
    {
        //TO DO maybe in a future build make a patrol type action.
    }

    // If the Crab isn't within reach of the player, but is detected by the aggro_range collider move towards it. Otherwise it is close enough to attack.
    // Gameobject Player is assigned when the player is in range of the aggro colliders inside the crab's hiearchy FOV_Range and Aggro Radius
    void Chase()
    {
        
        Current_State = AI_State.Walking;
        turnDirection = Aggro_Target.transform.position - rigidBody.position;
        turnDirection.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), turnSpeed * Time.deltaTime);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }

    // Calculates the time from when the crab grabbed the player and throws him after the grab target time has been passed.
    void Throw(GameObject thrownObject)
    {
        if ((System.DateTime.UtcNow - grabStartTime).TotalSeconds >= grabTargetTime){
            grabbedPlayer = false;
            Rigidbody thrownObjectRB = thrownObject.GetComponent<Rigidbody>();
            float throwForce = transform.forward.z * throwDistance;
            thrownObjectRB.AddForce(new Vector3(0, throwHeight, throwForce));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Grabs the player, sets the grab start time.
        if (collision.gameObject.tag == "Player")
        {
            grabbedPlayer = true;
            Grabbed_Target = collision.gameObject;
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            grabStartTime = System.DateTime.UtcNow;
        }
    }
}
