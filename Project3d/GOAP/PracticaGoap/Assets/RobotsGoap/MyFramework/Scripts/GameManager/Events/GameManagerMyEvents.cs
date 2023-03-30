using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ManagerMyEvents 
{
    private Dictionary<object, Dictionary<string,object>> events_;
    public static  ManagerMyEvents instance_;



    ~ManagerMyEvents()  
    {
        Debug.Log("Destruida instancia ManagerMyEvents desde destructor");
        Debug.Log("///////////////// destruyendo  todos los eventos ////////////////");
        foreach(var targetEvents in events_.Values)
            foreach (MyEventsBase e in targetEvents.Values)           
                e.RemoveAllListeners();
                            
    
    } 

    private ManagerMyEvents()
    {
        
        Debug.Log("Iniciado instancia desde contructor ManagerMyEvents" );
          

          events_ = new Dictionary<object, Dictionary<string,object>>();  

                    

    }
     public static ManagerMyEvents Instance()
    {
         if (instance_ == null)
            instance_ = new ManagerMyEvents();
         return instance_;
    }


    
    public  bool StartListening(string eventName,MyEvents.Delegate_ listener)
    {
        return StartListening(null, eventName,listener);
    }

    public  bool StartListening(object target,string eventName,MyEvents.Delegate_ listener)
    {
        Debug.Log("Suscribed on  ++++++++++++++++++++ " + eventName );
        MyEvents e = null;
        
        if (target == null)
            target = this;
        

        if (!events_.ContainsKey(target))
            events_.Add(target, new Dictionary<string, object>());
        
        Dictionary<string, object> targetEvents = events_[target];


        if (targetEvents.ContainsKey(eventName))
        {
            e = (MyEvents) targetEvents[eventName];
            //Debug.Log("agrego trigguer actualizo");
            e.AddListener(listener);
            
        } 
        else
        {
            //Debug.Log("agrego trigguer nuevo");
            e = new MyEvents();
            e.AddListener(listener);
            targetEvents.Add(eventName,e);
        }




        return true;
    }
    public  void StopListening(string eventName, MyEvents.Delegate_ listener)
    {
        StopListening(null, eventName,listener);
    }
    public  void StopListening(object target, string eventName, MyEvents.Delegate_ listener)
    {
        
        Debug.Log("Unsuscribe on ------------------------ " + eventName );
        MyEvents e = null;
        Dictionary<string, object> targetEvents;
        
        if (target == null)
            target = this;

        if (events_.TryGetValue(target, out targetEvents) && 
            (targetEvents.ContainsKey(eventName)))
        {
            e = (MyEvents) targetEvents[eventName];
            e.RemoveListener(listener);
        } 
       
    }
    public  bool TriggerEvent(string eventName)
    {   
        Debug.Log("Trigger **************** " + eventName );
        MyEvents e = null;
        Dictionary<string, object> targetEvents;
        bool success = false;        
        object  target = this;

        if (events_.ContainsKey(target))
        {
            targetEvents = events_[target];
            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents) targetEvents[eventName];
                success =  e.Invoke();

            }

        }                  
        return success;

    }
    public  bool TriggerEvent(object target,  string eventName)
    {
        Debug.Log("Trigger **************** " + eventName );
        MyEvents e = null;
        Dictionary<string, object> targetEvents;
        bool success = false;
        if (target == null)
            target = this;

        if (events_.ContainsKey(target))
        {
            targetEvents = events_[target];
            if (targetEvents.ContainsKey(eventName))
            {
                e = (MyEvents) targetEvents[eventName];
                success =  e.Invoke();

            }

        }                  
        return success;
    }
    public  bool StartListening<T>(string eventName,MyEvents<T>.Delegate_ listener)
    {
        return StartListening<T>(null, eventName,listener);
    }
    public  bool StartListening<T>(object target,string eventName,MyEvents<T>.Delegate_ listener)
    { 
        Debug.Log("Suscribed on +++++++++++++++++++++ " + eventName );
        MyEvents<T> e = null;
        
        if (target == null)
            target = this;
        

        if (!events_.ContainsKey(target))
            events_.Add(target, new Dictionary<string, object>());
        
        Dictionary<string, object> targetEvents = events_[target];


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


        return true;
    }

    public void StopListening<T>(string eventName, MyEvents<T>.Delegate_ listener)
    {
        StopListening<T>(null, eventName,listener);
    }
    public  void StopListening<T>(object target, string eventName, MyEvents<T>.Delegate_ listener)
    {
        Debug.Log("Unsuscribe on ------------------- " + eventName );
        MyEvents<T> e = null;
        Dictionary<string, object> targetEvents;
        
        if (target == null)
            target = this;

        if (events_.TryGetValue(target, out targetEvents) && 
            (targetEvents.ContainsKey(eventName)))
        {
            e = (MyEvents<T>) targetEvents[eventName];
            e.RemoveListener(listener);
        } 
       
    }
    public  DataExitReturned TriggerEvent<T>(string eventName,T data)
    {   
        return TriggerEvent<T>(null,eventName,data);

    }
    public  DataExitReturned TriggerEvent<T>(object target,  string eventName, T data)
    {
        Debug.Log("Trigger **************** " + eventName );
        MyEvents<T> e = null;
        DataExitReturned valueToReturn = new DataExitReturned();

        Dictionary<string, object> targetEvents;

        if (target == null)
            target = this;

        if (events_.ContainsKey(target))
        {
            targetEvents = events_[target];
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
    public void OnDestroy() ///para simular el destructor de Unity, aunque es invocado desde GameManager.
    {
        Debug.Log("///////////////// destruyendo  todos los eventos ////////////////");
        foreach(var targetEvents in events_.Values)
            foreach (MyEventsBase e in targetEvents.Values)           
                e.RemoveAllListeners();
                            
    }

  
}
