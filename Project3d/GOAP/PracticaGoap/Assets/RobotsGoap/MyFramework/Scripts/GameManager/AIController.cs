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
    //Debug.Log("posicion npc con y : " + v2);
    //Debug.Log("posicion npc corner con y: " + v1);
    //Debug.Log("distancia npc con y de braking hacia corner: " + (v1-v2).magnitude.ToString());
    if (!withPosY)
    {  ///si no quiero tener en cuenta la altura para el cálculo de la distancia, pongo sus valores Y a cero.
        ///Si se tiene cuenta la altura y se utilizar NavMesh  hay que tener cuidado con poner los destinos, waypoints, hidepoint etc en zonas donde el
        ///navmesh pueda llegar, para evitar errores calculando el path.
        v1.y = 0;
        v2.y = 0;
    }
   // Debug.Log("posicion npc : " + v2);
    //Debug.Log("posicion npc corner : " + v1);
    ///Debug.Log("distancia npc de braking hacia corner: " + (v1-v2).magnitude.ToString());

    return (v1-v2).magnitude; 
}
///RealMovement indica que el movimiento se haga siempre hacia adelante y girando, o sea, para llegar a cualquier ubicacion
///el NPC avanza mientras se va orientando, esto hay que tener cuidado si los destinos están demasiado cerca y la velocidad de rotación 
///es muy baja o la velocidad del NPC muy alta, porque nunca llegaría al destino.
///Si realmovement es false, funciona igual que el setdestination de unity, el desplazamiento se realiza siempre directamente hacia la dirección
///punto destino, y mientras se va girando el personaje. Así nunca hay problemas pero parece menos real, puesto que normalmente avanzamos mientras giramos.
///O sea, probar hasta dar con una relación velocidad NPC-velocidad de rotación - tipos giros(las rotaciones muy cerradas) que puede hacer el NPC en el escenario
private bool MovementApply(StatusNpc status,Vector3 location,bool withPosY=false)
{
    bool finalCorner=false; ///true se se está yendo al corner final, en cuyo caso se respertará el valor de la variable brakingdistance + la velocidad de movimiento
    float braking;
    braking = status.MovementValue(); ///como mínimo debo de estar a una distancia mayor de la velocidad de movimiento, para no pasarme el punto destino

    if (status.GetNavMeshUse())
        if ((status.GetNavMeshPathCurrentIndex()+1) >= status.GetNavMeshPath().corners.Length-1)
        {
            finalCorner = true;///si es el último corner del path, la distancia de frenado equivale al brakingdistance asigando.
            ///esto es para no aplicar esa distancia en todos los corner del path calculado
            braking = status.MovementValue() + status.GetBrakingDistance(); 

        }  
  
    if  (CalculateDistanceStep(location,status.transform.position,withPosY) > braking)
    {
        Quaternion rot =  Quaternion.LookRotation(location-status.transform.position);
        rot.z = 0; /// solo quiero rotar en el eje Y.
        rot.x = 0; /// 
        status.transform.rotation = Quaternion.Slerp(status.transform.rotation,rot, status.GetSpeedRotation() * Time.deltaTime);                    
        if (status.realMovement_)
            status.transform.Translate(0, 0, status.MovementValue());
        else
            status.transform.position = status.transform.position + (location - status.transform.position).normalized * status.MovementValue();            
        return false;

    }
    else
    {
        if (status.debugMode_)
        {
            Debug.Log("distancia inferior a braking " + CalculateDistanceStep(location,status.transform.position,withPosY).ToString());
                 Debug.Log("distancia se llegó al destino final o siguiente corner.........................................................................");
        }
       
        
       if (!finalCorner) 
       {
           status.transform.position = location; ///como estoy a una distancia inferior a la de mi movimiento, me coloco directamente en esa posiión
            ///esto es importante porque las rutas path obtenidas del navmesh, se ajustan mucho a los bordes, por ejemplo para girar una esquina, y si no me 
            ///coloco exactmente donde es, el NPC puede quedarse chocando contra la pared de la esquina por ejemplo.
            status.transform.LookAt(location);

       }
        return true;
    }
        

}


