using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_GamePlay : MonoBehaviour
{
    private bool suscribeToEvents = false;
    
    [SerializeField] public Text  textActiveLevel_;
    [SerializeField] public Text textTotalPoints_;
    [SerializeField] public Text  textLevelPoints_;
    [SerializeField] public Text textLifes_;
    [SerializeField] public Text textEnemiesLeft_;

    private const string EVENT_UPDATE_HUD_VALUES = "EVENT_UPDATE_HUD_VALUES";
    // Start is called before the first frame update
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        suscribeToEvents = GameManagerMyEvents.StartListening<EventData>(EVENT_UPDATE_HUD_VALUES,UpdateHudValues);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
         if (!suscribeToEvents)
            OnEnable(); 
        GameObject draft = GameObject.Find("TextLevel");
        textActiveLevel_ = draft.GetComponent<Text>();

        draft = GameObject.Find("TextLevelPoints");
        textLevelPoints_ = draft.GetComponent<Text>();

        draft = GameObject.Find("TextTotalPoints");
        textTotalPoints_ = draft.GetComponent<Text>();

        draft = GameObject.Find("TextLifes");        
        textLifes_ = draft.GetComponent<Text>();

        draft = GameObject.Find("TextEnemiesLeft");
        textEnemiesLeft_ = draft.GetComponent<Text>();
        UpdateHud();

    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
      Debug.Log("Unsuscribe Trigger");
      GameManagerMyEvents.StopListening<EventData>(EVENT_UPDATE_HUD_VALUES,UpdateHudValues);
      
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
    }

    void UpdateHud()
    {
      textLevelPoints_.text = GameManager.gameManager_.levelPoints_.ToString();
      textTotalPoints_.text = GameManager.gameManager_.totalPoints_.ToString();
      textLifes_.text = GameManager.gameManager_.lifes_.ToString();
      textActiveLevel_.text = GameManager.gameManager_.activeLevel_.ToString();
      textEnemiesLeft_.text = GameManager.gameManager_.countEnemies_.ToString();
      
    }
    void  UpdateHudValues(EventData data, EventDataReturned valueToReturn)
    {      
        int draft = data.Get<int>("addPoints");
        if (draft != 0)
        {
          GameManager.gameManager_.levelPoints_ += draft;
          GameManager.gameManager_.totalPoints_ += GameManager.gameManager_.levelPoints_;

        }
        
        draft = data.Get<int>("addLifes");
        if ((draft !=0) && (GameManager.gameManager_.lifes_ <= 5))
          GameManager.gameManager_.lifes_ +=  draft;

        if  (data.Get<bool>("lost"))
              GameManager.gameManager_.lifes_ --;        
        
        draft = data.Get<int>("addOrSubEnemy");  
      if (draft != 0)
        GameManager.gameManager_.countEnemies_ +=  draft;
        
        UpdateHud();
      
        /// condiciones de fin de partida, reinicio de nivel...

        if (GameManager.gameManager_.countEnemies_ == 0)
        {
            GameManager.gameManager_.levelPoints_ = 0;            
            GameManager.gameManager_.countEnemies_ = 0;
            GameManager.gameManager_.LoadNextLevel();            
        }
        

        if (GameManager.gameManager_.lifes_ == 0) 
        {
          //TO DO gameover
        }
        if (data.Get<bool>("lost"))
        {
            GameManager.gameManager_.levelPoints_ = 0;            
            GameManager.gameManager_.countEnemies_ = 0;                                         
            GameManager.gameManager_.ResetLevel();
        }
          
        
        
    }

}