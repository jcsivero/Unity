using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudWorldInitialOptions : HudWorldBase
{

[SerializeField] private Text  hudTextDebug_;
    override public  void Awake()
    {
        base.Awake();           
        SetName("HudWorldInitialOptions");
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        InitializeValues();    ///inicializo las variables propias de esta clase en AWake, para que otros objetos puedas actualizarlas desde Start, ya que 
        ///si pongo esta inicialización en Start, puede ejecutarse depués de otros métodos start que ya hayan puesto datos.

    }
   override public  void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        
    
        
        InstaciateCommands();  
      
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;         
        SetHudDebug();  
     
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {


    }    
    private void InitializeValues()
    {
     
        ///Creo e inicializo variables con los valores actuales del Hud, por si fueron puestos desde el inspector.
    }
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
    
    public void OnOffDebug()
    {
        if (GetGameManager().debugModeForce_ == DebugModeForce.debug)
            GetGameManager().debugModeForce_ = DebugModeForce.noDebug;
        else if (GetGameManager().debugModeForce_ == DebugModeForce.noDebug)
            GetGameManager().debugModeForce_ = DebugModeForce.debug;

        SetHudDebug();            
    }

    private void SetHudDebug()
    {
          if (GetGameManager().debugModeForce_ == DebugModeForce.debug)
            hudTextDebug_.text = "Debug ON";
        else if (GetGameManager().debugModeForce_ == DebugModeForce.noDebug)
            hudTextDebug_.text = "Debug OFF";
            
    }
    public void Quit()
    {
        
        Debug.Log("Saliendo");
        Application.Quit(0);
    }
}
