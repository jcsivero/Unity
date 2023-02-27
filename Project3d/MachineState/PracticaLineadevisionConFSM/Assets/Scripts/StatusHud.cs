using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusHud : Status
{
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public CommandHudUpdateAll commandHudUpdateAll_; ///comandos comunes    
    public CommandHudUpdateHealth commandHudUpdateHealth_; ///comandos comunes

   public CommandHudUpdateTotalPoints commandHudUpdateTotalPoints_; ///comandos comunes

    public CommandHudUpdateCountEnemies commandHudUpdateCountEnemies_; ///comandos comunes



 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    [SerializeField] private Text  hudTextHealthPlayer_;
    [SerializeField] private Text  hudTextCountEnemies_;
    [SerializeField] private Text  hudTextTotalPoints_;



 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public new void Awake()
    {
        base.Awake();
        InstaciateCommands();     
        SetName("StatusHud");        
        Debug.Log("|||||||||||||| Awake StatusHud||||||||||||||||");

    }
   public new void Start()
    {
        Debug.Log("|||||||||||||| Start StatusHud||||||||||||||||");
        if (GetTarget()== null)
            SetTarget(GameObject.Find("Hud")); ///si no se ha establecido un objeto destino, por defecto para el
            ///gameobject que contendrá el HUD sera el gameobject con etiqueta "Hud"
        AppendCommand(commandHudUpdateAll_); ///se ejecutará en el primer Update() de GameManager
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {
        
        commandHudUpdateAll_ = new CommandHudUpdateAll();
        commandHudUpdateHealth_ = new CommandHudUpdateHealth();
        commandHudUpdateCountEnemies_ = new CommandHudUpdateCountEnemies();
        commandHudUpdateTotalPoints_ = new CommandHudUpdateTotalPoints();
    

    }    
    public string GetHudHealth()
    {
        return hudTextHealthPlayer_.text;
    }
    public void SetHudHealthPlayer(float drat)
    {
        hudTextHealthPlayer_.text = drat.ToString() + " %";
    }

    public string GetHudCountEnemies()
    {
        return hudTextCountEnemies_.text;
    }
    public void SetHudCountEnemies(float drat)
    {
        hudTextCountEnemies_.text = "Enemigos Actuales : " + drat.ToString();
    }

    public string GetHudTotalPoints()
    {
        return hudTextTotalPoints_.text;
    }
    public void SetHudTotalPoints(float drat)
    {
        hudTextTotalPoints_.text = "Puntos " + drat.ToString() ;
    }



}
