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
private float CalculateDistance(Vector3 v1,Vector3 v2, bool withPosY=false)
{

/*    if (!withPosY)
    {  ///si no quiero tener en cuenta la altura para el cálculo de la distancia, pongo sus valores Y a cero.
        v1.y = 0;
        v2.y = 0;
    }
*/
    return (v1-v2).magnitude; 
}
private bool MovementApply(Status status,Vector3 location,bool withPosY=false)
{

    float braking = status.MovementValue() + status.GetBrakingDistance(); ///distancia de frenado 

    if  (CalculateDistance(location,status.transform.position,withPosY) > braking)
    {
        status.transform.rotation = Quaternion.Slerp(status.transform.rotation, Quaternion.LookRotation(location-status.transform.position), status.GetSpeedRotation() * Time.deltaTime);                    
        status.transform.Translate(0, 0, status.MovementValue());
        return false;

    }
    else
    {
        Debug.Log("se llegó al destino final o siguiente cornter.........................................................................");
        return true;
    }
        

}

private void recalculatePath(Status status, Vector3 location)
{
    Debug.Log("-----------------------------------------------------------------------------------------------------Recalculando path");
    if (status.GetNavMeshUseSetDestination())
    {
        status.GetNavMeshAgent().SetDestination(location);
        if (status.GetNavMeshAgent().path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
            status.SetNavMeshTargetPosition(status.GetNavMeshAgent().path.corners[status.GetNavMeshAgent().path.corners.Length-1]); ///igualo a posicion final del path.            
        else        
        {
            status.SetNavMeshTargetPosition(new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity));
            status.GetNavMeshPath().ClearCorners(); ///elimino los cornes anteriores para crear nuevos.  
            Debug.Log("error asignando nuevo path rutina NavMesh SetDestination.................................");
        }
            
    }
    else
    {
    ///////
    //Versión manual todavía en beta
    //////    
        status.GetNavMeshAgent().CalculatePath(location,status.GetNavMeshPath());
        if (status.GetNavMeshPath().status == UnityEngine.AI.NavMeshPathStatus.PathComplete) ///si se encontró un path correcto
        {
            status.SetNavMeshPathCurrentIndex(0);
            status.SetNavMeshTargetPosition(status.GetNavMeshPath().corners[status.GetNavMeshPath().corners.Length -1]); ///posición target destino es la última del NavMeshPath en caso de haber sido un 
            //path correcto.
        }
        else        
        {
            status.GetNavMeshPath().ClearCorners(); ///elimino los cornes anteriores para crear nuevos.        
            status.SetNavMeshTargetPosition(new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity));
            status.SetNavMeshPathCurrentIndex(0);
            Debug.Log("error asignando nuevo path.................................");
        }
            
        
    }      


    

}
public bool Seek(Status status,Vector3 location,bool withPosY=false) ///devolverá true si llegó al destino.
{
    bool optimizar= false;

    if (status.GetNavMeshUse())
        if (status.GetNavMeshUseSetDestination())
        {
           if (optimizar)
            if (status.GetNavMeshAgent().hasPath)
            {
                if (Vector3.Distance(location,status.GetNavMeshTargetPosition()) > status.GetTargetMarginPosition())
                {
                    Debug.Log("objetivo posicion cambiada rutina NavMesh SetDestination. : " + Vector3.Distance(location,status.GetNavMeshTargetPosition()).ToString());
                    recalculatePath(status,location);                    
                }                    
                
            }
            else 
            {
                if (Vector3.Distance(location,status.GetNavMeshTargetPosition()) > status.GetTargetMarginPosition())
                {
                    Debug.Log("asignando nuevo path rutina NavMesh SetDestination.");
                    recalculatePath(status,location);                    
                }
                else           
                {
                    Debug.Log("se llegó al destino final con rutina NavMesh SetDestination.......................................................................");                                         
                    return true;
                }     
                    
                    
                  
            }
            else
                ///la siguiente línea se activa solo para comprobar diferncia de rendimiento
                status.GetNavMeshAgent().SetDestination(location);///solo para comprobar sin optimizar los desplazamientos y cálculos de path, como mejora
                ///los frames por segundos.
               
        }
        else
        {
        /////////////
        //Versión manual en beta todavía
        /////////////
    
            if (status.GetNavMeshPath().corners.Length > 1) //como mínimo siempre hay dos cornes, el de la posición inicial y la de  la siguiente posición .
            {
                Debug.Log("tiene path");

                if  (Vector3.Distance(location,status.GetNavMeshTargetPosition()) > status.GetTargetMarginPosition())
                {
                    Debug.Log("objetivo posicion cambiada : " + Vector3.Distance(location,status.GetNavMeshTargetPosition()).ToString());
                    recalculatePath(status,location);                    
                }
                else
                {                         
                    if (MovementApply(status, status.GetNavMeshPath().corners[status.GetNavMeshPathCurrentIndex()+1],withPosY))
                    {
                        Debug.Log("llego  al corner numero: " + status.GetNavMeshPathCurrentIndex()+1);
                        status.SetNavMeshPathCurrentIndex(status.GetNavMeshPathCurrentIndex() +1);
                         if (status.GetNavMeshPathCurrentIndex() >= status.GetNavMeshPath().corners.Length-1)
                         {
                            Debug.Log("llego  al final del path por primera vez: ");
                            status.GetNavMeshPath().ClearCorners(); ///elimino los cornes anteriores para crear nuevos.
                            return true;
                         }
                    }
                }
    
            }
            else
            {
                if (Vector3.Distance(location,status.GetNavMeshTargetPosition()) > status.GetTargetMarginPosition())
                {
                        Debug.Log("asignando nuevo path");
                        recalculatePath(status, location);                           
                }
                else
                {
                    Debug.Log("ESTOY EN EL FINAL: ");
                return true;
                }
            }
        }
    else
     return   MovementApply(status, location,withPosY);              
                 
 return false;
}

