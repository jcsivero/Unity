using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class  Status :  BaseMono
{
    public GameObject origin_;
     public GameObject target_;
    public bool updateHud_;
    public int lifes_;
    public float health_;


    
    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    private const string EVENT_UPDATE_STATUS_WORLD = "EVENT_UPDATE_STATUS_WORLD";    
        
    public CommandAddOrSubHealth commandAddOrSubHealth_; ///comandos comunes
    public CommandAddOrSubLifes commandAddOrSubLifes_; ///comandos comunes
    
    public abstract string GetName();

    virtual public  GameObject GetOrigin()
     {
        return origin_;
     }    
    virtual public  GameObject GetTarget()
     {
        return target_;
     }  

    virtual public  void SetTarget(GameObject target)
     {
        target_ = target;
     }  

    public virtual void InstaciateCommands()
    {
        
        commandAddOrSubHealth_ = new CommandAddOrSubHealth(this);
        commandAddOrSubLifes_ = new CommandAddOrSubLifes(this);

    }
    virtual public void  SetOrigin(GameObject draft)
    {
        origin_ = draft;
    }

    public virtual  UnityEngine.AI.NavMeshAgent GetAgentNavMesh()
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
       SetTarget(GetStatusWorld().GetTarget());
        
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    /* public abstract int GetLifes();
     public abstract float GetHealth();
     

    public abstract void SetName(string draft);
    public abstract void SetHealth(float draft);
    public abstract void SetLifes(int draft);

    public abstract bool ExecutionTasks();

      virtual public  bool GetDelete(){
        return false;
     }

    virtual public void SetDelete(bool draft)
    {

    }
    virtual public  bool GetUpdateHud(){
        return updateHud_;
     }

    virtual public void SetUpdateHud(bool draft)
    {
        updateHud_ = draft;
    }*/

}

