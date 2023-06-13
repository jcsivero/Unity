using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
  

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected " + gameObject.name + " contra  " + other.gameObject.name);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected " + gameObject.name + " contra  " + collision.gameObject.name);
    }


}
