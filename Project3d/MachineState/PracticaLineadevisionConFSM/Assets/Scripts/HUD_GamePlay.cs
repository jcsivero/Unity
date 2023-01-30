using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_GamePlay : MonoBehaviour
{
    private bool suscribeToEvents = false;
    
    [SerializeField] public Text  textHealthPlayer_;

    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        suscribeToEvents = GameManagerMyEvents.StartListening(this.gameObject,EVENT_UPDATE_HUD_VALUES,UpdateHudValues);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
         if (!suscribeToEvents)
            OnEnable(); 
        GameObject draft = GameObject.Find("MyLife");
        textHealthPlayer_ = draft.GetComponent<Text>();
        
        UpdateHudValues();

    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GameManagerMyEvents.StopListening(EVENT_UPDATE_HUD_VALUES,UpdateHudValues);
      
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
    }

    bool UpdateHudValues()
    {
      textHealthPlayer_.text = GameManager.gameManager_.healthPlayer_.ToString();
      return true;
    }


}