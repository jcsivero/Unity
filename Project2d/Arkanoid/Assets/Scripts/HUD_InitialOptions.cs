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
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        Debug.Log("Pendiente de cargar valores para continuar partida");
    }
}
