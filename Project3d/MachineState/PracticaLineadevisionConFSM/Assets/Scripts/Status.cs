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
    public  string GetName()
    {
        return name_;
    }

     public  GameObject GetOrigin()
     {
        return origin_;
     }    
     public  GameObject GetTarget()
     {
        return target_;
     }  

     public  float GetHealth()
     {
        return health_;
     }    
     public  int  GetLifes()
     {
        return lifes_;
     }  

     public  void SetName(string draft)
     {
        name_ = draft;
     }  

     public  void SetTarget(GameObject target)
     {
        target_ = target;
     }  

    public void  SetOrigin(GameObject draft)
    {
        origin_ = draft;
    }
    public void  SetHealth(float draft)
    {
        health_ = draft;
    }

    public void  SetLifes(int draft)
    {
        lifes_ = draft;
    }
    private void InstaciateCommands()
    {
                
        commandAddOrSubHealth_ = new CommandAddOrSubHealth(this);
        commandAddOrSubLifes_ = new CommandAddOrSubLifes(this);

    }
 
    public virtual UnityEngine.AI.NavMeshAgent GetAgentNavMesh()
    {
        return null;
    }    

    public  void Awake()
    {
        InstaciateCommands();
        origin_ = gameObject;
        Debug.Log("|||||||||||||| Awake Status||||||||||||||||");

    }

   
    public void Start()
    {
        Debug.Log("|||||||||||||| Start Status||||||||||||||||");
      
        
    }

    
 
}

