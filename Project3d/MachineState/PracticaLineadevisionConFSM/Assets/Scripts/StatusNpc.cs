using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]
public class StatusNpc : Status
{
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public CommandAddOrSubEnemy commandAddOrSubEnemy_;
    
    public string hidePointTag_;
    public string wayPointTag_;  
    public int[] wayPointToGoOrder_; ///indica el orden de los WayPoints a visitar, en caso de tenerlos. Si su tamaño es cero, se visitarán
    ///todos los WayPoints detectados con la etiqueta que use este NPC. La indicada por tagWayPointsForThisNpc_;
    public  float wayPointsAccuracy_;    
    [SerializeField] private int wayPointCurrent_;
    [SerializeField] private int wayPointIndex_ = 0 ; ///indice dentro de la lista de waypoints. Solo será el mismo valor que currentWayPoints_ cuando
    ///no se ha definido un camino propio para este NPC asignado waypoints en concreto a la varaible wayPointToGoOrder_.  En caso de haberse definido
    ///esa variable, indexWayPoint_ contendrá la posición del array de la variable wayPointToGoOrder_ en la que se encuentra actualmente.
    ///O sea, indexWayPoint_ tiene la posición dentro de wayPointToGoOrder_ en caso de haberse definido, o la posición dentro de la lista de waypoints
    //asociados a la etiqueta.

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
 
    
    [SerializeField] public  float visDist_ = 20.0f;
    [SerializeField] public  float visAngle_ = 30.0f;
    [SerializeField] public  float visDistToAttack_ = 10.0f;    
    
    [SerializeField] public  bool navMeshUse_ = true;     
    
    [SerializeField] public  UnityEngine.AI.NavMeshPath navMeshPath_;    
    public int navMeshPathCurrentIndex_;

