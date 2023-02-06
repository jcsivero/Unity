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
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    [SerializeField] public  bool useNavMeshAI_ = true;
    [SerializeField] public  bool useNavMeshTarget_ = true;    
    [SerializeField] public  Animator anim_;
    [SerializeField] public  UnityEngine.AI.NavMeshAgent agentNavMesh_;   
    [SerializeField] public  float rotationSpeed_ = 2.0f;
    [SerializeField] public  float speed_ = 2.0f;
    [SerializeField] public  float accuracyToWayPoints_ = 1.0f;
    [SerializeField] public  float visDist_ = 20.0f;
    [SerializeField] public  float visAngle_ = 30.0f;
    [SerializeField] public  float visDistToAttack_ = 10.0f;
     
    [SerializeField] public  int currentWP_;
    [SerializeField] public  float currentSpeedAI_;
    [SerializeField] public  float currentSpeedTarget_;    

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


    void Update()
    {
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
    public void UpdateCurrentsSpeeds()
    {                
        ///Actualizo velocidad actual a la que se mueve el target.
        if (GetTarget().GetComponent<CharacterController>() != null)
                currentSpeedTarget_ = GetTarget().GetComponent<CharacterController>().velocity.magnitude;

        ///Actualizo veclocidad actual de movimiento del NPC.
        if ((useNavMeshAI_) && (agentNavMesh_ != null))
        {
            agentNavMesh_.speed = speed_;    
            currentSpeedAI_ =  agentNavMesh_.velocity.magnitude;
        }            
        else 
            currentSpeedAI_ = Time.deltaTime * speed_;

    }
    public float GetCurrentSpeedTarget()
    {        
        return currentSpeedTarget_;
        
    }

    public float GetCurrentSpeedAI()
    {

        return currentSpeedAI_;
    }

}
