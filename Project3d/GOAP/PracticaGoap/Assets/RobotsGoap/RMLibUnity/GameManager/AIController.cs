using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : BaseMono
{

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
    override public void Awake()
    {
        base.Awake();
        SetName("AiController");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        
        
    }
    
    ///
    // Use this for initialization
    override public void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
                

    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

///Calcula la distancia para el pequeño desplazamiento del objeto, o sea, el desplazamiento menor que se puede hacer, no es la distancia hacia el objetivo final.
private float CalculateDistanceStep(Vector3 location,StatusNpc status)
{
    
    location.y += status.GetNavMeshPointSurfaceToPivotNpc(); 
    return (location-status.transform.position).magnitude;

}
///RealMovement indica que el movimiento se haga siempre hacia adelante y girando, o sea, para llegar a cualquier ubicacion
///el NPC avanza mientras se va orientando, esto hay que tener cuidado si los destinos están demasiado cerca y la velocidad de rotación 
///es muy baja o la velocidad del NPC muy alta, porque nunca llegaría al destino.
///Si realmovement es false, funciona igual que el setdestination de unity, el desplazamiento se realiza siempre directamente hacia la dirección
///punto destino, y mientras se va girando el personaje. Así nunca hay problemas pero parece menos real, puesto que normalmente avanzamos mientras giramos.
///O sea, probar hasta dar con una relación velocidad NPC-velocidad de rotación - tipos giros(las rotaciones muy cerradas) que puede hacer el NPC en el escenario
private bool MovementApply(StatusNpc status,Vector3 location,float customizeBrakingDistance=Mathf.Infinity)
{
    //bool finalCorner=false; ///true se se está yendo al corner final, en cuyo caso se respertará el valor de la variable brakingdistance + la velocidad de movimiento
    float braking=0;
    braking = status.MovementValue(); ////en movimiento sin NavMesh, no se aplica la distancia de frenado, solo la distancia de movimiento.

    if (status.GetNavMeshUse())
        if ((status.GetNavMeshPathCurrentIndex()+1) >= status.GetNavMeshPath().corners.Length-1)
        {
            if (customizeBrakingDistance != Mathf.Infinity)
                braking += customizeBrakingDistance; ///distancia de frenado personalizada, viene bien en muchos caso, como en el flee mode por ejemplo, para
                ///que no esté aplicando la distancia de frenado global en la huida.
            else
                braking += status.GetBrakingDistance();  ///configuración de frenado global             

        }          
    if  (CalculateDistanceStep(location,status) > braking)
    {
        Quaternion rot =  Quaternion.LookRotation(location-status.transform.position);
        rot.z = 0; /// solo quiero rotar en el eje Y.
        rot.x = 0; /// 
        status.transform.rotation = Quaternion.Slerp(status.transform.rotation,rot, status.GetSpeedRotation() * Time.deltaTime);     
                    ///truco que se me ocurrió de los ángulos agudos para saber si me he pasado un corner por tener baja rotación o lo que sea, y seguir automáticamente hacia el
            ///siguiente corner, en lugar de quedarse intentando llegar dando vueltas. Esto debería de funcionar incluso aunque no se haya llegado a la distancia de velocidad
            //del corner correspondiente. Esto es útil solo con la variable de movimiento real activao realMovement y cuando se encuentra en situaciones de giros muy pronunciados
            ///que no se compensan con la velocidad de rotación asignada.

        
              ///solo si quedan los cornes suficiente para poder triangular.
                Vector3 myPosition = status.transform.position;
                myPosition.y -=  status.GetNavMeshPointSurfaceToPivotNpc();         
                Vector3 vectorNpcToNextCorner = location - myPosition;      
                Vector3 vectorCornerToCornerPlus = Vector3.zero;
                if (status.GetNavMeshPathCurrentIndex()+2 <= status.GetNavMeshPath().corners.Length-1) 
                     vectorCornerToCornerPlus = status.GetNavMeshPath().corners[status.GetNavMeshPathCurrentIndex()+2] - location;
                else
                    vectorCornerToCornerPlus = (location - status.GetNavMeshPath().corners[status.GetNavMeshPathCurrentIndex()]);

                float angle = Vector3.Angle(-vectorNpcToNextCorner,vectorCornerToCornerPlus);                
                if (debugMode_)
                    Debug.Log("Seek: Ángulo entre el opuesto de vectorNpcToNextCorner y vectorCornerToCornerPlus: " + angle.ToString());                
                if (angle < 90)
                    return true;

        if (status.realMovement_)                
                status.transform.Translate(0, 0, status.MovementValue());                         
        else
        {
            
            location.y += status.GetNavMeshPointSurfaceToPivotNpc();                     
            status.transform.position = status.transform.position + (location -status.transform.position).normalized * status.MovementValue();  

        }
        return false;

    }
    else
    {
        location.y += status.GetNavMeshPointSurfaceToPivotNpc();
        status.transform.position = location + (status.transform.position - location).normalized * braking;
               
        return true;
    }
        

}


public bool TargetIsMoved(Status status,Vector3 location)
{

    location.y = status.GetNavMeshTargetPosition().y;
    if (debugMode_)
        Debug.Log("Seek: location con Y rectificada a targetposition : " +location.ToString() + " targetposition : " + status.GetNavMeshTargetPosition().ToString() + " distancia :" + Vector3.Distance(location,status.GetNavMeshTargetPosition()).ToString());
  
    if (Vector3.Distance(location,status.GetNavMeshTargetPosition()) > status.GetTargetMarginPosition())
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
            status.SetNavMeshPointSurfaceToPivotNpc(); ///ya calculé la distancia entre el pivote del NPC y de la superficie del navmesh.
            status.SetNavMeshTargetPosition(status.GetNavMeshPath().corners[status.GetNavMeshPath().corners.Length -1]); ///posición target destino es la última del NavMeshPath en caso de haber sido un 
            //path correcto.
            if (status.debugMode_)
            {                
                    for (int i = 0; i <status.GetNavMeshPath().corners.Length -1;i++)
                    {   
                        Debug.DrawRay(status.GetNavMeshPath().corners[i],status.GetNavMeshPath().corners[i+1] -status.GetNavMeshPath().corners[i],Color.blue,20);                    
                        Debug.Log("Seek: posicion corner : " +status.GetNavMeshPath().corners[i+1].ToString() );
                        
                    }
                    
            }
            return true;
        }
        else
        {
            status.NavMeshErasePath();
            if (status.debugMode_)
                Debug.Log("Seek: error asignando nuevo path.................................");
            return false;
    
        }

}

