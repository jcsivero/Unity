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
    public float shootDist_ = 5.0f;

    string state = "IDLE";

    // Use this for initialization
    void Start()
    {
        bot_ = new Bot(this);
        anim_ = this.GetComponent<Animator>();
        if (target_ == null)
            target_ = GameObject.FindGameObjectWithTag("Player"); ///etiqueta del jugador, que será el objetivo por devecto de la AI, en caso de no
            //haberle asignado una.
        
        if (agent_ == null)
            agent_ = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (waypoints_ == null)
            waypoints_ = GameObject.FindGameObjectsWithTag("waypoint");

    }


    // Update is called once per frame
    void Update()
    {   
        
        Vector3 direction = target_.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        
        anim_.SetFloat("distance", Vector3.Distance(transform.position, target_.transform.position));     
        anim_.SetFloat("angle", Vector3.Distance(transform.position, target_.transform.position));     
        
        if (direction.magnitude < visDist_ && angle < visAngle_)
        {
            if (bot_.CanSeeTarget(target_))
                anim_.SetBool("visibleTarget",true);
            else
                anim_.SetBool("visibleTarget",false);
        }    
        else   
            anim_.SetBool("visibleTarget",false);
            /*

        if (direction.magnitude < visDist_ && angle < visAngle_)
        {
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed_);
            if (direction.magnitude > shootDist_)
            {
                if (state != "RUNNING")
                {
                    state = "RUNNING";
                    anim_.SetTrigger("isRunning");
                }
            }
            else
            {
                if (state != "SHOOTING")
                {
                    state = "SHOOTING";
                    anim_.SetTrigger("isShooting");
                }
            }
        }
        else
        {
            if (state != "IDLE")
            {
                state = "IDLE";
                anim_.SetTrigger("isIdle");
            }
        }
        if (state == "RUNNING")
            this.transform.Translate(0, 0, Time.deltaTime * speed_);*/

        
    }
    public GameObject GetTarget()
    {
        return target_;
        
    }

    public float GetCurrentSpeedTarget()
    {
        
        if (useNavMeshTarget_)
            currentSpeedTarget_ = target_.GetComponent<UnityEngine.AI.NavMeshAgent>().speed;
        else
        {
            if (target_.GetComponent<CharacterController>() != null)
                currentSpeedTarget_ = GetComponent<CharacterController>().velocity.magnitude;
        }
        return currentSpeedTarget_;
        
    }

    public float GetCurrentSpeedAI()
    {
        if (useNavMeshAI_)
            currentSpeedAI_ =  agent_.speed;

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

}