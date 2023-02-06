using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class  Status :  BaseMono
{
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    public string name_ = "Status";
    public CommandAddOrSubHealth commandAddOrSubHealth_; ///comandos comunes
    public CommandAddOrSubLifes commandAddOrSubLifes_; ///comandos comunes


 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    [SerializeField] private GameObject origin_;
     [SerializeField] private GameObject target_;
    [SerializeField] private bool updateHud_;
    [SerializeField] private int lifes_;
    [SerializeField] private float health_;



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    private const string ON_UPDATE_ALL_STATUS = "ON_UPDATE_ALL_STATUS";
    private bool suscribeToOnUpdateAllStatus_ = false;

    public  string GetName()
    {
        return name_;
    }

    virtual public  GameObject GetOrigin()
     {
        return origin_;
     }    
    virtual public  GameObject GetTarget()
     {
        return target_;
     }  

    virtual public  float GetHealth()
     {
        return health_;
     }    
    virtual public  int  GetLifes()
     {
        return lifes_;
     }  

     public  void SetName(string draft)
     {
        name_ = draft;
     }  

    virtual public  void SetTarget(GameObject target)
     {
        target_ = target;
     }  

   virtual public void  SetOrigin(GameObject draft)
    {
        origin_ = draft;
    }
   virtual public void  SetHealth(float draft)
    {
        health_ = draft;
    }

    virtual public void  SetLifes(int draft)
    {
        lifes_ = draft;
    }
    public virtual void InstaciateCommands()
    {
        
        commandAddOrSubHealth_ = new CommandAddOrSubHealth(this);
        commandAddOrSubLifes_ = new CommandAddOrSubLifes(this);

    }
 
    public virtual UnityEngine.AI.NavMeshAgent GetAgentNavMesh()
    {
        return null;
    }    

    public  virtual void Awake()
    {
        InstaciateCommands();
        origin_ = gameObject;

    }

   
    public virtual void Start()
    {
        if (!suscribeToOnUpdateAllStatus_)
            OnEnable(); 
       
        
    }

       // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    public virtual void OnEnable()   
    {
        if (!suscribeToOnUpdateAllStatus_) 
            suscribeToOnUpdateAllStatus_ = GetManagerMyEvents().StartListening(ON_UPDATE_ALL_STATUS,OnUpdateStatus); ///creo evento para actualizar  todos los StatusNpcRobots.
        ///Este evento es lanzado por GameManager,cuando ha actualizado todas las variables iniciales del estado del mundo.
        ///Después se puede utilizar para informar a todos los objetos a la vez y que se actualizen.
        ///Esto no lo hago directamente en el Start() porque no sabemos en que orden son ejecutados,y podría haber Start() que se ejecutan antes que el 
        ///Start() del GameManager, o del StatusWorld, , y entonces no tener todo actualizado, como target_ u otras variables.


        
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    public virtual void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger " +ON_UPDATE_ALL_STATUS);
      GetManagerMyEvents().StopListening(ON_UPDATE_ALL_STATUS,OnUpdateStatus);
      suscribeToOnUpdateAllStatus_ = false;
      
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
    }
    bool OnUpdateStatus()
    {
        
        return true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GetGameManager().ok_)
        {
            
        }
    }


}

