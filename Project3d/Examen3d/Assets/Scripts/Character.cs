using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] public bool collision_;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("posicion actual en trigguer ++++++++++++++++++++++++++: " +transform.position);
        //Debug.Log("onTrigger enter");                
        collision_ = true;
        
        GameEvents.TriggerOnActionGameObject(this.gameObject,other.gameObject);
        
        
    }
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("saliendo del tria trigguer ???????????????????????????: " +transform.position);
        collision_ = false;
    }
}