///collisionControl será un raycas que se aplicará desde el objeto hacia la localización y en caso de que se produzca colisión, se seguirá recalculando una nueva
//ruta, con el mismo procdimiento de ir rotando 5 grados.
private bool RecalculatePath(StatusNpc status, Vector3 location,bool collisionControl = false)

{
    if (status.debugMode_)
        Debug.Log("Seek: -----------------------------------------------------------------------------------------------------Recalculando path");
    
    ///voy a intentar recalcular el path para conseguir uno, antes de darlo por erróneo.
    status.pathRecalculated_ = false; 
    bool pathValid = GetPath(status,location);
    
    if (pathValid && collisionControl)
        
         if (ThereIsObstacule(status,status.GetNavMeshTargetPosition()))
         {   pathValid = false;
            status.NavMeshErasePath();
            Debug.Log("<color=red Colisión detectada</color");
         }

    if (pathValid)  ///intento conseguir primero el path, asumiendo que se da una posición correcta.       
        return true;

    if (!status.recalculatePathAutomatic)
        return false; ///devuelvo que no se ha conseguido path. Aquí no he intentado recalcularlo.

    
    int count = 1; // giro de 10 en 10 grados hasta completar una circunferencia o encontar un path válido.
    Vector3 previousLeftVector = Vector3.zero;
    Vector3 previousRightVector = Vector3.zero;
    
    int tryLeftOrRight = Random.Range(0,2); /// aleatoriamente eligo el lado hacia el que probar. 1 derecha  ,0 izquierda
    Debug.Log("numero aleatorio : " +tryLeftOrRight.ToString());
    ///El método que usa para conseguir ruta alternativas, es ir alternando 5 grados a la derecha, 5 a la izquierda, así sucesivamente hasta encontrar una posición válida
    Quaternion rotationRight= Quaternion.AngleAxis(10,Vector3.up);
    Quaternion rotationLeft= Quaternion.AngleAxis(-10,Vector3.up);

    while ((!pathValid) && (count < 35))
    {        
       
        ///obtengo una posición 5 º desplazado a la derecho o a la izquierda

        if (previousRightVector == Vector3.zero) ///en la primera iteración me aseguro de tener un vector
        {
                previousRightVector = location - status.GetOrigin().transform.position; 
                previousLeftVector = location - status.GetOrigin().transform.position; 
        }

        if (tryLeftOrRight==1)
        {
            previousRightVector = rotationRight *  previousRightVector;
            location = previousRightVector + status.GetOrigin().transform.position; ///               
            tryLeftOrRight = Random.Range(0,2);
            Debug.Log("numero aleatorio : " +tryLeftOrRight.ToString());
            if (status.debugMode_)
            {
                Debug.DrawRay(status.GetOrigin().transform.position,previousRightVector,Color.yellow,20);
                Debug.Log("Seek: Intentando Conseguir path alternativo por la derecha número : " + count.ToString());
            }
        }               
        else
        {
            previousLeftVector = rotationLeft * previousLeftVector;
            location = previousLeftVector + status.GetOrigin().transform.position; ///         
            tryLeftOrRight = Random.Range(0,2);     
            Debug.Log("numero aleatorio : " +tryLeftOrRight.ToString());  
            if (status.debugMode_)
            {
                Debug.DrawRay(status.GetOrigin().transform.position,previousLeftVector,Color.red,20);
                Debug.Log("Seek: Intentando Conseguir path alternativo por la izquierda número : " + count.ToString());
            }
        }    
                                
        count++;
        pathValid =  GetPath(status,location); ///intento con la nueva posición    
        if (pathValid && collisionControl)
             if (ThereIsObstacule(status,status.GetNavMeshTargetPosition()))
             {
                pathValid = false;
                status.NavMeshErasePath();
                Debug.Log("<color=red Colisión detectada</color");
             }
                
    }
                 
    if (pathValid)
          status.pathRecalculated_ = true; ///indico que estoy recalculando path.

        
  return pathValid;
}


