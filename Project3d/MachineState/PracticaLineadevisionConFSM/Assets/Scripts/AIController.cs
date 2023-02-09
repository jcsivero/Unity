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
    public void Seek(Status status,Vector3 location)
    {
        if (status.GetUseNavMesh())
        {
            status.GetAgentNavMesh().SetDestination(location);
            Debug.Log("Movimiento navmesh");
        }
        else
        {
            status.transform.rotation = Quaternion.Slerp(status.transform.rotation, Quaternion.LookRotation(location-status.transform.position), status.rotationSpeed_ * Time.deltaTime);                    
            status.transform.Translate(0, 0, status.MovementValue());
            Debug.Log("Movimiento manual");
            
        }

        
    }

    public void Flee(Status status,Vector3 location)
    {
        Vector3 fleeVector = location - status.GetOrigin().transform.position;
        Seek(status, status.GetOrigin().transform.position - fleeVector);
             
    }

    public void Pursue(Status status)
    {
        //Debug.Log("=========================================Modo busqueda");
        Vector3 targetDir = status.GetTarget().transform.position - status.GetOrigin().transform.position;

        float lookAhead = 0;
        if (status.GetCurrentSpeed() > 0.0f)
            lookAhead = targetDir.magnitude * status.GetTargetStatus().GetCurrentSpeed() * Time.deltaTime * 5 / status.GetCurrentSpeed();        
        
        Debug.DrawRay(status.GetTarget().transform.position, status.GetTarget().transform.forward * lookAhead,Color.red);
        Seek(status,status.GetTarget().transform.position + status.GetTarget().transform.forward * lookAhead);
    
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

        Seek(status,targetWorld);
    }

    public void PatrolMode(StatusNpc status)
    {
        if(status.tagWayPoint_.Length == 0)                
        {
            Debug.Log("////////////////////////No hay waypoints para este NPC. pasando a modo Wander.//////////"+ status.gameObject.name);
            Wander(status); ///si no se definió etiqueta para waypoints, se pasa a modo Wander
        }
            
        else
        {
            if (!GetStatusWorld().wayPoints_.ContainsKey(status.tagWayPoint_))
            {
                Debug.Log("///////////////////// Etiqueta para waypoint no econtrada/////////////////////" + status.gameObject.name);
                status.tagWayPoint_ = "Tag No founded";            
            }
            else
            {
                ///Primero obtengo todos los waypoint asignados a esta etiqueta, o sea, la del NPC
                List<GameObject> draft = GetStatusWorld().wayPoints_[status.tagWayPoint_];
                ///                
                if (Vector3.Distance(draft[status.GetCurrentWayPoint()].transform.position, status.transform.position) < status.accuracyToWayPoints_)  
                {
                    status.NextWayPoint(draft.Count);  
                    if (status.GetUseNavMesh())
                        status.ErasePathNavMesh();                  
                }    
                    

                if (status.GetUseNavMesh())
                {
                    if (!status.GetAgentNavMesh().hasPath)///solo asigno nueva ruta en caso de que no tenga. Esto lo hago solo con los waypoints, puesto que son fijos.
                    ///es para ahorrar recursos, ya que con NavMesh, el mismo complemento se encarga de llevar al NPC hasta el destino.                
                    {                    
                        Debug.Log("asignando nuevo path");                                            
                        Seek(status,draft[status.GetCurrentWayPoint()].transform.position);                    
                    }                                    

                }
                else
                    Seek(status,draft[status.GetCurrentWayPoint()].transform.position);                    
                
                
            }


        }
    }
public void Hide(StatusNpc status)
{
    
if(status.tagHidePoint_.Length == 0)                
{
    Debug.Log("////////////////////////No se definió etiqueta para Hidepoints para este NPC. "+ status.gameObject.name);        
}
else
{
    if (!GetStatusWorld().hidePoints_.ContainsKey(status.tagHidePoint_))
    {
        Debug.Log("///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
        status.tagHidePoint_ = "Tag No founded";            
    }
    else
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        ///Primero obtengo todos los Hidepoint asignados a esta etiqueta, o sea, la del NPC
        List<GameObject> draft = GetStatusWorld().hidePoints_[status.tagHidePoint_];            
        for (int i = 0; i <  draft.Count; i++)
        {
            
            Vector3 hideDir = draft[i].transform.position - status.GetTarget().transform.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = draft[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(status.GetOrigin().transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(status.GetOrigin().transform.position, hidePos);
            }
        }
        
        Seek(status, chosenSpot);
        Debug.DrawRay(status.GetOrigin().transform.position,chosenSpot- status.GetOrigin().transform.position ,Color.yellow);
            
    }

}

    }
  public void CleverHide(StatusNpc status)
{

    if(status.tagHidePoint_.Length == 0)                
    {
        Debug.Log("////////////////////////No se definió etiqueta para Hidepoints para este NPC. "+ status.gameObject.name);        
    }
    else
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        if (!GetStatusWorld().hidePoints_.ContainsKey(status.tagHidePoint_))
        {
            Debug.Log("///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
            status.tagHidePoint_ = "Tag No founded";            
        }

                   
        ///Primero obtengo todos los Hidepoint asignados a esta etiqueta, o sea, la del NPC
        List<GameObject> draft = GetStatusWorld().hidePoints_[status.tagHidePoint_];            
        GameObject chosenGO = draft[0];

        for (int i = 0; i <  draft.Count; i++)
        {
            Vector3 hideDir = draft[i].transform.position - status.GetTarget().transform.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = draft[i].transform.position + hideDir.normalized * 100;
            if (Vector3.Distance(status.GetOrigin().transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = draft[i];
                dist = Vector3.Distance(status.GetOrigin().transform.position, hidePos);
            }
        }
    
    Collider hideCol = chosenGO.GetComponent<Collider>();
    Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
    RaycastHit info;
    float distance = 250.0f;
    hideCol.Raycast(backRay, out info, distance);
    Debug.DrawRay(chosenSpot, -chosenDir.normalized * distance, Color.green);

    Seek(status,info.point + chosenDir.normalized);  
    }

  

}

public bool CanSeeTarget(Status status,GameObject target)
{
    RaycastHit raycastInfo;
    Vector3 rayToTarget = target.transform.position - status.GetOrigin().transform.position;
    Debug.DrawRay(status.GetOrigin().transform.position, rayToTarget ,Color.blue);

    if (Physics.Raycast(status.GetOrigin().transform.position, rayToTarget, out raycastInfo))
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