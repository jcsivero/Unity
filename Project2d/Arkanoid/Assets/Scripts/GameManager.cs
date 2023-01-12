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
    
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        numberOfLevels_ = SceneManager.sceneCountInBuildSettings -1; //descuento la escena del menú inicial
                        
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
}
