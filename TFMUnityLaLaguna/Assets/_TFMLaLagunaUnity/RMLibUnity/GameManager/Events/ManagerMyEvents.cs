using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ManagerMyEvents 
{
    //private Dictionary<object, CompoDictionary<string,object>> events_;
    private Dictionary<object, CompositeData> events_;
    public static  ManagerMyEvents instance_;



    ~ManagerMyEvents()  
    {
        Debug.Log("Destruida instancia ManagerMyEvents desde destructor");
        Debug.Log("///////////////// destruyendo  todos los eventos ////////////////");
        foreach(var targetEvents in events_.Values)
            foreach (MyEventsBase e in targetEvents.GetAll().Values)           
                e.RemoveAllListeners();
    
    } 

    private ManagerMyEvents()
    {
        
        Debug.Log("Events: Iniciado instancia desde contructor ManagerMyEvents" );
          

          events_ = new Dictionary<object, CompositeData>();  

                    

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
        Debug.Log("Events: Suscribed on  ++++++++++++++++++++ " + eventName );
                
        if (target == null)
            target = this;
        
        if (!events_.ContainsKey(target))
            events_.Add(target, new CompositeData("Mis Eventos"));        
        
        
        if (events_[target].GetIfExistValue(eventName))
        {
            Debug.Log("Events: Trigger actualizado");
            events_[target].Get<MyEvents>(eventName).AddListener(listener);            
            
        }
        else
        {
            Debug.Log("Events: Trigguer nuevo");
            MyEvents e = new MyEvents();
            e.AddListener(listener);
            events_[target].Set<MyEvents>(eventName,e);            

        } 
        
        return true;
    }
    public  bool StopListening(string eventName, MyEvents.Delegate_ listener)
    {
        return StopListening(null, eventName,listener);
    }
    public bool StopListening(object target, string eventName, MyEvents.Delegate_ listener)
    {
        
        Debug.Log("Events: Unsuscribe on ------------------------ " + eventName );

        if (target == null)
            target = this;

        if (events_.ContainsKey(target))        
            if (events_[target].GetIfExistValue(eventName))
            {
                events_[target].Get<MyEvents>(eventName).RemoveListener(listener);
                return true;
            }
                


        return false; ///si algo salió mal, o sea, seguramente no se pudo eliminar, detener el evento, posiblemente porque no se encontró.
       
    }
    public  bool TriggerEvent(string eventName)
    {           
        Debug.Log("Events: Trigger Lanzado **************** " + eventName );
        bool success = false;        
        
        if (events_.ContainsKey(this))
            if (events_[this].GetIfExistValue(eventName))
                success = events_[this].Get<MyEvents>(eventName).Invoke();
        return success;

    }
    public  bool TriggerEvent(object target,  string eventName)
    {
        Debug.Log("Events: Trigger Lanzado **************** " + eventName );
        bool success = false;
        
        if (target == null)
            target = this;

        if (events_.ContainsKey(target))
            if (events_[target].GetIfExistValue(eventName))
                success = events_[target].Get<MyEvents>(eventName).Invoke();
                
        return success;
    }
    public  bool StartListening<T>(string eventName,MyEvents<T>.Delegate_ listener)
    {
        return StartListening<T>(null, eventName,listener);
    }
    public  bool StartListening<T>(object target,string eventName,MyEvents<T>.Delegate_ listener)
    { 
      Debug.Log("Events: Suscribed on  ++++++++++++++++++++ " + eventName );
                
        if (target == null)
            target = this;
        
        if (!events_.ContainsKey(target))
            events_.Add(target, new CompositeData("Mis Eventos"));        
        
        
        if (events_[target].GetIfExistValue(eventName))
        {
            Debug.Log("Events: Trigger actualizado");
            events_[target].Get<MyEvents<T>>(eventName).AddListener(listener);            
            
        }
        else
        {
            Debug.Log("Events: Trigguer nuevo");
            MyEvents<T> e = new MyEvents<T>();
            e.AddListener(listener);
            events_[target].Set<MyEvents<T>>(eventName,e);            

        } 
        
        return true;
    }

    public bool  StopListening<T>(string eventName, MyEvents<T>.Delegate_ listener)
    {
        return StopListening<T>(null, eventName,listener);
    }
    public  bool  StopListening<T>(object target, string eventName, MyEvents<T>.Delegate_ listener)
    {

        Debug.Log("Events: Unsuscribe on ------------------------ " + eventName );

        if (target == null)
            target = this;

        if (events_.ContainsKey(target))        
            if (events_[target].GetIfExistValue(eventName))
            {
                events_[target].Get<MyEvents<T>>(eventName).RemoveListener(listener);
                return true;
            }
                


        return false; ///si algo salió mal, o sea, seguramente no se pudo eliminar, detener el evento, posiblemente porque no se encontró.
       
    }
    public  DataExitReturned TriggerEvent<T>(string eventName,T data)
    {   
        return TriggerEvent<T>(null,eventName,data);

    }
    public  DataExitReturned TriggerEvent<T>(object target,  string eventName, T data)
    {
        Debug.Log("Events: Trigger Lanzado **************** " + eventName );        
        DataExitReturned valueToReturn = new DataExitReturned();   

        if (target == null)
            target = this;

        if (events_.ContainsKey(target))
            if (events_[target].GetIfExistValue(eventName))
                events_[target].Get<MyEvents<T>>(eventName).Invoke(data,valueToReturn);
                
     
        return valueToReturn;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    public void OnDestroy() ///para simular el destructor de Unity, aunque es invocado desde GameManager.
    {
        Debug.Log("Events: ///////////////// destruyendo  todos los eventos desde OnDestroy de ManagerMyEvents ////////////////");
        foreach(var targetEvents in events_.Values)
            foreach (MyEventsBase e in targetEvents.GetAll().Values)           
                e.RemoveAllListeners();
                            
    }

  
}
