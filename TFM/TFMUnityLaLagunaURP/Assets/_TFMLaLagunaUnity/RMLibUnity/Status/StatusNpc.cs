using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]
public class 
StatusNpc : Status
{
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Atributos posibles para el inspector.
    //[SerializeField]
    //[Tooltip("")]
    //[HideInInspector]
    //[Range(1.0f, 10.0f)]
    [Header("=============== StatusNpc")]
    [Space(5)]
    [Header("Attributtes")]
    [SerializeField] private int lifes_;
    [SerializeField] private int health_;
    
    [HideInInspector] public int healthMax_; //se almacenará la variable health inicial para poder conocer en un momento dado, la vida máxima.    
    [Header("Way And Hide Points")]
    [Space(5)]
    public string hidePointTag_;
    private Vector3 hidePointPosBase_;///posición del Waypoint actual respecto a la base del colllider y dirección hacia un objetivo mediante CalculatePointToTarget desde AIController
    public string wayPointTag_;  
    public int[] wayPointToGoOrder_; ///indica el orden de los WayPoints a visitar, en caso de tenerlos. Si su tamaño es cero, se visitarán
    ///todos los WayPoints detectados con la etiqueta que use este NPC. La indicada por tagWayPointsForThisNpc_;
    public  float wayPointsAccuracy_=1;
        
    private Vector3  wayPointsPos_; ///posición actual de su transforn del waypoint actual.
    private Vector3  wayPointsPosBase_; ///posición del Waypoint actual respecto a la base del colllider y dirección hacia un objetivo, obtenido mediante la funcion de AIController llamda
    ///CalculatePointToTarget()


 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

    [Header("Movement: Opciones generales")]    
    [SerializeField]
    [Tooltip("Velocidad máxima de movimiento, este valor también actualiza e iguala el valor speed del complemento Navmesh en caso de utilizarlo. Así se dispone siempre de un mismo valor de velociad, independientemente de la rutina de movimiento utilizada")]        
    private  float speedMax_ = 2.0f;    

    [SerializeField]
    [Tooltip("Velocidad de rotación del Npc utilizada en caso de utilizar mi propia rutina de movimiento NavMesh o cuando no se usa NavMesh. Para velocidad de rotación con NavMesh y rutina de movimiento NavMesh, hay que modifical el valor del complemento NavMesh Angular Speed.")]        
    private  float speedRotation_ = 2.0f;    

    [Tooltip("Utilizar Complemento NavMesh, movimiento directo hacia objetivo, sin calcular rutas. Si está desactivado también se desactivará navMeshUseSetDestination_")]
    public  bool navMeshUse_ = true;     

    [Tooltip ("En caso de utilizar NavMesh, se utilizará SetDestination para el movimiento o si está a false se utilizará mi propia rutina de movimiento si está activada la opción navmeshUse. Al utilizar esta opción, la rotación y el brakingdistance serán los que indiquen el component del agente navmesh agregado.")]
    public bool navMeshUseSetDestination_  = false;

    [Tooltip("En caso de detectar colisiones, usa otra ruta")]
    public bool CollisionControl_ = false;

    [Header("Movement: Opciones Comunes para mi propia rutina NavMesh y navmesh SetDestination")] 
    [Tooltip ("Cuando se calcula una posición con CalculatePointTarget() de AiController,(esta función devuelve una posición cerca del GameOject indicado y en la orientación indicada) puede ocurrir que si se tiene un mapa navmesh con un radio muy grande, se podría devolver una posición fuera del navmesh, y por consiguiente una posición no válida, al calcular un HidePoint, un WayPoint de gameobject con collider.... El valor de esta variable se multiplicará  en la misma orientación  del punto obtenido, para así, devolver una posición que compense el radio del navmesh y por lo tanto devolver una posición válida.")]
    public float navMeshRadius_=0.5f; ///Cuando se calcula una posición con CalculatePointTarget() de AiController,(esta función devuelve una posición cerca del GameOject indicado y en la orientación indicada)
    ///puede ocurrir que si se tiene un mapa navmesh con un radio muy grande, se podría devolver una posición fuera del navmesh, y por consiguiente una posición no válida, al calcular un HidePoint, un WayPoint de gameobject con 
    ///collider.... El valor de esta variable se multiplicará  en la misma orientación  del punto obtenido, para así, devolver una posición que compense el radio del navmesh y por lo tanto
    ///devolver una posición válida.

    [Tooltip("Margen de movimiento de la posición de destino. Si ha cambiado más de este este margen, se recalculará un nuevo path en caso de utilizar NavMesh.")]
    public float targetMarginPosition_=1.0f; /// Margen de movimiento de la posición de destino. Si ha cambiado más de este este margen, se recalculará un nuevo path en caso de 

    [Tooltip("Indica si sse quiere que automáticamente se busca una ruta alternativa utilizando navmesh, en caso de obtener una posición errónea.")]
    public bool recalculatePathAutomatic = true; ///indica si sse quiere que automáticamente se busca una ruta alternativa utilizando navmesh, en caso de obtener
    ///una posición errónea.

    [Header("Movement: Opciones solo para mi propia rutina NavMesh(sin SetDestination)")]    

    [Tooltip("RealMovement indica que el movimiento se haga siempre hacia adelante y girando, o sea, para llegar a cualquier ubicacion el NPC avanza mientras se va orientando, esto hay que tener cuidado si los destinos están demasiado cerca y la velocidad de rotación es muy baja o la velocidad del NPC muy alta, porque nunca llegaría al destino. Si realmovement es false, funciona igual que el setdestination de unity, el desplazamiento se realiza siempre directamente hacia la dirección punto destino, y mientras se va girando el personaje. Así nunca hay problemas pero parece menos real, puesto que normalmente avanzamos mientras giramos. O sea, probar hasta dar con una relación velocidad NPC-velocidad de rotación - tipos giros(las rotaciones muy cerradas) que puede hacer el NPC en el escenario")]
    public bool realMovement_ = true;
    
    [SerializeField]
    [Tooltip ("Distancia de parado antes de llegar al destino final o a si se está usando mi rutina propia, también afecta a los diferentes corners  del path calculado")]
    public float brakingDistance_=1.0f; ///distancia de parado antes de llegar al destino final. Si se está usando mi rutina propia, solo afecta al corner final
    ///utilizar NavMesh.

    [Header("Behavior")]           
    [Header("Behavior: Flee Mode")]      
    [Tooltip("En el modo Flee, distancia hacia el nuevo punto de huida")]
    public bool FleeCollisionControl_ = false;
    public float sizeFleeVector_ = 20.0f;

    [Header("Behavior: Wander Mode")]          

     public bool WanderCollisionControl_ = false;
    [Tooltip("Se utilizará la posición de este GameObject para pivotar alrededor de él en el modo Wander, según radio y distancia")]
    public GameObject aroundPivot_ ; 

    [Tooltip("Distancia a la que colocar el punto calculado en base al radio, ese será el punto definitivo al que se realizará el patrullaje.")]
    public float wanderDistance =1 ;
    [Tooltip("Radio al que se alejará el punto aleatorio obtenido para patrullaje")]
    public float wanderRadius = 2;
    [SerializeField] 
    [Tooltip("Distancia hacia el objetivo que se puede utilizar como disparador en el Animator o cualquier otro sitio para indicar que el objetivo se encuentra a esa distancia o menos.")]
    private  float visDistance_ = 20.0f;
    [SerializeField]
    [Tooltip("Ángulo entre el Npc y el objetivoque se puede utilizar como disparador en el Animator o cualquier otro sitio para indicar que el objetivo se encuentra dentro de ese ángulo de visión o menos")]
     private  float visAngle_ = 30.0f;
    [SerializeField]
    [Tooltip("Distancia hacia el objetivo que se puede utilizar como disparador en el Animator o cualquier otro sitio para indicar que el objetivo se encuentra a esa distancia o menos. Se suele utilizar para definir una zona de ataque, este valor debería de ser menor que el de visDistance_")]
    private  float visDistanceToAttack_ = 10.0f;    

    [Header("Variables solo para depuración. Se deben eliminar en productos finales.")] 
    [SerializeField]
    private  float speedCurrent_;       
    public bool pathRecalculated_; ///a true si cuando se intento asignar path con seek, no  se pudo conseguir y tuvo que
    ////ser recalculado automáticamente para conseguir uno. Esta variable es controlada internamente por RecalculatePath() de AiController, la pongo en el inspector
    ///solo a título de depuración
    
    public float YminPosCollider_; ///Posición del valor mínimo del collider. del Npc. Utilizado para obtener posiciones a ras de suelo mucho más fiables para 
    ///no fallar en los Navmesh. La función CalculatePointTarget() de AIController realiza algo parecedolo mismo pero consumiendo más recursos.,
    //En los StatusNpc se calcula desde el inicio. Es el valor de Y, o sea, la altura a la que se encuentra el valor inferior del collider.

    public float navMeshPointSurfaceToPivotNpc_;    ///diferencia en Y desde el pivote del NPC hasta la superficie del navmesh. Utilizado para cálculos a ras
    //de suelo del navmesh. Es muy útil ya que esta posición varía, sobre todo  los puntos de pivote de los NPC:


    [HideInInspector ]public Vector3 navMeshTargetPositionInitial_ = Vector3.zero; ///posición  inicial para  cuando se quiere recalcular un path, así obligamos a que siempre haya una diferencia entre la
    ///posición del objeto y la del detino mayor que targetMarginPosition, obligando así a recalcular, eso si se utiliza NavMesh.
    
    private int navMeshPathCurrentIndex_;
        
    private Vector3 navMeshTargetPosition_; ///posición  final del path del navmesh.    

    private  UnityEngine.AI.NavMeshAgent navMeshAgent_;   
    [HideInInspector] public  Animator anim_;
    private  UnityEngine.AI.NavMeshPath navMeshPath_;    

    private GAgent goapAgent_; ///acceso al componente Goap del agente en caso de tenerlo agregado al NPC

    private int wayPointCurrent_;
    private int wayPointIndex_ = 0 ; ///indice dentro de la lista de waypoints. Solo será el mismo valor que currentWayPoints_ cuando
    ///no se ha definido un camino propio para este NPC asignado waypoints en concreto a la varaible wayPointToGoOrder_.  En caso de haberse definido
    ///esa variable, indexWayPoint_ contendrá la posición del array de la variable wayPointToGoOrder_ en la que se encuentra actualmente.
    ///O sea, indexWayPoint_ tiene la posición dentro de wayPointToGoOrder_ en caso de haberse definido, o la posición dentro de la lista de waypoints
    //asociados a la etiqueta.


protected const string ON_GOAP_BREAK_ONLY_THIS_GAGENT_NPC = "ON_GOAP_BREAK_ONLY_THIS_GAGENT_NPC";

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    override public void Awake()
    {
        base.Awake();        
        SetName("StatusNpc");            
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        wayPointsPos_ = Vector3.zero;
        wayPointsPosBase_ = Vector3.zero;
                
        
        anim_ = gameObject.GetComponent<Animator>();                        
        goapAgent_ = gameObject.GetComponent<GAgent>();    
        navMeshAgent_ = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshPath_ = new UnityEngine.AI.NavMeshPath();
        navMeshTargetPositionInitial_ =Vector3.zero;
        SetNavMeshTargetPosition(navMeshTargetPositionInitial_);        

    }
    override public void Start()
    {
        base.Start();    
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        SetTarget(GetLevelManager().GetActualPlayer());
        InstaciateCommands();       
        
        

        
    }

       // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    override public void OnEnable()   
    {        
        base.OnEnable();

    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    override public void OnDisable()
    {      
      base.OnDisable();
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        
           Debug.Log("------------- destruido objeto StatusNpc--------------- ");
    }
    override public bool OnUpdateAllStatus()
    {
        base.OnUpdateAllStatus();
        //código propio de esta clase
        return true;
    }


    protected new void Update()
    {  
        if (GetLevelManager().paused)
            return;
        base.Update();
        
    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void InstaciateCommands()
    {
        
    
  

    }


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
  
public float GetYMinPosCollider_()
{   
    if (GetOrigin().GetComponent<Collider>()!= null)
    {
        YminPosCollider_ = GetOrigin().GetComponent<Collider>().bounds.min.y;
        return YminPosCollider_;
    }   
    return 0;
}

  

public float GetNavMeshPointSurfaceToPivotNpc()
{
    return navMeshPointSurfaceToPivotNpc_;
}
public void SetNavMeshPointSurfaceToPivotNpc()
{
    navMeshPointSurfaceToPivotNpc_ = transform.position.y - GetNavMeshPath().corners[0].y;
}

override public float GetTargetMarginPosition()
{
    return targetMarginPosition_;
}
override public UnityEngine.AI.NavMeshAgent GetNavMeshAgent()
{    
    return navMeshAgent_;
}    
override public bool GetNavMeshUse()
{
    return navMeshUse_;
}
override public float GetNavMeshRadius()
{
    return navMeshRadius_;
}
public Vector3 GetHidePointPosBase()
{
    return hidePointPosBase_;
}
public void SetHidePointPosBase(Vector3 value)
{
    hidePointPosBase_ = value;
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
        NavMeshErasePath();
        SetNavMeshUseSetDestination(false); ///si desactivo NavMesh, también desactivo el posible uso de SetDestination
    } 
    return true;  
}
override public void SetNavMeshUseSetDestination(bool value)
{
    if (value)
    {
       if (SetNavMeshUse(true)) ///tengo que habilitar primero el uso de NavMesh
        navMeshUseSetDestination_ = true;
       else
        navMeshUseSetDestination_ =false; //si no se pudo activar, seguramente fue porque no está el complemento NavMesh agregado al GameObject.
    }
    else
        navMeshUseSetDestination_ = false;
       
}
override public bool GetNavMeshUseSetDestination()
{
    return navMeshUseSetDestination_;
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
    speedMax_ = speed;        
    
    //if (GetComponent<CharacterController>() != null)
     //       currentSpeed_ = GetComponent<CharacterController>().velocity.magnitude;
    
    if (GetNavMeshAgent() != null)
         GetNavMeshAgent().speed = speedMax_;        
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
override public void NavMeshErasePath()
{
    if (GetNavMeshUse())    
    {
        GetNavMeshAgent().ResetPath(); 
        GetNavMeshPath().ClearCorners(); ///comprobar si con uno solo basta.
        SetNavMeshPathCurrentIndex(0);
        SetNavMeshTargetPosition(navMeshTargetPositionInitial_);

    }
        
}

override public float GetSpeedCurrent()
{
            
    return speedCurrent_; ///si se usa cálculos de movimiento, recuerda que el verdadero valor es multiplicado por Time.deltaTime, sino es un valor
    ///muy grande
}

override  public void SetSpeedCurrent(float speed)
{
     speedCurrent_ = speed;        
    
}

override public float GetSpeedMax()
{
    return speedMax_;        

}


override public void SetSpeedRotation(float speed)
{        
    speedRotation_ = speed;
    
}

override public float  GetSpeedRotation()
{        
    return speedRotation_;    

}

override public float  GetVisDistance()
{        
    return visDistance_;    

}
override public float  GetVisAngle()
{        
    return visAngle_;    

}

override public float  GetVisDistanceToAttack()
{        
    return visDistanceToAttack_;    

}
override public void SetVisDistance(float distance)
{        
    visDistance_ = distance;
}
override public void SetVisAngle(float  angle)
{        
    visAngle_ = angle;
}
override public void SetVisDistanceToAttack(float distance)
{        
    visDistanceToAttack_ = distance;
}
override public void  SetHealth(int health)
{        
    health_ = health;
}
override public int  GetHealth()
{        
    return health_;
}
   
override public float MovementValue()
{
    return speedMax_ * Time.deltaTime; ////puedo poner también por valor de movimiento del ratón, ejes...
}

public GAgent GetGoapAgent()
{
    return goapAgent_;
}
public GameObject GetWayPointGameObjectCurrent()
{
    return GetLevelManager().wayPoints_[wayPointTag_][GetWayPointCurrent()];
 ////devuelve el GameObject del waypoints actual
}
public int  GetWayPointCurrent()
{
    return wayPointCurrent_; ///devuelvo el número de waypoints actual, su posición en la lista.
}
public Vector3  GetWayPointCurrentPos()
{
        ///devuelve la posición del waypoint actual normal o ajustada a la base del collider y en el punto de intersección con este NPC
                ///o sea, la posición devuelta por el método CalculatePointTarget() de AIcontroller. Eso depende de si ese waypoints tenía collider o no
                //esto se decidión en la funcion NextWayPoint()
        return  wayPointsPos_;

}
////Prepara el número del próximo waypoints a visitar. 
///Devuelve false si no hay waypoints o no se encuentra la etiqueta para este npc.
///Aquí también se comprueba que haya waypoints, y se ajusta la posición del siguiente waypoints igual que su posición calculad con CaclculatePointTarget() de AIController()
//Así no consumo tantos recursos, puesto que solo se recalcula en cada cambio de waypoints.
public bool NextWayPoint()
{
     if(wayPointTag_.Length == 0)                
    {
        if (debugMode_)
            Debug.Log("////////////////////////No hay waypoints para este NPC. pasando a modo Wander.//////////"+ gameObject.name);
        wayPointsPos_ = Vector3.zero;
        return false; ///esto indica que no hay waypoints para este NPC       
    }
        
    else
    {
        if (!GetLevelManager().wayPoints_.ContainsKey(wayPointTag_))
        {            
            if (debugMode_)
               Debug.Log("///////////////////// Etiqueta para waypoint no econtrada/////////////////////" + gameObject.name);
            wayPointTag_ = "Tag No founded";      
            wayPointsPos_ = Vector3.zero;      
            return false; ///esto indica  que no se encontró objectos con la etiqueta indicada.
        }
        else
        {            
            if (wayPointToGoOrder_.Length == 0)
            {
                ////Recorro todos los waypoints asignados a esta etiqueta
                SetWayPointCurrent(wayPointIndex_);                            
                wayPointIndex_++;

                if (wayPointIndex_ >= GetLevelManager().wayPoints_[wayPointTag_].Count)
                    wayPointIndex_ = 0;                                
                
            }
            else
            {   
                SetWayPointCurrent(wayPointToGoOrder_[wayPointIndex_]);
                wayPointIndex_++;
                
                if (wayPointIndex_ >= wayPointToGoOrder_.Length)
                    wayPointIndex_ = 0;                            
                                                        
            }
            
            wayPointsPos_ = GetWayPointGameObjectCurrent().transform.position;
            if (GetWayPointGameObjectCurrent().GetComponent<Collider>() != null) ///si el waypoint tiene colider, entonces su posición la obtengo
            ///utilizando la función CalculatePointTarget() que utiliza el collider para obtener la posición correcta.
            ///hay que tener cuidado,porque la rutas obtenidas del navmesh, si se especificó un radio muy ancho, ya que si es así, el punto obtenido
            ///del collider quedaría dentro de una zona no accesible del navmesh, y por consiguiente daría error asignando la ruta.
            ///la variable breakingdistance ayuda a obtener un valor adecuado.
            ///Esto no se puede resolver con el valor de la variable waypointsaccuracy  ya que este valor se aplica cuando se está llegando al destino,
            ///o sea, después de tener  un path válido, pero este error se producirían al intentar crear el camino.
            //Otra solucín sería asignar el navmesh lo más posible o bien, ampliar el collider para que coincida con el borde del navmesh generado.
                wayPointsPos_ =  GetAIController().CalculatePointTarget(GetOrigin(),GetWayPointGameObjectCurrent(),false,GetNavMeshRadius());
                
             if (debugMode_)
                Debug.Log("punto de target con collider waypoint : " + wayPointsPos_.ToString());
                
        }

    }   
    return true;
}


public void SetWayPointCurrent(int draft)
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