public void Flee(Status status,Vector3 location,bool withPosY=false)
{
    Vector3 fleeVector = location - status.GetOrigin().transform.position;
    Seek(status, status.GetOrigin().transform.position - fleeVector,withPosY);
            
}

public void Pursue(Status status,bool withPosY=false)
{
    //Debug.Log("=========================================Modo busqueda");
    Vector3 targetDir = status.GetTarget().transform.position - status.GetOrigin().transform.position;

    float lookAhead = 0;
    if (status.GetSpeedCurrent() > 0.0f)
        lookAhead = targetDir.magnitude * status.GetTargetStatus().GetSpeedCurrent() * Time.deltaTime * 5 / status.GetSpeedCurrent();        
    
    Debug.DrawRay(status.GetTarget().transform.position, status.GetTarget().transform.forward * lookAhead,Color.red);
    Seek(status,status.GetTarget().transform.position + status.GetTarget().transform.forward * lookAhead,withPosY);

}    

Vector3 wanderTarget = Vector3.zero;
public void Wander(Status status)
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

    Seek(status,targetWorld,false);
}

public void PatrolMode(StatusNpc status,bool withPosY = false)
{
    if(status.wayPointTag_.Length == 0)                
    {
        Debug.Log("////////////////////////No hay waypoints para este NPC. pasando a modo Wander.//////////"+ status.gameObject.name);
        Wander(status); ///si no se definió etiqueta para waypoints, se pasa a modo Wander
    }
        
    else
    {
        if (!GetStatusWorld().wayPoints_.ContainsKey(status.wayPointTag_))
        {
            Debug.Log("///////////////////// Etiqueta para waypoint no econtrada/////////////////////" + status.gameObject.name);
            status.wayPointTag_ = "Tag No founded";            
        }
        else
        {
            ///Primero obtengo todos los waypoint asignados a esta etiqueta, o sea, la del NPC
            List<GameObject> draft = GetStatusWorld().wayPoints_[status.wayPointTag_];
            /// 
            
            if ((Seek(status,draft[status.GetCurrentWayPoint()].transform.position,withPosY)) || (CalculateDistance(draft[status.GetCurrentWayPoint()].transform.position,status.transform.position,withPosY) < status.wayPointsAccuracy_)) 
            //si llegué al final del path o hasta el punto de precisión de los waypointts
                status.NextWayPoint(draft.Count);    


        }


    }
}
public void Hide(StatusNpc status,bool withPosY=false)
{
    
if(status.hidePointTag_.Length == 0)                
{
    Debug.Log("////////////////////////No se definió etiqueta para Hidepoints para este NPC. "+ status.gameObject.name);        
}
else
{
    if (!GetStatusWorld().hidePoints_.ContainsKey(status.hidePointTag_))
    {
        Debug.Log("///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
        status.hidePointTag_ = "Tag No founded";            
    }
    else
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        
        List<GameObject> draft = GetStatusWorld().hidePoints_[status.hidePointTag_];   
        GameObject chosenGO = null;
        for (int i = 0; i <  draft.Count; i++)
        {
            
            Vector3 hideDir = draft[i].transform.position - status.GetTarget().transform.position;            
            hideDir.y = status.transform.position.y; ///altura igual a la del npc
            Vector3 hidePos = draft[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(status.GetOrigin().transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenGO = draft[i];
                dist = Vector3.Distance(status.GetOrigin().transform.position, hidePos);
            }
        }
        
        chosenSpot.y = chosenGO.GetComponent<Collider>().bounds.min.y; ///emito el rayo desde la base de la figura, puesto que el punto de pivote podría estar demasiado alto si la figura es muy
    ///alta y tiene el pivote centrado, dándome un punto que normalmente sea inaccesible para el navmesh navigation.
        Seek(status, chosenSpot,withPosY);
        Debug.DrawRay(status.GetOrigin().transform.position,chosenSpot- status.GetOrigin().transform.position ,Color.yellow);
            
    }

}

    }
public bool CleverHide(StatusNpc status,bool withPosY=false)
{

    if(status.hidePointTag_.Length == 0)                
    {
        Debug.Log("////////////////////////No se definió etiqueta para Hidepoints para este NPC. "+ status.gameObject.name);        
    }
    else
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        if (!GetStatusWorld().hidePoints_.ContainsKey(status.hidePointTag_))
        {
            Debug.Log("///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
            status.hidePointTag_ = "Tag No founded";            
        }

                   
        ///Primero obtengo todos los Hidepoint asignados a esta etiqueta, o sea, la del NPC
        List<GameObject> draft = GetStatusWorld().hidePoints_[status.hidePointTag_];            
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
    
    chosenSpot.y = hideCol.bounds.min.y; ///emito el rayo desde la base de la figura, puesto que el punto de pivote podría estar demasiado alto si la figura es muy
    ///alta y tiene el pivote centrado, dándome un punto que normalmente sea inaccesible para el navmesh navigation.
    Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
    RaycastHit info;
    float distance = 250.0f;
    hideCol.Raycast(backRay, out info, distance);
    Debug.DrawRay(chosenSpot, -chosenDir.normalized * distance, Color.red);
    
    Debug.Log("punto de ocultación : " + (info.point + chosenDir.normalized).ToString() + " tamaño y" + hideCol.bounds.min.ToString());

    return Seek(status,info.point + chosenDir.normalized,withPosY);  
    }

  return false;

}

public bool CanSeeTarget(Status status,GameObject target)
{
    RaycastHit raycastInfo;
    Vector3 rayToTarget = target.transform.position - status.GetOrigin().transform.position;
    Debug.DrawRay(status.GetOrigin().transform.position, rayToTarget ,Color.blue);

    if (Physics.Raycast(status.GetOrigin().transform.position, rayToTarget, out raycastInfo))
    {
        Debug.Log("raycast + " + raycastInfo.transform.gameObject.tag + " " +raycastInfo.transform.gameObject.name) ;
        //Debug.Log("etiqueta" + raycastInfo.transform.tag);
        //Debug.Log("nombre: " +raycastInfo.transform.gameObject.name);
        //if (raycastInfo.transform.gameObject.tag == "Player")
            if (raycastInfo.transform.gameObject.tag == "Player")
                return true;
    }
    return false;
}

}