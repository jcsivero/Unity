using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_GamePlay : MonoBehaviour
{
    private bool suscribeToEventUpdateHudOnScreen = false;
    
    [SerializeField] public Text  textHealthPlayer_;
       

    private const string EVENT_UPDATE_HUD_ONSCREEN = "EVENT_UPDATE_HUD_ONSCREEN";
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        suscribeToEventUpdateHudOnScreen = GameManagerMyEvents.StartListening<StatusHud>(EVENT_UPDATE_HUD_ONSCREEN,UpdateHudOnScreen);
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GameManagerMyEvents.StopListening<StatusHud>(EVENT_UPDATE_HUD_ONSCREEN,UpdateHudOnScreen);
      suscribeToEventUpdateHudOnScreen = false;
      
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
         if (!suscribeToEventUpdateHudOnScreen)
            OnEnable(); 
            
        

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
    }

    void UpdateHudOnScreen(StatusHud status, EventDataReturned valueToReturn)
    {
      textHealthPlayer_.text = status.GetHealth().ToString();
      
    }


}