using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : BaseMono
{
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public bool debugMode_;

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     

 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        Debug.Log("|||||||||||||| Awake Hud||||||||||||||||");

    }
   public void Start()
    {

        if (GetGameManager().debugModeForce_ == DebugModeForce_.debug)
            debugMode_ = true;

        if (GetGameManager().debugModeForce_ == DebugModeForce_.noDebug)
            debugMode_ = false;

        if (debugMode_)
            Debug.Log("|||||||||||||| Start StatusHud||||||||||||||||");
        
        if (!ReadyEngine())
            Debug.Log("Engine no funcional. LevelManager no cargado o función GetInitialInformation de LevelManager no ejecutada con éxito");

        InstaciateCommands();  
        
        //if (GetTarget()== null)
          //  SetTarget(GameObject.Find("Hud")); ///si no se ha establecido un objeto destino, por defecto para el
            ///gameobject que contendrá el HUD sera el gameobject con etiqueta "Hud"     
        
        
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {


    }    

 

    virtual public Image GetHudWeapon()
    {
        return null;
    }
    virtual public void SetHudWeapon(Color color )
    {
        
    }

    virtual public Image GetHudKey()
    {
        return null;
    }
    virtual public void SetHudKey(Color color )
    {
        
    }

     virtual public string GetHudHealthPlayer()
    {
        return null;
    }
    virtual public void SetHudHealthPlayer(float drat)
    {
        
    }


   virtual public string GetHudHealthGuard()
    {
        return null;
    }
    virtual public void SetHudHealthGuard(float drat)
    {
        
    }   
virtual public string GetHudCountEnemies()
    {
        return null;
    }
virtual public void SetHudCountEnemies(float drat)
    {
        
    }

virtual public string GetHudTotalPoints()
    {
        return null;
    }
virtual public void SetHudTotalPoints(float drat)
    {
       
    }

}