public bool TargetIsMoved(Status status,Vector3 location)
{
    Vector3 v = status.GetNavMeshTargetPosition();
    v.y = location.y; ///corrigo su posición en Y, puesto que el path calculado por el navmesh, devuelve la posición Y modificada, para ajustarla al suelo,
    ///por eso la igualo a la posición a la que quiero ir para compensar en la comparación, yestar seguro de si se ha movido o no el objetivo.
    
    if (Vector3.Distance(location,v) > status.GetTargetMarginPosition())
        return true;
    else
        return false;

}
private bool GetPath(StatusNpc status, Vector3 location)
{
    status.GetNavMeshAgent().CalculatePath(location,status.GetNavMeshPath());
        if (status.GetNavMeshPath().status == UnityEngine.AI.NavMeshPathStatus.PathComplete) ///si se encontró un path correcto
        {   
            status.SetNavMeshPathCurrentIndex(0);
            status.SetNavMeshTargetPosition(status.GetNavMeshPath().corners[status.GetNavMeshPath().corners.Length -1]); ///posición target destino es la última del NavMeshPath en caso de haber sido un 
            //path correcto.
            if (status.debugMode_)
            {       
                    Vector3 draft = status.transform.position;
                    ///pinto el path a usar, el obtenido
                    for (int i = 0; i <status.GetNavMeshPath().corners.Length -1;i++)
                    {   
                        Debug.DrawRay(draft,status.GetNavMeshPath().corners[i+1] -draft,Color.blue,20);
                        draft += status.GetNavMeshPath().corners[i+1] -draft;
                    }
                    
            }
            return true;
        }
        else
        {
            status.NavMeshErasePath();
            if (status.debugMode_)
                Debug.Log("error asignando nuevo path.................................");
            return false;
    
        }

}
private bool RecalculatePath(StatusNpc status, Vector3 location, bool recalcutePathAutomatic = true)

