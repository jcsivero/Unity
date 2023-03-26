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
[RequireComponent(typeof (StatusWorld))]
[RequireComponent(typeof (StatusHud))]
[RequireComponent(typeof (AIController))]

public class GameManager :BaseMono
{
    
    public static  GameManager instance_ ;
    public float simulationVelocity_ = 1; 
    public StatusWorld statusWorld_;
    public StatusHud statusHud_;
 
    public AIController aiController_;
    public Commands commands_;
    

    [Tooltip("0 Forzar no depuración, 1 Forzar Depuración, 2 Personalizado por componente.")] 
    public DebugModeForce_ debugModeForce_ = DebugModeForce_.customize;
    private const string TRIGGER_ON_UPDATE_ALL_STATUS_NPC = "ON_UPDATE_ALL_STATUS_NPC";
    [HideInInspector ]public bool ok_ = false; //será true, cuando se haya ejecutado el Update de GameManager el primero, asegurando así que puedo establecer
    ///unas condiciones de inicio antes de que comiencen los update del resto de componentes. Si se necesitara actualizar recursos, se puede invocar el evento
    ///al que se adjuntan los scripts.
    
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
    void Awake()
    {
        Debug.Log("Iniciado instancia GameManager desde Awake");
        
        if (instance_!= null && instance_ != this)
            Destroy(gameObject);

        Time.timeScale = simulationVelocity_; ///escala de simulación, de velocidad del juego.
        instance_ = this;

        
        Object.DontDestroyOnLoad(gameObject);

        GetManagerMyEvents(); //Creo instancia de gestión de eventos.
                
        statusWorld_ = GetComponent<StatusWorld>();
        statusHud_ = GetComponent<StatusHud>();
        
        aiController_ = GetComponent<AIController>();

        commands_ = new Commands();


        
        statusWorld_.numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
        statusWorld_.activeLevel_ =SceneManager.GetActiveScene().buildIndex;

        ///Registro mis propios eventos para condiciones de victoria o derrota

        GetManagerMyEvents().StartListening("OnVictory",OnVictory);
        GetManagerMyEvents().StartListening("OnLose",OnLose);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
                
        //  Cursor.visible = false;
         // Cursor.lockState = CursorLockMode.Locked;
                
    //aquí ya estoy seguro de que están todas las suscricipones a eventos hechas.            
    
    }    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!ok_)
        {
            GetManagerMyEvents().TriggerEvent(TRIGGER_ON_UPDATE_ALL_STATUS_NPC) ; ///actualizo todos los objectos.            
            ok_ = true;
        }
        ExecuteCommands(); ///ejecuto todos los comandos que estén en cola.
            
    }


    public bool OnVictory()
    {
        
        GetStatusHud().hudFinalOptions_.SetHudFinal("You win");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GetStatusHud().hudFinalOptions_.gameObject.SetActive(true);
        return true;
    }

    public bool OnLose()
    {
     
        GetStatusHud().hudFinalOptions_.SetHudFinal("You Lose");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GetStatusHud().hudFinalOptions_.gameObject.SetActive(true);
        return true;
    }

          

}




