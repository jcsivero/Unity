using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public float accuracy_ = 1.0f;
    public float visDist_ = 20.0f;
    public float visAngle_ = 30.0f;
    public float visDistToAttack_ = 10.0f;
    


    void Awake()
    {
        Debug.Log("creada instancia AIController");
        bot_ = new Bot(this);
        anim_ = this.GetComponent<Animator>();
        
        GetTarget();

        if (agent_ == null)
            agent_ = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (waypoints_ == null)
            waypoints_ = GameObject.FindGameObjectsWithTag("waypoint");
        
        
    }
    ///
    // Use this for initialization
    void Start()
    {
        Debug.Log("ejecutando start AIController");
        UpdateCurrentsSpeeds();

    }


    // Update is called once per frame
    void Update()
    {   
        
        
    }
    public GameObject GetTarget()
    {
        if (target_ == null)
            target_ = GameObject.FindGameObjectWithTag("Player"); ///etiqueta del jugador, que será el objetivo por devecto de la AI, en caso de no
            //haberle asignado una.

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
        Debug.Log("destruido objeto AIController");
    }

}