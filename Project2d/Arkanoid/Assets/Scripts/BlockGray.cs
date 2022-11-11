using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGray : MonoBehaviour
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    //private bool eventsSuscribed = false;
    //private const string EVENT_COLLISION_KID = "EVENT_COLLISION_KID";
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
        
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        
    }

    void OnCollisionEnter2D()
    {

    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();

    }
}
