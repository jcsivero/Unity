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


    [SerializeField] public  float rotationSpeed_ = 2.0f;

    [SerializeField] public  float speedInitial_ = 2.0f;
    [SerializeField] public  float speedMax_ = 2.0f;

    [SerializeField] public  float currentSpeed_;    
       

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    [SerializeField] private GameObject origin_;
     [SerializeField] private GameObject target_;     
    [SerializeField] private bool updateHud_;
    [SerializeField] private int lifes_;
    [SerializeField] private float health_;
    [SerializeField] private Vector3 positionPreviousFrame_;


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

     public Status GetTargetStatus()
     {
        return this;
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


  
    public float GetCurrentSpeed()
    {
        ////Metodo propio, es una sola variable de velocidad independiente de Navmesh, CharacterController o cualqueir otro método.
        ///Se basa en la magnitud de la diferencia de posición  en la pantalla en valor absoluto, dividido entre el tiempo de cada frame.

        currentSpeed_  = Mathf.Abs((transform.position - positionPreviousFrame_).magnitude/Time.deltaTime);  
        

        return currentSpeed_; ///si se usa cálculos de movimiento, recuerda que el verdadero valor es multiplicado por Time.deltaTime, sino es un valor
        ///muy grande
    }

    virtual public float GetSpeedMax()
    {
        return speedMax_;        

    }

    virtual public void SetSpeedMax(float speed)
    {
        speedMax_ = speed;        

    }

 public float GetSpeedInitial()
    {
       return speedInitial_;

    }
    virtual public float MovementValue()
    {
        return speedMax_ * Time.deltaTime; ////puedo poner también por valor de movimiento del ratón, ejes...
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
        positionPreviousFrame_ = transform.position;
      
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected void Update()
    {
        if (GetGameManager().ok_)
        {
            positionPreviousFrame_ = transform.position;  ///variable necesaria  para calcular la velocidad
            GetCurrentSpeed();
        }
        
    }


}


