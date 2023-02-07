using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : BaseMono
{
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    void Awake()
    {
        Debug.Log("creada instancia AIController awake");
        
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
        




    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GetGameManager().ok_)
        {

        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    public void Seek(Status status,Vector3 location, bool navmesh = true)
    {
        if (navmesh)
        {
            status.GetAgentNavMesh().SetDestination(location);
            Debug.Log("Movimiento navmesh");
        }
        else
        {
             status.transform.rotation = Quaternion.Slerp(status.transform.rotation, Quaternion.LookRotation(location), status.rotationSpeed_ * Time.deltaTime);                    
            status.transform.Translate(0, 0, status.MovementValue());

            Debug.Log("Movimiento manual");
            
        }

        
    }

    public void Flee(Status status,Vector3 location,bool navmesh = true)
    {
        Vector3 fleeVector = location - status.GetOrigin().transform.position;
        Seek(status, status.GetOrigin().transform.position - fleeVector, navmesh);
             
    }

    public void Pursue(Status status,bool navmesh = true)
    {
         Debug.Log("=========================================Modo busqueda");
        Vector3 targetDir = status.GetTarget().transform.position - status.GetOrigin().transform.position;

        float lookAhead = 0;
        if (status.GetCurrentSpeed() != 0.0f)
             lookAhead = targetDir.magnitude * status.GetTargetStatus().GetCurrentSpeed()  / status.GetCurrentSpeed();        

        Debug.DrawRay(status.GetTarget().transform.position, status.GetTarget().transform.forward * lookAhead,Color.red);
        Seek(status,status.GetTarget().transform.position + status.GetTarget().transform.forward * lookAhead,navmesh);

    }    

    Vector3 wanderTarget = Vector3.zero;
    public void Wander(Status status,bool navmesh = true)
    {
        Debug.Log("=========================================Modo Wander");
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;
        
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = status.GetOrigin().transform.InverseTransformVector(targetLocal);

        Seek(status,targetWorld,navmesh);
    }

    /*public void Hide(Status draft)
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i <  World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - draft.GetTarget().transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(draft.GetOrigin().transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(draft.GetOrigin().transform.position, hidePos);
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
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - draft.GetTarget().transform.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 100;
            

            if (Vector3.Distance(draft.GetOrigin().transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(draft.GetOrigin().transform.position, hidePos);
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
*/
    public bool CanSeeTarget(Status draft,GameObject target)
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - draft.GetOrigin().transform.position;
        Debug.DrawRay(draft.GetOrigin().transform.position, target.transform.position - draft.GetOrigin().transform.position ,Color.red);

        if (Physics.Raycast(draft.GetOrigin().transform.position, rayToTarget, out raycastInfo))
        {            
            //Debug.Log("etiqueta" + raycastInfo.transform.tag);
            //Debug.Log("nombre: " +raycastInfo.transform.gameObject.name);
            //if (raycastInfo.transform.gameObject.tag == "Player")
                if (raycastInfo.transform.gameObject == target)
                    return true;
        }
        return false;
    }


  
}