using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class  Status :  BaseMono
{
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    [Header("=============== Status")]
    [Space(5)]               
    [Tooltip("Para depuración. A True, en los Update() se actualizarán las variables que se hayan modificado en el inspector en tiempo de ejecución, o que  interese visualizar su valor en todo momento como la velocidad actual.... ")]
    public bool debugMode_ = true; ///
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    [Header("Links to GameObjects")]
    [SerializeField] private GameObject origin_;
    [SerializeField] private GameObject target_;     
    private Vector3 positionPreviousFrame_;

    [HideInInspector] public CommandAddOrSubHealth commandAddOrSubHealth_; ///comandos comunes
    [HideInInspector] public CommandAddOrSubLifes commandAddOrSubLifes_; ///comandos comunes
    private string name_ = "Status";
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

     virtual public  int GetHealth()
     {
        return 0;
     }    

     public Status GetTargetStatus()
     {
        return GetTarget().GetComponent<Status>();
     }
     virtual public  int  GetLifes()
     {
        return 0;
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
    virtual public void  SetHealth(int draft)
    {        
    }
    virtual public void  SetLifes(int draft)
    {        
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
virtual public void SetNavMeshUseSetDestination(bool value)
{

}
    virtual public bool GetNavMeshUseSetDestination()
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

virtual public float GetTargetMarginPosition()
{
    return 0.0f;
}
virtual public void ErasePathNavMesh()
{

}


virtual public float GetSpeedCurrent()
{
            
    return 0;
}

virtual public void SetSpeedCurrent(float speed)
{
    
}
virtual public float GetSpeedMax()
{
    return 0;  

}

virtual public void SetSpeedMax(float speed)
{        
    

}

virtual public void SetSpeedRotation(float speed)
{        
    
}

virtual public float  GetSpeedRotation()
{        
    return 0;    

}

virtual public float  GetVisDistance()
{        
    return 0;    

}
virtual public float  GetVisAngle()
{        
    return 0;    

}
virtual public float  GetVisDistanceToAttack()
{        
    return 0;    

}
virtual public void SetVisDistance(float distance)
{        
    
}
virtual public void SetVisAngle(float  angle)
{        
    
}
virtual public void SetVisDistanceToAttack(float distance)
{        
    
}
virtual public float MovementValue()
{
    return 0;
    
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
        if (debugMode_)
        {
            ///solo a modo de depuración, se pierde rendimiento pero estos métodos actualizan variables modificadas desde el inspector
            ///para realizar depuración y pruebas en ejecución. En producto final se pueden quitar.            
            SetSpeedMax(GetSpeedMax());
            
        ////Metodo propio, es una sola variable de velocidad independiente de Navmesh, CharacterController o cualqueir otro método.
        ///Se basa en la magnitud de la diferencia de posición  en la pantalla en valor absoluto, dividido entre el tiempo de cada frame.

            
            SetSpeedCurrent(Mathf.Abs((transform.position - positionPreviousFrame_).magnitude/Time.deltaTime));        
            positionPreviousFrame_ = transform.position;  ///variable necesaria  para calcular la velocidad            
            SetNavMeshUse(GetNavMeshUse()); ///para actualizar en modo debug. O sea, si cambio en el inspector el valor se actualice inmediatamente.
            SetNavMeshUseSetDestination(GetNavMeshUseSetDestination());
        }
        
    }
    
}


}