{
    if (status.debugMode_)
        Debug.Log("-----------------------------------------------------------------------------------------------------Recalculando path");
    
    bool pathValid = GetPath(status,location);
    
    if (pathValid)  ///intento conseguir primero el path, asumiendo que se da una posición correcta.
        return true;

    if (!recalcutePathAutomatic)
        return false; ///devuelvo que no se ha conseguido path. Aquí no he intentado recalcularlo.

    ///voy a intentar recalcular el path para conseguir uno, antes de darlo por erróneo.
    status.pathRecalculated_ = false; 
    int count = 1; // giro de 5 en 5 grados hasta completar una circunferencia o encontar un path válido.
    Vector3 previousLeftVector = Vector3.zero;
    Vector3 previousRightVector = Vector3.zero;
    bool tryLeftOrRight = true; ///primero comienzo a probar hacia la derecha(true) después izquierda (false)

    ///El método que usa para conseguir ruta alternativas, es ir alternando 5 grados a la derecha, 5 a la izquierda, así sucesivamente hasta encontrar una posición válida
    Quaternion rotationRight= Quaternion.AngleAxis(5,Vector3.up);
    Quaternion rotationLeft= Quaternion.AngleAxis(-5,Vector3.up);

    while ((!pathValid) && (count < 71))
    {        
       
        ///obtengo una posición 5 º desplazado a la derecho o a la izquierda

        if (previousRightVector == Vector3.zero) ///en la primera iteración me aseguro de tener un vector
        {
                previousRightVector = location - status.GetOrigin().transform.position; 
                previousLeftVector = location - status.GetOrigin().transform.position; 
        }

        if (tryLeftOrRight)
        {
            previousRightVector = rotationRight *  previousRightVector;
            location = previousRightVector + status.GetOrigin().transform.position; ///               
            tryLeftOrRight = false;
            if (status.debugMode_)
            {
                Debug.DrawRay(status.GetOrigin().transform.position,previousRightVector,Color.yellow,20);
                Debug.Log("Intentando Conseguir path alternativo por la derecha número : " + count.ToString());
            }
        }               
        else
        {
            previousLeftVector = rotationLeft * previousLeftVector;
            location = previousLeftVector + status.GetOrigin().transform.position; ///         
            tryLeftOrRight = true;       
            if (status.debugMode_)
            {
                Debug.DrawRay(status.GetOrigin().transform.position,previousLeftVector,Color.red,20);
                Debug.Log("Intentando Conseguir path alternativo por la izquierda número : " + count.ToString());
            }
        }    
                                
        count++;
        pathValid =  GetPath(status,location); ///intento con la nueva posición    
    }
                 
    if (pathValid)
          status.pathRecalculated_ = true; ///indico que estoy recalculando path.

        
  return pathValid;
}
public bool Seek(StatusNpc status,Vector3 location,bool withPosY=false, bool recalcutePathAutomatic = true)///devolverá true si llegó al destino.
{
    bool atDestination = false; ///si he llegado al final del trayecto, esta variable será true 

        /////////////
        //Calculo siempre el path y ya después utilizaré mi propio movimiento mediante MovementApply() el cual mueve solo en
        ///incrementos según la velociad, por lo que tengo que irlo llamando en cada frame, o bien puede elegir usar SetDestination
        ///con el path calculado. En el caso de SetDestination, es el propio método el que hará mover al NPC, por lo que no hace falta
        ///llamarlo en cada frame. No obstante , esta rutina también controla y recalcula un path nuevo solo si se ha cambiado de destino
        ///así que es buena opción llamarla siempre, ya que se encarga además, de que el path este libre de obstáculos, comportamiento que no puedo controlar
        ////si se usa la versión con SetDestination y sin llamarla en cada movimiento.
        //llersión manual en beta todavía
        /////////////
        if (status.GetNavMeshUse())
        {
             if (status.debugMode_)
                Debug.Log("número de corners : " + status.GetNavMeshPath().corners.Length.ToString());
            
            if (((status.GetNavMeshUseSetDestination()) && (status.GetNavMeshAgent().hasPath)&& (status.GetNavMeshAgent().remainingDistance > status.GetNavMeshAgent().stoppingDistance )) || ((!status.GetNavMeshUseSetDestination()) && (status.GetNavMeshPath().corners.Length > 1))) 
            //como mínimo siempre hay dos cornes, el de la posición inicial y la de  la siguiente posición en caso de que haya path.
            ///esto es así para la version sin Setdestination. Con setdestination compruebo el path con hasPath(aunque también hay corners ya que primero se calculan y después se le asigna con Setpath), con mi versión compruebo si hay corners.
            {
                
                if (status.debugMode_)
                    Debug.Log("tiene path");

                if  (TargetIsMoved(status,location))
                {
                    if (status.debugMode_)
                        Debug.Log("objetivo posicion cambiada : " );
                    if (RecalculatePath(status,location,recalcutePathAutomatic))    
                        if (status.GetNavMeshUseSetDestination())
                             status.GetNavMeshAgent().SetPath(status.GetNavMeshPath()); ///en caso de usar SetDestination, le asigno el 
                             ///path obtenido para ejecutarse en background

                }
                else
                {   

                    ///TO DO. Aquí pondré la llamada a la función que controle colisiones, u otras propiedades para movimiento
                    ////como una especie de filtro previo antes de realizr el movimiento. 

                    if (!status.GetNavMeshUseSetDestination()) ///si no uso SetDestination, aplico el movimiento manualmente recorriendo los corners.    
                        if (MovementApply(status, status.GetNavMeshPath().corners[status.GetNavMeshPathCurrentIndex()+1],false))
                        {
                            if (status.debugMode_)
                                Debug.Log("llego  al corner numero: " + status.GetNavMeshPathCurrentIndex()+1);

                            status.SetNavMeshPathCurrentIndex(status.GetNavMeshPathCurrentIndex() +1);
                            if (status.GetNavMeshPathCurrentIndex() >= status.GetNavMeshPath().corners.Length-1)
                            {
                                if (status.debugMode_)
                                    Debug.Log("llego  al final del path por primera vez: ");
                                status.GetNavMeshPath().ClearCorners(); ///elimino los cornes anteriores para crear nuevos.
                                atDestination = true;                            
                            }
                                            
                        }

                }
            }
            else
            {
                if (TargetIsMoved(status,location))
                {
                    if (status.debugMode_)
                        Debug.Log("asignando nuevo path");
                                                         
                    if (RecalculatePath(status,location,recalcutePathAutomatic))    
                        if (status.GetNavMeshUseSetDestination())
                             status.GetNavMeshAgent().SetPath(status.GetNavMeshPath()); ///en caso de usar SetDestination, le asigno el 
                             ///path obtenido para ejecutarse en background

                }
                else
                {
                    if (status.debugMode_)
                        Debug.Log("ESTOY EN EL FINAL: ");
                    atDestination = true;   
                    status.NavMeshErasePath();                 
                }
            }
                         

                                      
        }
        else
             status.atDestination_ = MovementApply(status, location,withPosY);  

         
    
status.atDestination_ = atDestination;
return atDestination;
}

