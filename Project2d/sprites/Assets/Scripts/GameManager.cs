using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public float moveSpeedX_ = 10;
    [SerializeField] public float moveSpeedY_ = 10;
    public static GameManager gameManager_;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
                       
        if (gameManager_!= null && gameManager_ != this)
            Destroy(gameObject);
        else
            gameManager_ = this;
            Object.DontDestroyOnLoad(gameObject);

    }

    

}
