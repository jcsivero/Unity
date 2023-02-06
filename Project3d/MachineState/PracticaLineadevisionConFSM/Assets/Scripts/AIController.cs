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
    public void Seek(Status draft,Vector3 location)
    {
        draft.GetAgentNavMesh().SetDestination(location);
        
    }

    public void Flee(Status draft,Vector3 location)
    {
        Vector3 fleeVector = location - draft.GetOrigin().transform.position;
        draft.GetAgentNavMesh().SetDestination(draft.GetOrigin().transform.position - fleeVector);
    }

    public void Pursue(StatusNpc draft)
    {
        Vector3 targetDir = draft.GetTarget().transform.position - draft.GetOrigin().transform.position;

        float lookAhead = targetDir.magnitude * draft.GetCurrentSpeedTarget() / draft.GetCurrentSpeedAI();
        Debug.DrawRay(draft.GetTarget().transform.position, draft.GetTarget().transform.forward * lookAhead,Color.red);
        Seek((Status)draft,draft.GetTarget().transform.position + draft.GetTarget().transform.forward * lookAhead);
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
        Vector3 targetWorld = draft.GetOrigin().transform.InverseTransformVector(targetLocal);

        Seek(draft,targetWorld);
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