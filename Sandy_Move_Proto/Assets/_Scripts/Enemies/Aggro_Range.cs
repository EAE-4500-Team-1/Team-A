using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro_Range : MonoBehaviour
{
    public Crab parent;
    public GameObject FOV_Range;
    public GameObject Aggro_Radius;
    private float aggroDistance;

    // Start is called before the first frame update
    void Start()
    {
        aggroDistance = Aggro_Radius.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            parent.isInAggroRange = true;
            parent.Player = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            print(Vector3.Distance(transform.position, other.transform.position));
            if (Vector3.Distance(transform.position, other.transform.position) <= aggroDistance)
            {
                FOV_Range.SetActive(false);
                Aggro_Radius.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            parent.isInAggroRange = false;
            parent.Player = null;
        }
    }
}
