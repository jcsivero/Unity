using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD_InitialOptions : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPlay()
    {
        GameManager.gameManager_.statusWorld_.activeLevel_ = 1;
        SceneManager.LoadScene(GameManager.gameManager_.statusWorld_.activeLevel_);
    }

    public void ContinueGame()
    {
        Debug.Log("Pendiente de cargar valores para continuar partida");
    }
}