    public Vector3 navMeshTargetPosition_; ///posición  final del path del navmesh.
    public float navMeshTargetMarginPosition_=1.0f; /// Margen de movimiento de la posición de destiono de un navmesh. Si ha cambiado más de este este margen, se recalculará un nuevo path.
    public float brakingDistance_=1.0f; ///distancia de parado antes de llegar a los diferentes corners.
    [SerializeField] private  UnityEngine.AI.NavMeshAgent navMeshAgent_;   
    [SerializeField] private  Animator anim_;

    


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    private const string ON_UPDATE_ALL_STATUS_NPC = "ON_UPDATE_ALL_STATUS_NPC";
     private bool suscribeToOnUpdateAllStatusNpc_ = false;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
  

override public UnityEngine.AI.NavMeshAgent GetNavMeshAgent()
{    
    return navMeshAgent_;
}    
override public bool GetNavMeshUse()
{
    return navMeshUse_;
}
override public bool SetNavMeshUse(bool navmesh) 
{
    if (navmesh)
        if (GetNavMeshAgent() != null)
            navMeshUse_ = true;
        else
            return false; ///devuelvo con error porque se intento activar NavMesh y no está el complemento aplicado al objeto.
    else
    {
        navMeshUse_ = false;
        ErasePathNavMesh();
    } 
    return true;  
}

override public UnityEngine.AI.NavMeshPath GetNavMeshPath()
{
    return navMeshPath_;
}
override public int GetNavMeshPathCurrentIndex()
{
    return navMeshPathCurrentIndex_;
}
override public void  SetNavMeshPathCurrentIndex(int index)
{
    navMeshPathCurrentIndex_ = index;
}    

override public void SetSpeedMax(float speed)
{
    base.SetSpeedMax(speed);
    
    //if (GetComponent<CharacterController>() != null)
     //       currentSpeed_ = GetComponent<CharacterController>().velocity.magnitude;
    
    if (GetNavMeshAgent() != null)
         navMeshAgent_.speed = speedMax_;        
}
override public float GetBrakingDistance()
{
    return brakingDistance_;
}
override public void SetBrakingDistance(float distance)
{
    brakingDistance_ = distance;
}
override public void SetNavMeshTargetPosition(Vector3 pos)
{
    navMeshTargetPosition_ = pos;
}
override public Vector3 GetNavMeshTargetPosition()
{
    return navMeshTargetPosition_;
}

override public float GetNavMeshTargetMarginPosition()
{
    return navMeshTargetMarginPosition_;
}
override public void ErasePathNavMesh()
{
    if  (GetNavMeshAgent() != null) ///borro un posible path que ya tuviera asignado el navmes.
       if (GetNavMeshAgent().hasPath)           
           GetNavMeshAgent().ResetPath(); 
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public new void Awake()
    {
        base.Awake();
        InstaciateCommands();       
        SetName("StatusNpc");
        NextWayPoint(0); ///inicialzo la variable currentWayPoint_ con el valor que corresponda.
        anim_ = gameObject.GetComponent<Animator>();                        
        navMeshAgent_ = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshPath_ = new UnityEngine.AI.NavMeshPath();
        navMeshTargetPosition_ = new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity);
        Debug.Log("|||||||||||||| Awake StatusNpc||||||||||||||||");

    }
    public new void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start StatusNpc||||||||||||||||");
        if (!suscribeToOnUpdateAllStatusNpc_)
            OnEnable(); 
       
        
        
    }

       // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    public void OnEnable()   
    {        
        if (!suscribeToOnUpdateAllStatusNpc_) 
            suscribeToOnUpdateAllStatusNpc_ = GetManagerMyEvents().StartListening(ON_UPDATE_ALL_STATUS_NPC,OnUpdateStatusNpc); ///creo evento para actualizar  todos los StatusNpcRobots.
        ///Este evento es lanzado por GameManager,cuando ha actualizado todas las variables iniciales del estado del mundo.
        ///Después se puede utilizar para informar a todos los objetos a la vez y que se actualizen.
        ///Esto no lo hago directamente en el Start() porque no sabemos en que orden son ejecutados,y podría haber Start() que se ejecutan antes que el 
        ///Start() del GameManager, o del StatusWorld, , y entonces no tener todo actualizado, como target_ u otras variables.


        
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    public void OnDisable()
    {      
      GetManagerMyEvents().StopListening(ON_UPDATE_ALL_STATUS_NPC,OnUpdateStatusNpc);
      suscribeToOnUpdateAllStatusNpc_ = false;
      
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        
           Debug.Log("------------- destruido objeto StatusNpc--------------- ");
    }
    bool OnUpdateStatusNpc()
    {
        SetTarget(GetStatusWorld().GetTarget());
        return true;
    }


    protected new void Update()
    {  
        base.Update();
        if (GetGameManager().ok_)
        {
            SetNavMeshUse(navMeshUse_); ///para actualizar en modo debug. O sea, si cambio en el inspector el valor se actualice inmediatamente.
        }
    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InstaciateCommands()
    {
        
        commandAddOrSubEnemy_ = new CommandAddOrSubEnemy();


    }

public int GetCurrentWayPoint()
{
    return wayPointCurrent_;
}

////Devuelve el número del próximo waypoints a visitar. Se le pasa la capacidad de la lista de Waypoints para reiniciar en caso de llegar al último
public void NextWayPoint(int count)
{
        
        if (wayPointToGoOrder_.Length == 0)
        {
            ////Recorro todos los waypoints asignados a esta etiqueta
            SetCurrentWayPoint(wayPointIndex_);            
            wayPointIndex_++;

            if (wayPointIndex_ >= count)
                wayPointIndex_ = 0;
            
        }
        else
        {   
            SetCurrentWayPoint(wayPointToGoOrder_[wayPointIndex_]);
            wayPointIndex_++;
            
            if (wayPointIndex_ >= wayPointToGoOrder_.Length)
                wayPointIndex_ = 0;
                                                     
        }
        
}

public void SetCurrentWayPoint(int draft)
{
    wayPointCurrent_ = draft;
}

virtual public void Fire()
    {
    }
virtual public void StopFiring()
{
 
}
virtual public void StartFiring()
{
 
}    

virtual public void HealthRecovery()
{

}
virtual public void StartHealthRecovery()
{

}
virtual public void StopHealthRecovery()
{

}
}
