using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseTrigger : MonoBehaviour
{
    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger enter");
        
        GameManagerMyEvents.TriggerEvent<EventData>(EVENT_UPDATE_HUD_VALUES,EventData.Create(EVENT_UPDATE_HUD_VALUES)
                   .Set<bool>("lost",true));     

    }
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("colision enter");
        GameManagerMyEvents.TriggerEvent<EventData>(EVENT_UPDATE_HUD_VALUES,EventData.Create(EVENT_UPDATE_HUD_VALUES)
            .Set<bool>("lost",true));     
     
    }
   

}
