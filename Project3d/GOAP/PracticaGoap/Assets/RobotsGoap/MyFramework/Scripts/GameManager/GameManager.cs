using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum DebugModeForce_ ///tipo de depuración
{
    noDebug = 0,
    debug = 1,
    customize = 2
}
[RequireComponent(typeof (World))]
[RequireComponent(typeof (AIController))]

public class GameManager :BaseMono
{
    
    public static  GameManager instance_ ;
    public float simulationVelocity_ = 1; 
    public World world_;
    public HudWorldBase hudWorld_;

    public LevelManager levelManager_;
 
    public AIController aiController_;
    public Commands commands_;
    

    [Tooltip("0 Forzar no depuración, 1 Forzar Depuración, 2 Personalizado por componente.")] 
    public DebugModeForce_ debugModeForce_ = DebugModeForce_.customize;
    private const string TRIGGER_ON_UPDATE_ALL_STATUS_NPC = "ON_UPDATE_ALL_STATUS_NPC";    
    public  bool readyEngine_ = false; 
    //public  bool firtUpdate_ = true; 
    
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
        
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        GetManagerMyEvents().OnDestroy();
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    override public void Awake()
    {
        base.Awake();
        SetName("GameManager");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        
        
        if (instance_ == null)
        {
            Time.timeScale = simulationVelocity_; ///escala de simulación, de velocidad del juego.
            instance_ = this;    
            Object.DontDestroyOnLoad(gameObject);

            GetManagerMyEvents(); //Creo instancia de gestión de eventos.
                                
            world_ = GetComponent<World>();            
            
            aiController_ = GetComponent<AIController>();

            commands_ = new Commands();


            
            world_.numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
            world_.activeLevel_ =SceneManager.GetActiveScene().buildIndex;

            ///Registro mis propios eventos para condiciones de victoria o derrota

            GetManagerMyEvents().StartListening("OnVictory",OnVictory);
            GetManagerMyEvents().StartListening("OnLose",OnLose);

        }
        else
        {
            instance_.readyEngine_ = false;           
            //instance_.firtUpdate_ = false;             
            Destroy(gameObject);

        }

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    override public void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        //  Cursor.visible = false;
         // Cursor.lockState = CursorLockMode.Locked;
                
    //aquí ya estoy seguro de que están todas las suscricipones a eventos hechas.            
    
    }    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>


    void Update()
    {
            GetManagerMyEvents().TriggerEvent(TRIGGER_ON_UPDATE_ALL_STATUS_NPC) ; ///actualizo todos los objectos.            
        GetHudWorld().UpdateHud();
        GetHudLevel().UpdateHud();
        ExecuteCommands(); ///ejecuto todos los comandos que estén en cola.
            
    }

    override public bool ReadyEngine()
    {
        if (!GetGameManager().readyEngine_)
        {
            ///si todavía no esta listo el motor, o sea, el gameobject con el scripts levelmanager.cs no ha ejecutado ya su método Start()
            ///entonces localizo el component levelmanager.cs.
            GetGameManager().levelManager_ = GameObject.Find("LevelManager").GetComponent<LevelManager>();


            if (GetGameManager().levelManager_ == null)
            {
                Debug.Log("!!!!!!!!!Error. GameObject con nombre LevelManager no econtrado o el componente LevelManager.cs o clase heredada dentro de ese gameobject o alguno de sus hijos.");                
                GetGameManager().readyEngine_ = false;
            }
            else 
                GetGameManager().levelManager_.Start(); ///ejecuto método start de levelmanager, el cual es el que realmente me dirá si está el motor listo o no.                
        
        }
        return GetGameManager().readyEngine_;        
    }
    public bool OnVictory()
    {
        
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GetHudLevel().SetValue<string>("HudTextFinal","You Win");
        GetHudLevel().gameObject.SetActive(true);

        
        return true;
    }

    public bool OnLose()
    {
     
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;        
        GetHudLevel().SetValue<string>("HudTextFinal","You Lose");
        GetHudLevel().gameObject.SetActive(true);
        
        return true;
    }

          

}





