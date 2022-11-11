using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGreen : MonoBehaviour
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    //private bool eventsSuscribed = false;
    //private const string EVENT_COLLISION_KID = "EVENT_COLLISION_KID";
    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";    

    void OnEnable()
    {
                
      //  eventsSuscribed = GameManagerMyEvents.StartListening(this.gameObject,EVENT_COLLISION_KID,CollisionKid); //por si se ejecuta este OnEnable antes que el awake de la clase GameManagerEvents        
       // Debug.Log(eventsSuscribed);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameManagerMyEvents.TriggerEvent<EventData>(EVENT_UPDATE_HUD_VALUES,EventData.Create(EVENT_UPDATE_HUD_VALUES)                            
                    .Set<int>("addOrSubEnemy",1));                       
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        
    }

    void OnCollisionEnter2D()
    {
        GameManagerMyEvents.TriggerEvent<EventData>(EVENT_UPDATE_HUD_VALUES,EventData.Create(EVENT_UPDATE_HUD_VALUES)
                            .Set<int>("addPoints",5)
                            .Set<int>("addOrSubEnemy",-1));
                            
                                                        
        Destroy(this.gameObject);
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();

    }
}