public bool Seek(StatusNpc status,Vector3 location,float customizeBrakingDistance=Mathf.Infinity,bool collisionControl = false)///devolverá true si llegó al destino.
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
                Debug.Log("Seek: número de corners : " + status.GetNavMeshPath().corners.Length.ToString());
            
            if  (TargetIsMoved(status,location)) ///comprueba donde quiero ir con la posición que tengo guardada de destiono
            {
                if (status.debugMode_)
                    Debug.Log("Seek: objetivo posicion cambiada : " );
                if (RecalculatePath(status,location,collisionControl))    
                    if (status.GetNavMeshUseSetDestination())
                            status.GetNavMeshAgent().SetPath(status.GetNavMeshPath()); ///en caso de usar SetDestination, le asigno el 
                            ///path obtenido para ejecutarse en background                        

            }

            if (((status.GetNavMeshUseSetDestination()) && (status.GetNavMeshAgent().hasPath)&& (status.GetNavMeshAgent().remainingDistance > status.GetNavMeshAgent().stoppingDistance )) || ((!status.GetNavMeshUseSetDestination()) && (status.GetNavMeshPath().corners.Length > 1))) 
            //como mínimo siempre hay dos cornes, el de la posición inicial y la de  la siguiente posición en caso de que haya path.
            ///esto es así para la version sin Setdestination. Con setdestination compruebo el path con hasPath(aunque también hay corners ya que primero se calculan y después se le asigna con Setpath), con mi versión compruebo si hay corners.
            {
                
                if (status.debugMode_)
                    Debug.Log("Seek: tiene path");

                    ///TO DO. Aquí pondré la llamada a la función que controle colisiones, u otras propiedades para movimiento
                    ////como una especie de filtro previo antes de realizr el movimiento. Para evitar tropezar contra objetos que se interpongan en el camino 

                    if (!status.GetNavMeshUseSetDestination()) ///si no uso SetDestination, aplico el movimiento manualmente recorriendo los corners.    
                        if (MovementApply(status, status.GetNavMeshPath().corners[status.GetNavMeshPathCurrentIndex()+1],customizeBrakingDistance))
                        {
                            if (status.debugMode_)
                                Debug.Log("Seek: llego al corner numero: " + status.GetNavMeshPathCurrentIndex()+1);

                            status.SetNavMeshPathCurrentIndex(status.GetNavMeshPathCurrentIndex() +1);
                            if (status.GetNavMeshPathCurrentIndex() >= status.GetNavMeshPath().corners.Length-1)
                            {
                                if (status.debugMode_)
                                    Debug.Log("Seek: llego al final del path por primera vez: ");                                
                                status.NavMeshErasePath();   
                                atDestination = true;      
                            }
                                            
                        }
                
            }
            else
            {
                if (status.debugMode_)
                    Debug.Log("Seek: ESTOY EN EL FINAL: ");                                         

                status.NavMeshErasePath();   
                atDestination = true;      
   
            }
                                      
        }
        else
             status.atDestination_ = MovementApply(status, location,customizeBrakingDistance);  

         
    
