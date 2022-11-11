using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataEvent : UnityEvent<EventData>{}
public class GameManagerEventsUnity : MonoBehaviour
{
    public UnityEvent OnStart;
    public Spin spin;

    private Dictionary<GameObject, Dictionary<string,DataEvent>> events_;
    public static GameManagerEventsUnity gameManagerEventsUnity_;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        if ((gameManagerEventsUnity_ != null) && (gameManagerEventsUnity_ != this ))
        {
            Destroy(gameObject);
        }
        else
        {
            gameManagerEventsUnity_ = this;            
            events_ = new Dictionary<GameObject, Dictionary<string,DataEvent>>();
        }
            
    }
    
    public static bool StartListening(string eventName,UnityAction<EventData> listener)
    {
        return StartListening(null, eventName,listener);
    }
    public static bool StartListening(GameObject target,string eventName,UnityAction<EventData> listener)
    { 
        bool ok = false;
        
        if (gameManagerEventsUnity_ != null)
        {
            DataEvent e = null;
            
            if (target == null)
                target = gameManagerEventsUnity_.gameObject;
            

            if (!gameManagerEventsUnity_.events_.ContainsKey(target))
                gameManagerEventsUnity_.events_.Add(target, new Dictionary<string, DataEvent>());
            
            Dictionary<string, DataEvent> targetEvents = gameManagerEventsUnity_.events_[target];


            if (targetEvents.TryGetValue(eventName, out e))
            {
                Debug.Log("agrego trigguer actualizo");
                e.AddListener(listener);
                
            } 
            else
            {
                Debug.Log("agrego trigguer nuevo");
                e = new DataEvent();
                e.AddListener(listener);
                targetEvents.Add(eventName,e);
            }

            ok = true;

        }

        return ok;
    }

    public static void StopListening(string eventName, UnityAction<EventData> listener)
    {
        StopListening(null, eventName,listener);
    }
    public static void StopListening(GameObject target, string eventName, UnityAction<EventData> listener)
    {
        DataEvent e = null;
        Dictionary<string, DataEvent> targetEvents;
        
        if (target == null)
            target = gameManagerEventsUnity_.gameObject;

        if (gameManagerEventsUnity_.events_.TryGetValue(target, out targetEvents) && 
            (targetEvents.TryGetValue(eventName,out e)))
        {
            e.RemoveListener(listener);
        } 
       
    }
    public static void TriggerEvent( EventData data)
    {   
        TriggerEvent(null,data);

    }
    public static void TriggerEvent(GameObject target, EventData data)
    {
        DataEvent e = null;
        Dictionary<string, DataEvent> targetEvents;

        if (target == null)
            target = gameManagerEventsUnity_.gameObject;

        if (gameManagerEventsUnity_.events_.TryGetValue(target, out targetEvents) &&
            (targetEvents.TryGetValue(data.name_, out e)))
        {
            e.Invoke(data);
        } 

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        foreach(var targetEvents in gameManagerEventsUnity_.events_.Values)
            foreach (var e in targetEvents.Values)
            {
            e.RemoveAllListeners();
            }
    }

    // Start is called before the first frame update
    /*void Start()
    {
        //Call toggle spin
        //spin = GameObject.Find("Cube").GetComponent<Spin>();
        //spin.ToggleSpin();
        if (OnStart != null)
            OnStart?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
