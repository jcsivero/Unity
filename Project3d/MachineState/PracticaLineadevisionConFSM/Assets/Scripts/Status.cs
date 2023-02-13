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

    
    [SerializeField] public  float speedMax_ = 2.0f;

    [SerializeField] public  float currentSpeed_;    
       

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    [SerializeField] private GameObject origin_;
     [SerializeField] private GameObject target_;     
    [SerializeField] private int lifes_;
    [SerializeField] private int health_;
    private Vector3 positionPreviousFrame_;


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

     public  int GetHealth()
     {
        return health_;
     }    

     public Status GetTargetStatus()
     {
        return GetTarget().GetComponent<Status>();
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
    public void  SetHealth(int draft)
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
 
    virtual public UnityEngine.AI.NavMeshAgent GetNavMeshAgent()
    {
        return null;
    }    

    virtual public bool SetNavMeshUse(bool navmesh) 
    {
        return false;
    }
    
    virtual public bool GetNavMeshUse()
    {
        return false;
    }
    virtual public UnityEngine.AI.NavMeshPath GetNavMeshPath()
    {
        return null;
    }
     virtual public int GetNavMeshPathCurrentIndex()
    {
        return 0;
    }
    virtual public void  SetNavMeshPathCurrentIndex(int index)
    {
        
    }
virtual public float GetBrakingDistance()
{
    return 0.0f;
}
virtual public  void SetBrakingDistance(float distance)
{
    
}
virtual public void SetNavMeshTargetPosition(Vector3 pos)
{

}
virtual public Vector3 GetNavMeshTargetPosition()
{
    return Vector3.zero;
}

virtual public float GetNavMeshTargetMarginPosition()
{
    return 0.0f;
}
virtual public void ErasePathNavMesh()
{

}


    public float GetSpeedCurrent()
    {
                
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
        SetSpeedMax(GetSpeedMax());
      
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected void Update()
    {
        if (GetGameManager().ok_)
        {
            ///solo a modo de depuración, se pierde rendimiento pero estos métodos actualizan variables modificadas desde el inspector
            ///para realizar depuración y pruebas en ejecución. En producto final se pueden quitar.            
            SetSpeedMax(GetSpeedMax());
            
        ////Metodo propio, es una sola variable de velocidad independiente de Navmesh, CharacterController o cualqueir otro método.
        ///Se basa en la magnitud de la diferencia de posición  en la pantalla en valor absoluto, dividido entre el tiempo de cada frame.

            currentSpeed_  = Mathf.Abs((transform.position - positionPreviousFrame_).magnitude/Time.deltaTime);  
            positionPreviousFrame_ = transform.position;  ///variable necesaria  para calcular la velocidad            
            
        }
        
    }


}


