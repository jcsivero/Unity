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
   
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
  

    [Header("Links to GameObjects")]
    [SerializeField] private GameObject origin_;
    [Tooltip("Si no se establece un target, por defecto se localizará el GameObejct con nombre Player para el jugador, nombre Hud para el Hud, etc")]
    [SerializeField] private GameObject target_;     
    [HideInInspector]public bool atDestination_= false; ///true si se encuentra en el destino fijado con la función seek de AIController
    protected Vector3 posReferenceFromChanged_; ///Guarda la posición de referencia contra la que se comprobará si el objeto ha variado su posición más alla del umbral establecidor.
    protected Vector3 positionPreviousFrame_; ///utilizado para averiguar la velocidad entre cambios de frames.


    

    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    private const string ON_UPDATE_ALL_STATUS = "ON_UPDATE_ALL_STATUS";
    private bool suscribeToOnUpdateAllStatus_ = false;
    
override public  void Awake()
{    
    base.Awake();
    SetName("Status");      
    Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
    SetOrigin(gameObject);    
        

}


override public void Start()
{
    base.Start();
    Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");

    if (!suscribeToOnUpdateAllStatus_)        
            OnEnable(); 

    InstaciateCommands();
    
    positionPreviousFrame_ = transform.position;
    SetSpeedMax(GetSpeedMax());
    
    
        
}
 virtual public void OnEnable()   
    {        
        if (!suscribeToOnUpdateAllStatus_) 
            suscribeToOnUpdateAllStatus_ = GetManagerMyEvents().StartListening(ON_UPDATE_ALL_STATUS,OnUpdateAllStatus); ///creo evento para actualizar  todos los StatusNpcRobots.
        ///Este evento es lanzado por GameManager,cuando ha actualizado todas las variables iniciales del estado del mundo.
        ///Después se puede utilizar para informar a todos los objetos a la vez y que se actualizen.
        ///Esto no lo hago directamente en el Start() porque no sabemos en que orden son ejecutados,y podría haber Start() que se ejecutan antes que el 
        ///Start() del GameManager, o del StatusWorld, , y entonces no tener todo actualizado, como target_ u otras variables.


    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    virtual public void OnDisable()
    {      
      GetManagerMyEvents().StopListening(ON_UPDATE_ALL_STATUS,OnUpdateAllStatus);
      suscribeToOnUpdateAllStatus_ = false;
      
    }
    virtual public bool OnUpdateAllStatus()
    {
        
        return true;
    }
/// <summary>
/// Update is called every frame, if the MonoBehaviour is enabled.
/// </summary>
protected void Update()
{
    if (GetLevelManager().paused)
        return;

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

public  GameObject GetOrigin()
{
return origin_;
}    
public  GameObject GetTarget()
{
return target_;
}  

virtual public void  SetHealth(int health)
{        
    
}
virtual public int  GetHealth()
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

 

public  void SetTarget(GameObject target)
{
target_ = target;
}  

public void  SetOrigin(GameObject draft)
{
    origin_ = draft;
}

virtual public void  SetLifes(int draft)
{        
}
private void InstaciateCommands()
{
            


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

virtual public float GetNavMeshRadius()
{
    return 0.5f;
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
    return 0;
}

virtual public void NavMeshErasePath()
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




}