status.atDestination_ = atDestination;
return atDestination;
}

public void Flee(StatusNpc status,Vector3 location)
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
        if (status.pathRecalculated_)      
          if (((status.GetNavMeshUseSetDestination()) && (status.GetNavMeshAgent().hasPath)) && (status.GetNavMeshAgent().remainingDistance > status.GetNavMeshAgent().stoppingDistance )|| ((!status.GetNavMeshUseSetDestination()) && (status.GetNavMeshPath().corners.Length > 1))) 
          {
                        
            //if (status.GetNavMeshPathCurrentIndex() == 0) ///me aseguro que solo huye hacia el prmer corner conseguido, por si consiguió una ruta que atravesara algún muro debido a un flee vector amplio
                Seek(status, status.GetNavMeshTargetPosition(),0,status.FleeCollisionControl_);  
            //else  
              //  Seek(status, status.GetOrigin().transform.position + fleeVector,0,status.FleeCollisionControl_);                     
          }

          else
            Seek(status, status.GetOrigin().transform.position + fleeVector,0,status.FleeCollisionControl_);            
        else
          Seek(status, status.GetOrigin().transform.position + fleeVector,0,status.FleeCollisionControl_);            
    else
        Seek(status, status.GetOrigin().transform.position + fleeVector,0,status.FleeCollisionControl_);     
                
            
}

public void Pursue(StatusNpc status)
{
    if (debugMode_)
        Debug.Log("Seek: =========================================Modo busqueda");
    Vector3 targetDir = status.GetTarget().transform.position - status.GetOrigin().transform.position;

    float lookAhead = 0;
    if (status.GetSpeedCurrent() > 0.0f)
        lookAhead = targetDir.magnitude * status.GetTargetStatus().GetSpeedCurrent() * Time.deltaTime * 5 / status.GetSpeedCurrent();        
    
    Debug.DrawRay(status.GetTarget().transform.position, status.GetTarget().transform.forward * lookAhead,Color.red);
    Seek(status,status.GetTarget().transform.position + status.GetTarget().transform.forward * lookAhead);

}    


 Vector3 targetWorld = Vector3.zero;

 ///función para deambular simulando un patrullaje sin waypoints. Puede ser arbitrario con un radio alrededor del NPC o alrededor de un pivotde
 //representado por la posición de un gameobject.
public void Wander(StatusNpc status,float wanderJitter = 1)
{
        if (status.aroundPivot_ == null)
        status.aroundPivot_ = status.GetOrigin();     

    if (status.debugMode_)
        Debug.Log("Seek: =========================================Modo Wander");


    ///revisar porque con esta condición puede que solo se mueva de corner en corner, nunca acabar el path obtenido si devolvió más de dos corner
    if (status.GetNavMeshPath().status != UnityEngine.AI.NavMeshPathStatus.PathComplete)  ///si no tiene un path asignado, asigno uno.
    {
        if (status.debugMode_)
            Debug.Log("Seek: =========================================Asignando nuevo Modo Wander");
        
        //wanderDistance += status.MovementValue() + status.GetBrakingDistance();
        float draftWanderDistance = status.wanderDistance;
        draftWanderDistance += status.MovementValue(); ///como mínimo el movimiento wander debe de avanzar lo que estipule su
        ///velocidad.
        Vector3 wanderTarget = new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= status.wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, draftWanderDistance);
    

        targetWorld = status.aroundPivot_.transform.TransformPoint(targetLocal);
        Seek(status,targetWorld,0,status.WanderCollisionControl_);

    }
    else
    {
            if (status.debugMode_)
                Debug.DrawRay(status.aroundPivot_.transform.position,status.GetNavMeshTargetPosition() - status.aroundPivot_.transform.position ,Color.black);            
            
            
           // if (status.GetNavMeshPathCurrentIndex() == 0)  ///aquí me aseguro de que solo realiza movimiento hasta el primer corner obtenido, para evitar
            ///que por ejemplo estuviera patrullando cerca de un  muro, y si se hubiea elegido un radio amplio, podría dar un punto de patrullaje hacia el otro
            ///lado del muro.
                Seek(status, status.GetNavMeshTargetPosition(),0,status.WanderCollisionControl_);  ///voy a la posición de destino y no tengo en cuenta distancia de frenado.
            //else 
              //  status.NavMeshErasePath(); 
                
    }


    
}


