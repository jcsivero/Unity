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
    
    public string tagHidePoint_;
    public string tagWayPoint_;  
    public int[] wayPointToGoOrder_; ///indica el orden de los WayPoints a visitar, en caso de tenerlos. Si su tamaño es cero, se visitarán
    ///todos los WayPoints detectados con la etiqueta que use este NPC. La indicada por tagWayPointsForThisNpc_;
     public  float accuracyToWayPoints_;    
[SerializeField] private int currentWayPoint_;
[SerializeField] private int indexWayPoint_ = 0 ; ///indice dentro de la lista de waypoints. Solo será el mismo valor que currentWayPoints_ cuando
///no se ha definido un camino propio para este NPC asignado waypoints en concreto a la varaible wayPointToGoOrder_.  En caso de haberse definido
///esa variable, indexWayPoint_ contendrá la posición del array de la variable wayPointToGoOrder_ en la que se encuentra actualmente.
///O sea, indexWayPoint_ tiene la posición dentro de wayPointToGoOrder_ en caso de haberse definido, o la posición dentro de la lista de waypoints
//asociados a la etiqueta.

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    [SerializeField] public  bool useNavMeshAI_ = true;      
    
    [SerializeField] public  float visDist_ = 20.0f;
    [SerializeField] public  float visAngle_ = 30.0f;
    [SerializeField] public  float visDistToAttack_ = 10.0f;    
    
    [SerializeField] private  Animator anim_;
    [SerializeField] private  UnityEngine.AI.NavMeshAgent agentNavMesh_;   


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    private const string ON_UPDATE_ALL_STATUS_NPC = "ON_UPDATE_ALL_STATUS_NPC";
     private bool suscribeToOnUpdateAllStatusNpc_ = false;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
  

    public override UnityEngine.AI.NavMeshAgent GetAgentNavMesh()
    {
        return agentNavMesh_;
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
        agentNavMesh_ = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
    return currentWayPoint_;
}

////Devuelve el número del próximo waypoints a visitar. Se le pasa la capacidad de la lista de Waypoints para reiniciar en caso de llegar al último
public void NextWayPoint(int capacity)
{
        
        if (wayPointToGoOrder_.Length == 0)
        {
            ////Recorro todos los waypoints asignados a esta etiqueta
            SetCurrentWayPoint(indexWayPoint_);            
            indexWayPoint_++;

            if (indexWayPoint_ >= capacity)
                indexWayPoint_ = 0;
            
        }
        else
        {   
            SetCurrentWayPoint(wayPointToGoOrder_[indexWayPoint_]);
            indexWayPoint_++;
            
            if (indexWayPoint_ >= wayPointToGoOrder_.Length)
                indexWayPoint_ = 0;
                                                     
        }
        
}

public void SetCurrentWayPoint(int draft)
{
    currentWayPoint_ = draft;
}
override public void SetSpeedMax(float speed)
{
    base.SetSpeedMax(speed);
    
    //if (GetComponent<CharacterController>() != null)
     //       currentSpeed_ = GetComponent<CharacterController>().velocity.magnitude;
    
    if (agentNavMesh_ != null)
        agentNavMesh_.speed = speedMax_;        
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


}
