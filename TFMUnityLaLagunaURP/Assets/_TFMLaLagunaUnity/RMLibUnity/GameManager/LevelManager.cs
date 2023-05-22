using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : BaseMono
{    
    public HudLevelBase hudLevel_;
    public GameObject actualPlayer_;
    public GameObject actualEnemy_;
    public Dictionary<string,List<GameObject>> wayPoints_;    
    public Dictionary<string,List<GameObject>> hidePoints_;
    public Dictionary<string,List<GameObject>> gameObjectsByName_;
    public List<GameObject> allGameObjects_; 

    public bool paused = false; 

    private const string ON_PAUSE = "ON_PAUSE";
    public bool suscribeToOnPaused_ = false;
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
override public void Awake()
    {
        
        SetName("LevelManager");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");                        
        

        ///Inicializo variables de control de objecots.        
        wayPoints_= new Dictionary<string,List<GameObject>>();        
        hidePoints_ = new Dictionary<string,List<GameObject>>();
        gameObjectsByName_ = new Dictionary<string,List<GameObject>>();
        allGameObjects_ = new List<GameObject>();
        
        
        /*patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach (GameObject c in cubes)
            cubicles.Enqueue(c);
        if (cubes.Length > 0)
            world.ModifyState("freeCubicle", cubes.Length);*/
       
        
    }    

override public void Start()
    {            
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");                        

    if (!GetGameManager().readyEngine_)
    {
        if (GetInitialInformation())
        {
            GetGameManager().readyEngine_ = true;
            SetLevelManager(this);
        }
        else 
        {
            GetGameManager().readyEngine_ = false;
            SetLevelManager(null);

        }

        if (GetGameManager().readyEngine_)
            InstaciateCommands();  
        
        Time.timeScale = GetGameManager().simulationVelocity_; ///escala de simulación, de velocidad del juego.
        Cursor.visible = false;        
        if (!GetGameManager().mobilVesion_)
            Cursor.lockState = CursorLockMode.Locked;
        else        
            Cursor.lockState = CursorLockMode.None;
        paused = false;


        if (!suscribeToOnPaused_)        
            OnEnable();         
            


    }
    }

virtual public void OnEnable()   
    {        
        if (!suscribeToOnPaused_) 
            suscribeToOnPaused_ = GetManagerMyEvents().StartListening(ON_PAUSE,OnPause); ///encargada de pausar el juego tras presionar tecla ESC


    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
virtual public void OnDisable()
{      
    GetManagerMyEvents().StopListening(ON_PAUSE,OnPause);
    suscribeToOnPaused_ = false;
      
}


/// <summary>
/// This function is called when the MonoBehaviour will be destroyed.
/// </summary>
void OnDestroy()
{
    ///OnDisable es llamado cuando se destruye un objeto, así que no me preocupo de desuscribir los eventos, ya que los hago en ondisable.

}
virtual    public void InstaciateCommands()

{
}
public bool OnPause()
{
    Debug.Log("pause presionado");
    paused =  !paused;
    if (paused)
    {        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;                
        GetHudLevel().gameObject.SetActive(true);      
        GetHudLevel().SetValue<string>("HudTextFinal","Pausa\nPresione ESC para continuar");                      
        Time.timeScale = 0;        
    }

    else
    {
        Time.timeScale = GetGameManager().simulationVelocity_;
        Cursor.visible = false;
        if (!GetGameManager().mobilVesion_)
            Cursor.lockState = CursorLockMode.Locked;
        else        
            Cursor.lockState = CursorLockMode.None;        
        
        GetHudLevel().gameObject.SetActive(false);            

    }
    
    return true;
}

virtual public bool GetInitialInformation()
    {
        GetGameManager().hudWorld_ = GameObject.Find("HudWorld").GetComponentInChildren<HudWorldBase>();
        //GetGameManager().hudWorld_ = FindFirstObjectByType<HudWorld>();

        if (GetGameManager().hudWorld_ == null)
        {
            Debug.Log("<color=red> Error grave: !!!!!!!!!Error. GameObject con nombre HudWorld no econtrado o el componente HudWorld.cs o clase heredada dentro de ese gameobject o alguno de sus hijos.</color>");
            return false;
        }
     
        
        hudLevel_ = GameObject.Find("HudLevel").GetComponent<HudLevelBase>();
        
        if (hudLevel_ == null)
        {
            Debug.Log("<color=red>Error grave: !!!!!!!!!Error. GameObject con nombre HudLevel no econtrado o el componente HudLevel.cs o clase heredada dentro de ese gameobject o alguno de sus hijos.</color>");            
            return false;
        }

        ///asigno actual player, actual enemi ,etc según valores de controlgameobjects. por el momento directamente
        if (actualPlayer_ == null)
            actualPlayer_ = GameObject.Find("Player");///por defecto el usuairo llamado player

        GetGameManager().inputController_ = GameObject.Find("InputController").GetComponentInChildren<InputController>();        

        if (GetGameManager().inputController_ == null)
        {
            Debug.Log("<color=red> Error grave: !!!!!!!!!Error. GameObject con nombre InputController no econtrado o el componente InputController.cs o clase heredada dentro de ese gameobject o alguno de sus hijos.</color>");
            return false;
        }

        if (actualPlayer_ != null)             
        {
            GetGameManager().inputController_.characterControllerPlayer_ = actualPlayer_.GetComponent<CharacterController>();            
            GetGameManager().inputController_.cameraController_ = actualPlayer_.GetComponentInChildren<Camera>();

        }

        ControlGameObjects controlGameObjects;
        List<GameObject> refToGameObjets;
        Dictionary<string,List<GameObject>> wayPointsDraft;

        string tag;
        string nameGameObject;

        ///Primero borro toda la información, para comenzar desde cero.

        wayPoints_.Clear();
        hidePoints_.Clear();
        gameObjectsByName_.Clear();
        allGameObjects_.Clear();   
        wayPointsDraft = new Dictionary<string,List<GameObject>>();

        GameObject[] refObject = GameManager.FindObjectsOfType<GameObject>();

        for (int i=0; i<refObject.Length;i++)
        {
            allGameObjects_.Add(refObject[i]); ///los gameobjects encontrados los meto uno a uno en la lista. Sus referencias
            
            ////Ahora actualizo el diccionario gameObjectsByName_ con nombre y lista de gameobjects con el mismo nombre.
            
            nameGameObject =refObject[i].name;

            if (!gameObjectsByName_.ContainsKey(nameGameObject))
                gameObjectsByName_.Add(nameGameObject, new List<GameObject>());
        
            refToGameObjets = gameObjectsByName_[nameGameObject];
            refToGameObjets.Add(refObject[i]);

            /////Ahroa actualizo los wayPoints            

            controlGameObjects =  refObject[i].GetComponent<ControlGameObjects>();
            if (controlGameObjects == null)
                Debug.Log("...................Falta componente ControlGameObjects en objeto con nombre: " + nameGameObject);
            else
            {
                if (controlGameObjects.tagsForWayPoints_.Length > 0)
                {
                    for (int j=0; j < controlGameObjects.tagsForWayPoints_.Length;j++)
                    {
                        tag = controlGameObjects.tagsForWayPoints_[j];
                        if (!wayPoints_.ContainsKey(tag))
                                wayPoints_.Add(tag, new List<GameObject>());
                        
                        refToGameObjets = wayPoints_[tag];
                        if (refToGameObjets.Count< controlGameObjects.indexWayPoint_+1)
                            for (int k=refToGameObjets.Count; k < controlGameObjects.indexWayPoint_+1;k++)
                                refToGameObjets.Add(null); ///relleno huecos en caso de que haya waypoints con 
                                ///numero mayor de los elementos en la lista. Después ya se iran rellenando.
                        refToGameObjets[controlGameObjects.indexWayPoint_] = refObject[i];
                    }
                }
            ////Ahora actualizo los puntos de ocultación.
            
                if (controlGameObjects.tagsForHidePoints_.Length > 0)
                {
                    for (int j=0; j < controlGameObjects.tagsForHidePoints_.Length;j++)
                    {
                        tag = controlGameObjects.tagsForHidePoints_[j];
                        if (!hidePoints_.ContainsKey(tag))
                                hidePoints_.Add(tag, new List<GameObject>());
                        
                        refToGameObjets = hidePoints_[tag];
                        refToGameObjets.Add(refObject[i]);                                                

                    }
                }

            }

            

        }
    return true; ///si se pudo obtener toda la información, se da por correcto.Esto propicia el poner readyengine a true
        
}

virtual    public GameObject GetActualPlayer()
    {
        return actualPlayer_;
    }
virtual    public GameObject GetActualEnemy()
    {
        return actualEnemy_;
    }
}
