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

///Calcula la distancia para el pequeño desplazamiento del objeto, o sea, el desplazamiento menor que se puede hacer, no es la distancia hacia el objetivo final.
private float CalculateDistanceStep(Vector3 v1,Vector3 v2, bool withPosY=false)
{

    if (!withPosY)
    {  ///si no quiero tener en cuenta la altura para el cálculo de la distancia, pongo sus valores Y a cero.
        ///Si se tiene cuenta la altura y se utilizar NavMesh  hay que tener cuidado con poner los destinos, waypoints, hidepoint etc en zonas donde el
        ///navmesh pueda llegar, para evitar errores calculando el path.
        v1.y = 0;
        v2.y = 0;
    }

    return (v1-v2).magnitude; 
}
private bool MovementApply(Status status,Vector3 location,bool withPosY=false)
{

    float braking = status.MovementValue() + status.GetBrakingDistance(); ///distancia de frenado 

    if  (CalculateDistanceStep(location,status.transform.position,withPosY) > braking)
    {
        Quaternion rot =  Quaternion.LookRotation(location-status.transform.position);
        rot.z = 0; /// solo quiero rotar en el eje Y.
        rot.x = 0; /// 
        status.transform.rotation = Quaternion.Slerp(status.transform.rotation,rot, status.GetSpeedRotation() * Time.deltaTime);                    
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
            status.ErasePathNavMesh();
            //status.SetNavMeshTargetPosition(status.navMeshTargetPositionInfinity_);
            //status.GetNavMeshPath().ClearCorners(); ///elimino los cornes anteriores para crear nuevos.  
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
            status.ErasePathNavMesh();
            //status.GetNavMeshPath().ClearCorners(); ///elimino los cornes anteriores para crear nuevos.        
            //status.SetNavMeshTargetPosition(new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity));
            //status.SetNavMeshPathCurrentIndex(0);
            Debug.Log("error asignando nuevo path.................................");
        }
            
        
    }      
    

}
public bool TargetIsMoved(Status status,Vector3 location)
{
    if (Vector3.Distance(location,status.GetNavMeshTargetPosition()) > status.GetTargetMarginPosition())
        return true;
    else
        return false;

}


public bool Seek(Status status,Vector3 location,bool withPosY=false) ///devolverá true si llegó al destino.
{
    bool optimizar= true;

    if (status.GetNavMeshUse())
        if (status.GetNavMeshUseSetDestination())
        {
            if (optimizar)
                if (status.GetNavMeshAgent().hasPath)
                {
                    if (TargetIsMoved(status,location))
                    {
                        Debug.Log("objetivo posicion cambiada rutina NavMesh SetDestination. : " + Vector3.Distance(location,status.GetNavMeshTargetPosition()).ToString());
                        recalculatePath(status,location);                    
                    }                    
                    
                }
                else 
                {
                    if (TargetIsMoved(status,location))
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

                if  (TargetIsMoved(status,location))
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
                if (TargetIsMoved(status,location))
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
    bool patrol=true;     
    if (status.GetWayPointCurrentPos(true)== Vector3.zero)
        patrol = status.NextWayPoint();     ///inicializo los waytpoints si la posición devuelta es cero.

    if (patrol)
    {
        if ((Seek(status,status.GetWayPointCurrentPos(true),withPosY)) || (CalculateDistanceStep(status.GetWayPointCurrentPos(true),status.GetOrigin().transform.position,withPosY) < status.wayPointsAccuracy_)) 
            status.NextWayPoint();    

    }
    else
       Wander(status);
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
///obtinene el punto del hidepoint más cercano, en base a su punto mínimo del collider y en la dirección contraria al target
public Vector3 CleverHide(StatusNpc status,bool withPosY=false)
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
        Collider hideCol = chosenGO.GetComponent<Collider>();

        for (int i = 0; i <  draft.Count; i++)
        {
            Vector3 hidePoint = draft[i].transform.position;
            hideCol = draft[i].GetComponent<Collider>();            
            hidePoint.y = hideCol.bounds.min.y; ///compruebo la distancia con la posición mínima de la altura del collider, para evitar calculos erróneos cuando
            ///los pivotes se encuentran en el centro de objetos demasiado altos, en los que daría una distancia  mayor de la que realmente están.             

            if (Vector3.Distance(status.GetOrigin().transform.position,hidePoint) < dist)
            {
                chosenGO = draft[i];
                dist = Vector3.Distance(status.GetOrigin().transform.position, hidePoint);
            }
        }
    
    
    status.SetHidePointPosBase(CalculatePointTarget(status.GetTarget(),chosenGO,true));
    Debug.Log("punto de ocultación : " + status.GetHidePointPosBase().ToString());

    return status.GetHidePointPosBase();
    }

  return Vector3.zero; ///significa que hubo error

}
///dirige el objeto hacia el punto de ocultación previamente  calculado con CleverHide(). Si se quiere utilizar la función de actualización
///del punto de ocultación en base al movimiento del objeto target, o sea, activar la variable follow, previmente debe de haberse ejecutado
///la funcion PosIsChangedReset() para reiniciar el contador de detección de movimiento.
public bool GoToCleverHide(StatusNpc status,bool withPosY = false,bool follow=false)
{
    if (follow)
        if (status.GetTargetStatus().PosIsChanged())
        {
            CleverHide(status,withPosY);
            status.GetTargetStatus().PosIsChangedReset();         
        }
            
    
    return Seek(status,status.GetHidePointPosBase(),withPosY);    
}
///Calcula el punto de destino desde la posición actual hacia el Gameobject final. Lo calcula con respecto a la mínima posicion del collider del objeto destino con el
///que impactará un RayCast. Así puedo trabajar a varias altura y no verme afectado por la altura de los objetos con pivote en el centro de la maya.
public Vector3 CalculatePointTarget(GameObject origin, GameObject target,bool inverse = false)
{

    Vector3 point = target.transform.position;
    Collider pointCol = target.GetComponent<Collider>();            
    point.y = pointCol.bounds.min.y;///compruebo la distancia con la posición mínima de la altura del collider, para evitar calculos erróneos cuando
    ///los pivotes se encuentran en el centro de objetos demasiado altos, en los que daría una distancia  mayor de la que realmente están.             

    Vector3 dirPoint = point - origin.transform.position;            
    dirPoint.y = 0.0f;       
    dirPoint = dirPoint.normalized;
    Vector3 rayPos = point;
    Ray ray = new Ray();
    
    RaycastHit info;
    float distance = 100.0f;    

    if (inverse) ///si quiero obtener la posición justo en el lado contrario del objeto con respecto al origin. Esto es útil para establecer puntos de ocultación+
    ///como el algoritmo Hide, o ClerverHide.
    {
        rayPos = rayPos + dirPoint * 100;
        ray.origin = rayPos;
        ray.direction = -dirPoint;
        pointCol.Raycast(ray, out info, distance);
        //info.point += dirPoint;
        Debug.DrawRay(rayPos, -dirPoint * distance, Color.red);
    }
    else
    {
        rayPos = rayPos - dirPoint * 100;
        ray.origin = rayPos;
        ray.direction = dirPoint;        
        pointCol.Raycast(ray, out info, distance);
        //info.point -= dirPoint; 
        Debug.DrawRay(rayPos, dirPoint * distance, Color.red);
    }

        Debug.Log("punto de llegada : " + info.point.ToString() );
    return info.point;
    
    
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
            if (raycastInfo.transform.gameObject == target)
                return true;
    }
    return false;
}


}