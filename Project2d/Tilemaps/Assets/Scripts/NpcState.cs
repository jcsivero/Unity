using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("colision");
        other.gameObject.GetComponent<Animator>().SetTrigger("attack");
        gameObject.GetComponent<Animator>().SetTrigger("dead");
        //Destroy(gameObject);
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
               Debug.Log("trigger");
        other.gameObject.GetComponent<Animator>().SetTrigger("attack");
        gameObject.GetComponent<Animator>().SetTrigger("dead");
        //Destroy(gameObject);
    }
}
