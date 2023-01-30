using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager_;

    public int  numberOfLevels_;
    public int countEnemies_;    
    public int activeLevel_; //por defecto comienza en la escena 1. La 0 es el menú principal
    public int totalPoints_ = 0;
    public int  levelPoints_ = 0;
    public int lifes_ = 3;

    public int healthPlayer_ = 100;    
    private bool suscribeToEvents = false;
    private const string EVENT_UPDATE_STATUS_WORLD = "EVENT_UPDATE_STATUS_WORLD";
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        suscribeToEvents = GameManagerMyEvents.StartListening<EventData>(EVENT_UPDATE_STATUS_WORLD,UpdateStatusWorld);
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GameManagerMyEvents.StopListening<EventData>(EVENT_UPDATE_STATUS_WORLD,UpdateStatusWorld);
      
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
        numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
        activeLevel_ =SceneManager.GetActiveScene().buildIndex;
        if (gameManager_!= null && gameManager_ != this)
            Destroy(gameObject);
        else
            gameManager_ = this;
            Object.DontDestroyOnLoad(gameObject);

    }

    
      public void LoadNextLevel()
    {
        if (activeLevel_ < numberOfLevels_)    
            activeLevel_ ++;
        else 
            activeLevel_ = 1;
        
        SceneManager.LoadScene(activeLevel_);

    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(activeLevel_);
    }


    void  UpdateStatusWorld(EventData data, EventDataReturned valueToReturn)
    {
        int draft = data.Get<int>("addPoints");
        if (draft != 0)
        {
          levelPoints_ += draft;
          totalPoints_ += levelPoints_;

        }
        
        draft = data.Get<int>("addLifes");
        if ((draft !=0) && (GameManager.gameManager_.lifes_ <= 5))
          lifes_ +=  draft;


        draft = data.Get<int>("addOrSubEnemy");  
        if (draft != 0)
        countEnemies_ +=  draft;

         /// condiciones de fin de partida, reinicio de nivel...

        if (countEnemies_ == 0)
        {
            levelPoints_ = 0;            
            countEnemies_ = 0;
            LoadNextLevel();            
        }
        

        if (lifes_ == 0) 
        {
          //TO DO gameover
        }

        if (data.Get<bool>("lost"))
        {
            levelPoints_ = 0;            
            countEnemies_ = 0;                                         
            ResetLevel();
        }
    }
          

}