public void Flee(StatusNpc status,Vector3 location, bool withPosY=false)
{
    Vector3 fleeVector = Vector3.zero;
    Vector3 v1 = status.GetOrigin().transform.position;
    v1.y = 0;
    location.y = 0;
    float size = status.MovementValue() * status.sizeFleeVector_; ///distancia de frenado 
    
    fleeVector = (v1 - location).normalized * size;


    if (status.debugMode_)
        Debug.DrawRay(status.GetOrigin().transform.position, fleeVector,Color.green);

    
    if (status.GetNavMeshUse())
        if (((status.GetNavMeshUseSetDestination()) && (status.GetNavMeshAgent().hasPath)) || ((!status.GetNavMeshUseSetDestination()) && (status.GetNavMeshPath().corners.Length > 1))) 
            Seek(status, status.GetNavMeshTargetPosition(),withPosY);  
        else
          Seek(status, status.GetOrigin().transform.position + fleeVector,withPosY);            
    else
        Seek(status, status.GetOrigin().transform.position + fleeVector,withPosY);            
                
            
}

public void Pursue(StatusNpc status,bool withPosY=false)
{
    //Debug.Log("=========================================Modo busqueda");
    Vector3 targetDir = status.GetTarget().transform.position - status.GetOrigin().transform.position;

    float lookAhead = 0;
    if (status.GetSpeedCurrent() > 0.0f)
        lookAhead = targetDir.magnitude * status.GetTargetStatus().GetSpeedCurrent() * Time.deltaTime * 5 / status.GetSpeedCurrent();        
    
    Debug.DrawRay(status.GetTarget().transform.position, status.GetTarget().transform.forward * lookAhead,Color.red);
    Seek(status,status.GetTarget().transform.position + status.GetTarget().transform.forward * lookAhead,withPosY);

}    


 Vector3 targetWorld = Vector3.zero;

 ///función para deambular simulando un patrullaje sin waypoints. Puede ser arbitrario con un radio alrededor del NPC o alrededor de un pivotde
 //representado por la posición de un gameobject.
public void Wander(StatusNpc status,float wanderRadius = 2, float wanderDistance = 1f,float wanderJitter = 1,GameObject aroundPivot = null)
{
        if (aroundPivot == null)
        aroundPivot = status.GetOrigin();     

    if (status.debugMode_)
        Debug.Log("=========================================Modo Wander");


    if (status.GetNavMeshPath().status != UnityEngine.AI.NavMeshPathStatus.PathComplete)  ///si no tiene un path asignado, asigno uno.
    {
        if (status.debugMode_)
            Debug.Log("=========================================Asignando nuevo Modo Wander");
        
        wanderDistance += status.MovementValue() + status.GetBrakingDistance(); ///como mínimo el movimiento wander debe de avanzar lo que estipule su
        ///velocidad más la distancia de frenado,
        Vector3 wanderTarget = new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
    

        targetWorld = aroundPivot.transform.TransformPoint(targetLocal);
                 
        Seek(status,targetWorld,false,true);

    }
    else
    {
            if (status.debugMode_)
                Debug.DrawRay(aroundPivot.transform.position,status.GetNavMeshTargetPosition() - aroundPivot.transform.position ,Color.black);            

            Seek(status,status.GetNavMeshTargetPosition(),false);  ///voy a la posición de destino.
    }


    
}


