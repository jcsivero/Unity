using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD_InitialOptions : BaseMono
{
    // Start is called before the first frame update
    public void OnPlay()
    {
        GetStatusWorld().activeLevel_ = 1;
        SceneManager.LoadScene(GetStatusWorld().activeLevel_);
    }

    public void ContinueGame()
    {
        Debug.Log("Pendiente de cargar valores para continuar partida");
    }
}
