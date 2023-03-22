using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof (StatusWorld))]
[RequireComponent(typeof (StatusHud))]
[RequireComponent(typeof (AIController))]


public class GameManager :BaseMono
{
    
    public static  GameManager instance_ ;
        
    public StatusWorld statusWorld_;
    public StatusHud statusHud_;

    public AIController aiController_;
    public Commands commands_;
    
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
  
        instance_ = this;

        Object.DontDestroyOnLoad(gameObject);

        GetManagerMyEvents(); //Creo instancia de gestión de eventos.
                
        statusWorld_ = GetComponent<StatusWorld>();
        statusHud_ = GetComponent<StatusHud>();
        aiController_ = GetComponent<AIController>();

        commands_ = new Commands();


        
        statusWorld_.numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
        statusWorld_.activeLevel_ =SceneManager.GetActiveScene().buildIndex;


    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
                
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
   /*   public void LoadNextLevel()
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
          */

}




