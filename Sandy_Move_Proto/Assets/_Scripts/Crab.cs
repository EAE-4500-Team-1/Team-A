using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public bool isInAggroRange = false;
    public GameObject Player;
    public float minDistance;
    public float moveSpeed;
    public float turnSpeed;
    [HideInInspector]
    public enum AI_State{ Idle, Walking, Combat}
    public AI_State Current_State;
    private Animator animController;
    private Vector3 turnDirection;
    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        Current_State = AI_State.Idle;
        animController = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * minDistance;
        Debug.DrawRay(transform.position, forward, Color.red);
        if (Player != null)
        {
            Chase();
        }
        else
        {
            Current_State = AI_State.Idle;
        }
        animController.SetInteger("Current_State", (int) Current_State);
    }

    void Chase()
    {
        // If the Crab isn't within reach of the player, but is detected by the aggro_range collider move towards it. Otherwise it is close enough to attack.
        if (Vector3.Distance(transform.position, Player.transform.position) >= minDistance)
        {
            Current_State = AI_State.Walking;
            turnDirection = Player.transform.position - rigidBody.position;
            turnDirection.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), turnSpeed * Time.deltaTime);

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            turnDirection = Player.transform.position - rigidBody.position;
            turnDirection.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(turnDirection), turnSpeed * Time.deltaTime);
            Current_State = AI_State.Idle;
        }
    }
}
