using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HudWorldInitialOptions : HudWorld
{
    // Start is called before the first frame update
    public void OnOneEnemy()
    {
        GetWorld().activeLevel_ = 1;
        SceneManager.LoadScene(GetWorld().activeLevel_);
    }

    public void OnMultiplesEnemies()
    {
        GetWorld().activeLevel_ = 2;
        SceneManager.LoadScene(GetWorld().activeLevel_);


    }
 
    
}
