using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Animations;


public class AIController : MonoBehaviour
{

    public GameObject target_;
    public GameObject bullet_;
    public GameObject originOfFire_;

    public Bot bot_;

    public GameObject[] waypoints_;
    public int currentWP_;

    public bool useNavMeshAI_ = true;
    public bool useNavMeshTarget_ = true;    
    public float currentSpeedAI_;
    public float currentSpeedTarget_;

    public Animator anim_;
    public UnityEngine.AI.NavMeshAgent agent_;
    public float rotationSpeed_ = 2.0f;
    public float speed_ = 2.0f;

    public float accuracyToWayPoints_ = 1.0f;
    public float visDist_ = 20.0f;
    public float visAngle_ = 30.0f;
    public float visDistToAttack_ = 10.0f;

    public StatusNpc statusNpc_;
    public EventData eventData_;
    
    [SerializeField] public TextMesh  textHealthNpc_;
    
    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    private const string EVENT_UPDATE_STATUS_WORLD = "EVENT_UPDATE_STATUS_WORLD";    
    

    void Awake()
    {
        Debug.Log("creada instancia AIController");
        bot_ = new Bot(this);
        statusNpc_ = new StatusNpc();
        
        statusNpc_.SetOrigin(this.gameObject);

        eventData_ =  EventData.Create("EventStatusNpc").Set<Status>("StatusNpc", statusNpc_);



        anim_ = this.GetComponent<Animator>();
                

        if (agent_ == null)
            agent_ = GetComponent<UnityEngine.AI.NavMeshAgent>();

        
    }
     void OnEnable()
    {
                                
        
    }

    
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {

    }
    ///
    // Use this for initialization
    void Start()
    {
        Debug.Log("ejecutando start AIController");

        if (waypoints_ == null)
            waypoints_ = GameObject.FindGameObjectsWithTag("waypoint");

         if (target_ == null)
            target_ = GameObject.FindGameObjectWithTag("Player"); ///etiqueta del jugador, que será el objetivo por devecto de la AI, en caso de no
            //haberle asignado una.

        UpdateCurrentsSpeeds();

        eventData_.Set<int>("addOrSubEnemy",1); ///indico que hay un nuevo NPC.
        GameManagerMyEvents.TriggerEvent<EventData>(EVENT_UPDATE_STATUS_WORLD,eventData_);


    }
    
    public GameObject GetTarget()
    {

        return target_;
        
    }

    public void UpdateCurrentsSpeeds()
    {                
        ///Actualizo velocidad actual a la que se mueve el target.
        if (target_.GetComponent<CharacterController>() != null)
                currentSpeedTarget_ = target_.GetComponent<CharacterController>().velocity.magnitude;

        ///Actualizo veclocidad actual de movimiento del NPC.
        if ((useNavMeshAI_) && (agent_ != null))
        {
            agent_.speed = speed_;    
            currentSpeedAI_ =  agent_.velocity.magnitude;
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

    void Fire()
    {
        GameObject b = Instantiate(bullet_, originOfFire_.transform.position, originOfFire_.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(originOfFire_.transform.forward * 500);
    }
    public void StopFiring()
    {
        CancelInvoke("Fire");
    }
    public void StartFiring()
    {
        InvokeRepeating("Fire", 0.5f, 0.5f);
    }
        /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        eventData_.Set<int>("addOrSubEnemy",-1);
        statusNpc_.SetUpdateHud(true);
        statusNpc_.SetDelete(true);
        GameManagerMyEvents.TriggerEvent<EventData>(EVENT_UPDATE_STATUS_WORLD,eventData_);               
            
        OnDisable();        
        Debug.Log("destruido objeto AIController");
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "bullet")
        {
            statusNpc_.SetHealth(statusNpc_.GetHealth()-10);
        
            if (statusNpc_.GetHealth() <=0)    
                Destroy(this.gameObject);
        }
        else
             textHealthNpc_.text = statusNpc_.GetHealth().ToString() + "%";
                       
        
    }

}