public void PatrolMode(StatusNpc status,bool withPosY = false)
{  
    bool patrol=true;     
    if (status.GetWayPointCurrentPos()== Vector3.zero)
        patrol = status.NextWayPoint();     ///inicializo los waytpoints si la posición devuelta es cero.

    if (patrol)
    {
        if ((Seek(status,status.GetWayPointCurrentPos(),withPosY,false)) || (CalculateDistanceStep(status.GetWayPointCurrentPos(),status.GetOrigin().transform.position,withPosY) < status.wayPointsAccuracy_)) 
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
    if (!GetLevelManager().hidePoints_.ContainsKey(status.hidePointTag_))
    {
        Debug.Log("///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
        status.hidePointTag_ = "Tag No founded";            
    }
    else
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        
        List<GameObject> draft = GetLevelManager().hidePoints_[status.hidePointTag_];   
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
        if (!GetLevelManager().hidePoints_.ContainsKey(status.hidePointTag_))
        {
            Debug.Log("///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
            status.hidePointTag_ = "Tag No founded";            
        }

                   
        ///Primero obtengo todos los Hidepoint asignados a esta etiqueta, o sea, la del NPC
        List<GameObject> draft = GetLevelManager().hidePoints_[status.hidePointTag_];            
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
    
    
    status.SetHidePointPosBase(CalculatePointTarget(status.GetTarget(),chosenGO,true,status.GetNavMeshRadius()));
    
    if (status.debugMode_)
        Debug.Log("punto de ocultación : " + status.GetHidePointPosBase().ToString());

    return status.GetHidePointPosBase();
    }

  return Vector3.zero; ///significa que hubo error

}
///dirige el objeto hacia el punto de ocultación previamente  calculado con CleverHide(). Si se quiere utilizar la función de actualización
///del punto de ocultación en base al movimiento del objeto target, o sea, activar la variable follow, previmente debe de haberse ejecutado
///la funcion PosIsChangedReset() para reiniciar el contador de detección de movimiento.
//Por defecto no utiliza el reclculado de ruta en caso de fallar, se supone que ya ha sido probado bien por el programador
public bool GoToCleverHide(StatusNpc status,bool withPosY = false,bool follow=false, bool recalcutePathAutomatic = false)
{
    if (follow)
        if (status.GetTargetStatus().PosIsChanged())
        {
            CleverHide(status,withPosY);
            status.GetTargetStatus().PosIsChangedReset();         
        }
            
    
    return Seek(status,status.GetHidePointPosBase(),withPosY,recalcutePathAutomatic);    
}
///Calcula el punto de destino desde la posición actual hacia el Gameobject final.
///El cálculo se realiza:
//Primero se obtiene la posición del gameobject
///Después se calcula la mínima posición de su collider y obtenemos el valor para Y, así no nos preocupamos de la altura del objeto.
///Ahora tenemos la posición del objeto con la Y mínima de su collider.
///Conseguimos lanzar un rayo en X y Z y no verse afectado por la altura de los objetos con pivote en el centro de la maya y además permite que origen y destino  estén en alturas diferentes.
///Lanzamos un rayo desde el propio collider del gameobject destino contra si mismo con dirección desde el gameobject origen.
///Este rayo puede ser se puede lanzar inverso, o sea, desde el sentido contrario pero con la misma dirección.
///Este rayo al impactar con el collider del objeto destino, nos devuelve el punto exacto, puede ser justo en el lado contrario si era inverso.
///Ahora se le suma un valor equipará el radio de un posible navmesh en la escena, para que no devueva un punto inaccesible para el navmesh.
///Este valor  es personalizable pero para las funciones de CleHide y WayPoint cuando utilizan la función CalculatePointTarget(), su valor predeterminado es el 
///brakingdistance del statusnpc. 

///Debo de tener cuidado con los collider tipo capsula, puesto que por su forma, al lanzar el rayo impactará con el punto inferior y por su forma, incluso sumándole
///el brakingdistance, puede no devolver una posición válida si se trabaja con navmesh.
public Vector3 CalculatePointTarget(GameObject origin, GameObject target,bool inverse = false, float navMeshRadius=0)
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
        info.point += dirPoint * navMeshRadius;         
        if (origin.GetComponent<Status>().debugMode_)
            Debug.DrawRay(rayPos, -dirPoint * distance, Color.red,10);
        
    }
    else
    {
        rayPos = rayPos - dirPoint * 100;
        ray.origin = rayPos;
        ray.direction = dirPoint;        
        pointCol.Raycast(ray, out info, distance);        
        info.point -= dirPoint * navMeshRadius; 
        if (origin.GetComponent<Status>().debugMode_)
            Debug.DrawRay(rayPos, dirPoint * distance, Color.red,10);
    }
       
        
    return info.point;
    
    
}

public bool CanSeeTarget(Status status,GameObject target)
{
    RaycastHit raycastInfo;
    Vector3 rayToTarget = target.transform.position - status.GetOrigin().transform.position;          

    if (Physics.Raycast(status.GetOrigin().transform.position, rayToTarget, out raycastInfo))
    {
        if (status.debugMode_)
        {
            Debug.DrawRay(status.GetOrigin().transform.position, rayToTarget ,Color.magenta);
            Debug.Log("raycast + " + raycastInfo.transform.gameObject.tag + " " +raycastInfo.transform.gameObject.name) ;
        }
            

            if (raycastInfo.transform.gameObject == target)
                return true;
    }
    return false;
}


}
