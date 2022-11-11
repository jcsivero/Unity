using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManagerMyEvents : MonoBehaviour
{
    private Dictionary<GameObject, Dictionary<string,object>> events_;
    public static GameManagerMyEvents gameManagerMyEvents_;
     

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        if ((gameManagerMyEvents_ != null) && (gameManagerMyEvents_ != this ))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Creando instancia GameManagerMyEvents");
            gameManagerMyEvents_ = this;            
            events_ = new Dictionary<GameObject, Dictionary<string,object>>();            
        }
            
    }
    
    public static bool StartListening(string eventName,MyEvents.Delegate_ listener)
    {
        return StartListening(null, eventName,listener);
    }

    public static bool StartListening(GameObject target,string eventName,MyEvents.Delegate_ listener)
    {
       bool ok = false;
        
        if (gameManagerMyEvents_ != null)
        {
            MyEvents e = null;
            
            if (target == null)
                target = gameManagerMyEvents_.gameObject;
            

            if (!gameManagerMyEvents_.events_.ContainsKey(target))
                gameManagerMyEvents_.events_.Add(target, new Dictionary<string, object>());
            
            Dictionary<string, object> targetEvents = gameManagerMyEvents_.events_[target];


            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents) targetEvents[eventName];
                Debug.Log("agrego trigguer actualizo");
                e.AddListener(listener);
                
            } 
            else
            {
                Debug.Log("agrego trigguer nuevo");
                e = new MyEvents();
                e.AddListener(listener);
                targetEvents.Add(eventName,e);
            }

            ok = true;

        }

        return ok;
    }
    public static void StopListening(string eventName, MyEvents.Delegate_ listener)
    {
        StopListening(null, eventName,listener);
    }
    public static void StopListening(GameObject target, string eventName, MyEvents.Delegate_ listener)
    {
        MyEvents e = null;
        Dictionary<string, object> targetEvents;
        
        if (target == null)
            target = gameManagerMyEvents_.gameObject;

        if (gameManagerMyEvents_.events_.TryGetValue(target, out targetEvents) && 
            (targetEvents.ContainsKey(eventName)))
        {
            e = (MyEvents) targetEvents[eventName];
            e.RemoveListener(listener);
        } 
       
    }
    public static bool TriggerEvent(string eventName)
    {   
        
        MyEvents e = null;
        Dictionary<string, object> targetEvents;
        bool success = false;        
        GameObject target = gameManagerMyEvents_.gameObject;

        if (gameManagerMyEvents_.events_.ContainsKey(target))
        {
            targetEvents = gameManagerMyEvents_.events_[target];
            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents) targetEvents[eventName];
                success =  e.Invoke();

            }

        }                  
        return success;

    }
    public static bool TriggerEvent(GameObject target,  string eventName)
    {
        MyEvents e = null;
        Dictionary<string, object> targetEvents;
        bool success = false;
        if (target == null)
            target = gameManagerMyEvents_.gameObject;

        if (gameManagerMyEvents_.events_.ContainsKey(target))
        {
            targetEvents = gameManagerMyEvents_.events_[target];
            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents) targetEvents[eventName];
                success =  e.Invoke();

            }

        }                  
        return success;
    }
    public static bool StartListening<T>(string eventName,MyEvents<T>.Delegate_ listener)
    {
        return StartListening<T>(null, eventName,listener);
    }
    public static bool StartListening<T>(GameObject target,string eventName,MyEvents<T>.Delegate_ listener)
    { 
        bool ok = false;
        
        if (gameManagerMyEvents_ != null)
        {
            MyEvents<T> e = null;
            
            if (target == null)
                target = gameManagerMyEvents_.gameObject;
            

            if (!gameManagerMyEvents_.events_.ContainsKey(target))
                gameManagerMyEvents_.events_.Add(target, new Dictionary<string, object>());
            
            Dictionary<string, object> targetEvents = gameManagerMyEvents_.events_[target];


            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents<T>) targetEvents[eventName];
                Debug.Log("agrego trigguer actualizo");
                e.AddListener(listener);
                
            } 
            else
            {
                Debug.Log("agrego trigguer nuevo");
                e = new MyEvents<T>();
                e.AddListener(listener);
                targetEvents.Add(eventName,e);
            }

            ok = true;

        }

        return ok;
    }

    public static void StopListening<T>(string eventName, MyEvents<T>.Delegate_ listener)
    {
        StopListening<T>(null, eventName,listener);
    }
    public static void StopListening<T>(GameObject target, string eventName, MyEvents<T>.Delegate_ listener)
    {
        MyEvents<T> e = null;
        Dictionary<string, object> targetEvents;
        
        if (target == null)
            target = gameManagerMyEvents_.gameObject;

        if (gameManagerMyEvents_.events_.TryGetValue(target, out targetEvents) && 
            (targetEvents.ContainsKey(eventName)))
        {
            e = (MyEvents<T>) targetEvents[eventName];
            e.RemoveListener(listener);
        } 
       
    }
    public static EventDataReturned TriggerEvent<T>(string eventName,T data)
    {   
        return TriggerEvent<T>(null,eventName,data);

    }
    public static EventDataReturned TriggerEvent<T>(GameObject target,  string eventName, T data)
    {
        MyEvents<T> e = null;
        EventDataReturned valueToReturn = new EventDataReturned();

        Dictionary<string, object> targetEvents;

        if (target == null)
            target = gameManagerMyEvents_.gameObject;

        if (gameManagerMyEvents_.events_.ContainsKey(target))
        {
            targetEvents = gameManagerMyEvents_.events_[target];
            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents<T>) targetEvents[eventName];
                e.Invoke(data, valueToReturn);

            }

        } 
                 
        return valueToReturn;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        /*foreach(var targetEvents in gameManagerMyEvents_.events_.Values)
            foreach (MyEvents e in targetEvents.Values)           
                e.RemoveAllListeners();
                */
            
    }

  
}
