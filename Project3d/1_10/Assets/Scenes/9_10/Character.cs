using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] public Text text_;
    [SerializeField] public int points_;
    [SerializeField] public bool collision_;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (text_ == null)
            text_ = GameObject.Find("Message").GetComponent<Text>();
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>

    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("posicion actual en trigguer ++++++++++++++++++++++++++: " +transform.position);
        //Debug.Log("onTrigger enter");
        points_ += 5;
        other.gameObject.GetComponent<Renderer>().material.color = Color.red;
        text_.text = "Puntos totales: " + points_ + "Última colisión con: " + other.gameObject.name;
        collision_ = true;
        
        
    }
    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("saliendo del tria trigguer ???????????????????????????: " +transform.position);
        collision_ = false;
    }
}


