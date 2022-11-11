using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockViolet : MonoBehaviour
{
    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    
    void OnEnable()
    {
                
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
                            .Set<int>("addPoints",10)
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
