using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof (StatusWorld))]
[RequireComponent(typeof (StatusHud))]
[RequireComponent(typeof (Commands))]
[RequireComponent(typeof (AIController))]


public class GameManager :BaseMono
{
    
    public static  GameManager instance_ ;
        
    public StatusWorld statusWorld_;
    public StatusHud statusHud_;

    public AIController aiController_;
    public Commands commands_;
    
    

    private bool suscribeToEventUpdateStatusNpc = false;
    private bool suscribeToEventUpdateStatusPlayer = false;
    private bool suscribeToEventUpdateStatusWorld = false;
//    private bool suscribeToEventUpdateStatusHud = false;

    private const string EVENT_UPDATE_STATUS_WORLD = "EVENT_UPDATE_STATUS_WORLD";
    private const string EVENT_UPDATE_STATUS_NPC = "EVENT_UPDATE_STATUS_NPC";
    private const string EVENT_UPDATE_STATUS_PLAYER = "EVENT_UPDATE_STATUS_PLAYER";
    //private const string EVENT_UPDATE_STATUS_HUD = "EVENT_UPDATE_STATUS_HUD";
    
    private const string EVENT_UPDATE_HUD_ONSCREEN = "EVENT_UPDATE_HUD_ONSCREEN";


     public static GameManager Instance()  /// llamar solo desde después de los Awake, para asegurarse que la instancia está creada. O sea, se puede llamar desde OnEnable() o Start()
    {
         return instance_;
    }

    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()   
    {
        if (!suscribeToEventUpdateStatusNpc) 
            suscribeToEventUpdateStatusNpc = GetManagerMyEvents().StartListening<StatusNpc>(EVENT_UPDATE_STATUS_NPC,UpdateStatusNpc);
        if (!suscribeToEventUpdateStatusPlayer) 
            suscribeToEventUpdateStatusPlayer = GetManagerMyEvents().StartListening<StatusPlayer>(EVENT_UPDATE_STATUS_PLAYER,UpdateStatusPlayer);
        if (!suscribeToEventUpdateStatusWorld) 
            suscribeToEventUpdateStatusWorld = GetManagerMyEvents().StartListening<Status>(EVENT_UPDATE_STATUS_WORLD,UpdateStatusWorld);
        //if (!suscribeToEventUpdateStatusHud) 
         //   suscribeToEventUpdateStatusHud = GetManagerMyEvents().StartListening(EVENT_UPDATE_STATUS_HUD,UpdateStatusHud);            
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GetManagerMyEvents().StopListening<Status>(EVENT_UPDATE_STATUS_WORLD,UpdateStatusWorld);
      suscribeToEventUpdateStatusWorld = false;
      GetManagerMyEvents().StopListening<StatusNpc>(EVENT_UPDATE_STATUS_NPC,UpdateStatusNpc);
      suscribeToEventUpdateStatusNpc = false;
      GetManagerMyEvents().StopListening<StatusPlayer>(EVENT_UPDATE_STATUS_PLAYER,UpdateStatusPlayer);
      suscribeToEventUpdateStatusPlayer = false;
//      GetManagerMyEvents().StopListening(EVENT_UPDATE_STATUS_HUD,UpdateStatusHud);
 //     suscribeToEventUpdateStatusHud = false;
      
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Debug.Log("Iniciado instancia GameManager desde Awake");
        
        if (instance_!= null && instance_ != this)
            Destroy(gameObject);
  
        instance_ = this;

        Object.DontDestroyOnLoad(gameObject);

        GetManagerMyEvents(); //Creo instancia de gestión de eventos.
        //commands_ = new Commands(); //inicio el administrador de comandos.
        commands_ = GetComponent<Commands>();
        statusWorld_ = GetComponent<StatusWorld>();
        statusHud_ = GetComponent<StatusHud>();
        aiController_ = GetComponent<AIController>();

        if (statusWorld_.GetTarget()== null)
            statusWorld_.SetTarget(GameObject.FindGameObjectWithTag("Player"));

        statusWorld_.numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
        statusWorld_.activeLevel_ =SceneManager.GetActiveScene().buildIndex;


    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        
        Debug.Log("Iniciado instancia GameManager desde contructor --------- " + GetInstanceID().ToString());
        if ((!suscribeToEventUpdateStatusNpc) || (!suscribeToEventUpdateStatusPlayer)  || (!suscribeToEventUpdateStatusWorld) )
           OnEnable(); 

    //aquí ya estoy seguro de que están todas las suscricipones a eventos hechas.            
    
    //GetManagerMyEvents().TriggerEvent(EVENT_UPDATE_STATUS_HUD);
    UpdateStatusHud();

    }    
      public void LoadNextLevel()
    {
        if (statusWorld_.activeLevel_ < statusWorld_.numberOfLevels_)    
            statusWorld_.activeLevel_ ++;
        else 
            statusWorld_.activeLevel_ = 1;
        
        SceneManager.LoadScene(statusWorld_.activeLevel_);

    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(statusWorld_.activeLevel_);
    }

  bool  UpdateStatusHud()
    {                  
        ///Actualizo solo las variables que son mostradas en el HUD.        
        //statusHud_.SetHealth(statusWorld_.health_);        
          
        GetManagerMyEvents().TriggerEvent<StatusHud>(EVENT_UPDATE_HUD_ONSCREEN,statusHud_);
        return true;      
    }
    
    void  UpdateStatusWorld(Status status, EventDataReturned valueToReturn)
    {       
        ///Analizo objeto Status
       /* if (status.GetName() == "StatusPlayer")
            UpdateStatusPlayer((StatusPlayer) status, valueToReturn);

        if (status.GetName() == "StatusNpc")
            UpdateStatusNpc((StatusNpc) status,valueToReturn);

        if (status.GetUpdateHud())
            UpdateStatusHud();*/
        
    }
    void  UpdateStatusPlayer(StatusPlayer statusPlayer,EventDataReturned valueToReturn)
    {
       
        ///Analizo objeto Status

    }
          
    void  UpdateStatusNpc(StatusNpc statusNpc,EventDataReturned valueToReturn)
    {
       
 
    }
          

}




