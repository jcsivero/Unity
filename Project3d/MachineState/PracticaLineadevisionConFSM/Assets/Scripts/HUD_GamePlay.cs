using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_GamePlay : MonoBehaviour
{
    private bool suscribeToEventUpdateHudValues = false;
    
    [SerializeField] public Text  textHealthPlayer_;
       

    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        suscribeToEventUpdateHudValues = GameManagerMyEvents.StartListening<StatusHud>(EVENT_UPDATE_HUD_VALUES,UpdateHudValues);
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GameManagerMyEvents.StopListening<StatusHud>(EVENT_UPDATE_HUD_VALUES,UpdateHudValues);
      suscribeToEventUpdateHudValues = false;
      
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
         if (!suscribeToEventUpdateHudValues)
            OnEnable(); 
            
        

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
    }

    void UpdateHudValues(StatusHud status, EventDataReturned valueToReturn)
    {
      textHealthPlayer_.text = status.GetHealth().ToString();
      
    }


}