public void PatrolMode(StatusNpc status)
{  
    bool patrol=true;     
    if (status.GetWayPointCurrentPos()== Vector3.zero)
        patrol = status.NextWayPoint();     ///inicializo los waytpoints si la posición devuelta es cero.

    if (patrol)
    {
        if ((Seek(status,status.GetWayPointCurrentPos(),0)) || (CalculateDistanceStep(status.GetWayPointCurrentPos(),status) < status.wayPointsAccuracy_)) 
            status.NextWayPoint();    

    }
    else
       Wander(status);
}
public void Hide(StatusNpc status)
{
    
if(status.hidePointTag_.Length == 0)                
{    
        Debug.Log("Seek: ////////////////////////No se definió etiqueta para Hidepoints para este NPC. "+ status.gameObject.name);        
}
else
{
    if (!GetLevelManager().hidePoints_.ContainsKey(status.hidePointTag_))
    {
        Debug.Log("Seek: ///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
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
        Seek(status, chosenSpot,0);
        Debug.DrawRay(status.GetOrigin().transform.position,chosenSpot- status.GetOrigin().transform.position ,Color.yellow);
            
    }

}

    }
///obtinene el punto del hidepoint más cercano, en base a su punto mínimo del collider y en la dirección contraria al target
public Vector3 CleverHide(StatusNpc status,bool withPosY=false)
{

    if(status.hidePointTag_.Length == 0)                
    {
        Debug.Log("Seek: ////////////////////////No se definió etiqueta para Hidepoints para este NPC. "+ status.gameObject.name);        
    }
    else
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        if (!GetLevelManager().hidePoints_.ContainsKey(status.hidePointTag_))
        {
            Debug.Log("Seek: ///////////////////// Etiqueta para HidePoint no econtrada/////////////////////" + status.gameObject.name);
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
        Debug.Log("Seek: punto de ocultación : " + status.GetHidePointPosBase().ToString());

    return status.GetHidePointPosBase();
    }

  return Vector3.zero; ///significa que hubo error

}
///dirige el objeto hacia el punto de ocultación previamente  calculado con CleverHide(). Si se quiere utilizar la función de actualización
///del punto de ocultación en base al movimiento del objeto target, o sea, activar la variable follow, previmente debe de haberse ejecutado
///la funcion PosIsChangedReset() para reiniciar el contador de detección de movimiento.
//Por defecto no utiliza el reclculado de ruta en caso de fallar, se supone que ya ha sido probado bien por el programador
public bool GoToCleverHide(StatusNpc status,bool withPosY = false,bool follow=false)
{
    if (follow)
        if (status.GetTargetStatus().PosIsChanged())
        {
            CleverHide(status,withPosY);
            status.GetTargetStatus().PosIsChangedReset();         
        }
            

    return Seek(status,status.GetHidePointPosBase(),0);    
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

public bool ThereIsObstacule(StatusNpc status,Vector3 target)
{   
    //Vector3 fwd = status.transform.TransformDirection(Vector3.forward);
    Debug.Log("<color=red> Seek: Comprobando obstáculos </color>");
     Vector3 myPosition;
    myPosition = status.transform.position;
    myPosition.y -= status.GetNavMeshPointSurfaceToPivotNpc();         
    Debug.DrawRay(myPosition,target -myPosition, Color.white,20);
    return  Physics.Raycast(myPosition,target -myPosition,(target -myPosition).magnitude);
    
    
    
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
            Debug.Log("Seek: raycast + " + raycastInfo.transform.gameObject.tag + " " +raycastInfo.transform.gameObject.name) ;
        }
            

            if (raycastInfo.transform.gameObject == target)
                return true;
    }
    return false;
}


}
