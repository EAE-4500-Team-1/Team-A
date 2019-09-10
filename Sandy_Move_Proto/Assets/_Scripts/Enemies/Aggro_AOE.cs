using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro_AOE : MonoBehaviour
{
    public Crab parent;
    public GameObject FOV_Range;
    public GameObject Aggro_Radius;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            parent.isInAggroRange = true;
            parent.Player = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FOV_Range.SetActive(true);
            Aggro_Radius.SetActive(false);
        }
    }
}
