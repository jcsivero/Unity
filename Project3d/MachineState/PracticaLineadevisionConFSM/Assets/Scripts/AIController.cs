using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.Animations;


public class AIController : BaseMono
{
             
    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    private const string EVENT_UPDATE_STATUS_WORLD = "EVENT_UPDATE_STATUS_WORLD";    
    

    void Awake()
    {
        Debug.Log("creada instancia AIController");
        
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

        AppendCommand(new CommandUpdateWayPoints());

            //haberle asignado una.

        //UpdateCurrentsSpeeds();
        //taskAddEnemy_.Exec(GameManager.gameManager_.statusWorld_);
        //GameManagerMyEvents.TriggerEvent<Status>(EVENT_UPDATE_STATUS_WORLD,eventData_);


    }


    public void Seek(Status draft,Vector3 location)
    {
        draft.GetAgentNavMesh().SetDestination(location);
        
    }

    public void Flee(Status draft,Vector3 location)
    {
        Vector3 fleeVector = location - draft.origin_.transform.position;
        draft.GetAgentNavMesh().SetDestination(draft.origin_.transform.position - fleeVector);
    }

    public void Pursue(Status draft)
    {
        Vector3 targetDir = draft.target_.transform.position - draft.origin_.transform.position;
/*
        float lookAhead = targetDir.magnitude * ai_.GetCurrentSpeedTarget() / ai_.GetCurrentSpeedAI();
        Debug.DrawRay(ai_.target_.transform.position, ai_.target_.transform.forward * lookAhead,Color.red);
        Seek(ai_.target_.transform.position + ai_.target_.transform.forward * lookAhead);*/
    }

    Vector3 wanderTarget = Vector3.zero;
    public void Wander(Status draft)
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = draft.origin_.transform.InverseTransformVector(targetLocal);

        Seek(draft,targetWorld);
    }

    public void Hide(Status draft)
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - draft.target_.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(draft.origin_.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(draft.origin_.transform.position, hidePos);
            }
        }

        Seek(draft, chosenSpot);

    }
    public void CleverHide(Status draft)
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - ai_.target_.transform.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 100;
            

            if (Vector3.Distance(draft.origin_.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(draft.origin_.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);
        Debug.DrawRay(chosenSpot, -chosenDir.normalized * distance, Color.red);

        Seek(draft,info.point + chosenDir.normalized);
        //Seek(info.point);
    }

    public bool CanSeeTarget(Status draft,GameObject target)
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - draft.origin_.transform.position;
        Debug.DrawRay(draft.origin_.transform.position, target.transform.position - draft.origin_.transform.position ,Color.red);

        if (Physics.Raycast(draft.origin_.transform.position, rayToTarget, out raycastInfo))
        {            
            //Debug.Log("etiqueta" + raycastInfo.transform.tag);
            //Debug.Log("nombre: " +raycastInfo.transform.gameObject.name);
            //if (raycastInfo.transform.gameObject.tag == "Player")
                if (raycastInfo.transform.gameObject == target)
                    return true;
        }
        return false;
    }

    
    /*public GameObject GetTarget()
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
        
        statusNpc_.SetUpdateHud(true);
        statusNpc_.SetDelete(true);
        GetManagerMyEvents().TriggerEvent<Status>(EVENT_UPDATE_STATUS_WORLD,statusNpc_);               
            
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
                
            textHealthNpc_.text = statusNpc_.GetHealth().ToString() + "%";
        }
                     
                       
        
    }*/

}