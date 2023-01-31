using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager_;

    public StatusWorld statusWorld_;
    public StatusHud statusHud_;
   
    private bool suscribeToEventUpdateStatusNpc = false;
    private bool suscribeToEventUpdateStatusPlayer = false;
    private bool suscribeToEventUpdateStatusWorld = false;
//    private bool suscribeToEventUpdateStatusHud = false;

    private const string EVENT_UPDATE_STATUS_WORLD = "EVENT_UPDATE_STATUS_WORLD";
    private const string EVENT_UPDATE_STATUS_NPC = "EVENT_UPDATE_STATUS_NPC";
    private const string EVENT_UPDATE_STATUS_PLAYER = "EVENT_UPDATE_STATUS_PLAYER";
    //private const string EVENT_UPDATE_STATUS_HUD = "EVENT_UPDATE_STATUS_HUD";
    
    private const string EVENT_UPDATE_HUD_ONSCREEN = "EVENT_UPDATE_HUD_ONSCREEN";
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (!suscribeToEventUpdateStatusNpc) 
            suscribeToEventUpdateStatusNpc = GameManagerMyEvents.StartListening<StatusNpc>(EVENT_UPDATE_STATUS_NPC,UpdateStatusNpc);
        if (!suscribeToEventUpdateStatusPlayer) 
            suscribeToEventUpdateStatusPlayer = GameManagerMyEvents.StartListening<StatusPlayer>(EVENT_UPDATE_STATUS_PLAYER,UpdateStatusPlayer);
        if (!suscribeToEventUpdateStatusWorld) 
            suscribeToEventUpdateStatusWorld = GameManagerMyEvents.StartListening<Status>(EVENT_UPDATE_STATUS_WORLD,UpdateStatusWorld);
        //if (!suscribeToEventUpdateStatusHud) 
         //   suscribeToEventUpdateStatusHud = GameManagerMyEvents.StartListening(EVENT_UPDATE_STATUS_HUD,UpdateStatusHud);            
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GameManagerMyEvents.StopListening<Status>(EVENT_UPDATE_STATUS_WORLD,UpdateStatusWorld);
      suscribeToEventUpdateStatusWorld = false;
      GameManagerMyEvents.StopListening<StatusNpc>(EVENT_UPDATE_STATUS_NPC,UpdateStatusNpc);
      suscribeToEventUpdateStatusNpc = false;
      GameManagerMyEvents.StopListening<StatusPlayer>(EVENT_UPDATE_STATUS_PLAYER,UpdateStatusPlayer);
      suscribeToEventUpdateStatusPlayer = false;
//      GameManagerMyEvents.StopListening(EVENT_UPDATE_STATUS_HUD,UpdateStatusHud);
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
        //statusHud_ = new StatusHud();
        statusHud_.SetOrigin(this.gameObject);  
        
        //statusWorld_ = new StatusWorld();
        statusWorld_.SetOrigin(this.gameObject);  
        
        statusWorld_.numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
        statusWorld_.activeLevel_ =SceneManager.GetActiveScene().buildIndex;

        if (gameManager_!= null && gameManager_ != this)
            Destroy(gameObject);
        else
            gameManager_ = this;
            Object.DontDestroyOnLoad(gameObject);
  


    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if ((!suscribeToEventUpdateStatusNpc) || (!suscribeToEventUpdateStatusPlayer)  || (!suscribeToEventUpdateStatusWorld) )
            OnEnable(); 

    //aquí ya estoy seguro de que están todas las suscricipones a eventos hechas.            
    
    //GameManagerMyEvents.TriggerEvent(EVENT_UPDATE_STATUS_HUD);
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
        statusHud_.SetHealth(statusWorld_.health_);          
        GameManagerMyEvents.TriggerEvent<StatusHud>(EVENT_UPDATE_HUD_ONSCREEN,statusHud_);
        return true;      
    }
    void  UpdateStatusWorld(Status status, EventDataReturned valueToReturn)
    {       
        ///Analizo objeto Status
        if (status.GetName() == "StatusPlayer")
            UpdateStatusPlayer((StatusPlayer) status, valueToReturn);

        if (status.GetName() == "StatusNpc")
            UpdateStatusNpc((StatusNpc) status,valueToReturn);

        if (status.GetUpdateHud())
            UpdateStatusHud();
        
    }
    void  UpdateStatusPlayer(StatusPlayer statusPlayer,EventDataReturned valueToReturn)
    {
       
        ///Analizo objeto Status

    }
          
    void  UpdateStatusNpc(StatusNpc statusNpc,EventDataReturned valueToReturn)
    {
       
        ///Analizo objeto Status

    

  


         /// condiciones de fin de partida, reinicio de nivel...

   /*     if (countEnemies_ == 0)
        {
            levelPoints_ = 0;            
            countEnemies_ = 0;
            LoadNextLevel();            
        }
        

/*        if (lifes_ == 0) 
        {
          //TO DO gameover
        }

        if (data.Get<bool>("lost"))
        {
            levelPoints_ = 0;            
            countEnemies_ = 0;                                         
            ResetLevel();
        }*/
    }
          

}

