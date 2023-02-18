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

    [Tooltip("Margen de movimiento de la posición de destino. Si ha cambiado más de este este margen, se recalculará un nuevo path en caso de utilizar NavMesh.")]
    public float targetMarginPosition_=1.0f; /// Margen de movimiento de la posición de destino. Si ha cambiado más de este este margen, se recalculará un nuevo path en caso de 
    
     
    protected Vector3 posReferenceFromChanged_; ///Guarda la posición de referencia contra la que se comprobará si el objeto ha variado su posición más alla del umbral establecidor.

    private Vector3 minPos_; ///Posición del valor mínimo del collider. del Npc. Utilizado para obtener posiciones a ras de suelo mucho más fiables para 
    ///no fallar en los Navmesh. La función CalculatePointTarget() de AIController realiza algo parecedolo mismo pero consumiendo más recursos.,
    //En los StatusNpc se calcula desde el inicio.
    protected Vector3 positionPreviousFrame_; ///utilizado para averiguar la velocidad entre cambios de frames.


    [HideInInspector] public CommandAddOrSubHealth commandAddOrSubHealth_; ///comandos comunes
    [HideInInspector] public CommandAddOrSubLifes commandAddOrSubLifes_; ///comandos comunes
    [HideInInspector ]public Vector3 navMeshTargetPositionInfinity_; ///posición  infinita para cuando se quiere recalcular un path, así obligamos a que siempre haya una diferencia entre la
    ///posición del objeto y la del detino mayor que targetMarginPosition, obligando así a recalcular, eso si se utiliza NavMesh.


    private string name_ = "Status";
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
public void PosIsChangedReset()
{    
    posReferenceFromChanged_ = transform.position;
}
public bool PosIsChanged() ///indica si el objeto ha cambiado su posición más alla del umbral de targetMarginPosition con respecto a la posición que tenía cuando se hizo ResetPosIsChanged()
{
    if (Vector3.Distance(transform.position,posReferenceFromChanged_) > GetTargetMarginPosition())
        return true;
    
    return false;
            
}
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


public float GetTargetMarginPosition()
{
    return targetMarginPosition_;
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
public Vector3 GetMinPos_()
{   
    return minPos_;
}
public void SetMinPos_()
{
    if (GetOrigin().GetComponent<Collider>() != null)
    {
            minPos_ = transform.position;
            minPos_.y = GetOrigin().GetComponent<Collider>().bounds.min.y;
    }
        
}
public  void Awake()
{
    InstaciateCommands();
    SetOrigin(gameObject);
    SetMinPos_();
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

            
            SetNavMeshUse(GetNavMeshUse()); ///para actualizar en modo debug. O sea, si cambio en el inspector el valor se actualice inmediatamente.
            SetNavMeshUseSetDestination(GetNavMeshUseSetDestination());
        }


        SetSpeedCurrent(Mathf.Abs((transform.position - positionPreviousFrame_).magnitude/Time.deltaTime));        
        positionPreviousFrame_ = transform.position;  ///variable necesaria  para calcular la velocidad            
        
    }
    
}


}


