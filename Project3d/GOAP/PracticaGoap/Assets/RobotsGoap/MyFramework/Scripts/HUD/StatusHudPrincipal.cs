using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StatusHudPrincipal : Status
{
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    [SerializeField] private Text  hudTextHealthPlayer_;
    [SerializeField] private Text  hudTextCountEnemies_;
    [SerializeField] private Text  hudTextTotalPoints_;
    [SerializeField] private Text  hudTextHealthGuard_;
   
    [SerializeField] private Image  hudWeapon_;
    [SerializeField] private Image  hudKey_;

  

 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
public string GetHudHealthPlayer()
    {
        return hudTextHealthPlayer_.text;
    }
    public void SetHudHealthPlayer(float drat)
    {
        hudTextHealthPlayer_.text = drat.ToString() + " %";
    }


   public string GetHudHealthGuard()
    {
        return hudTextHealthGuard_.text;
    }
    public void SetHudHealthGuard(float drat)
    {
        hudTextHealthGuard_.text = drat.ToString() + " %";
    }

    public Image GetHudWeapon()
    {
        return hudWeapon_;
    }
    public void SetHudWeapon(Color color )
    {
        hudWeapon_.color = color;
    }

        public Image GetHudKey()
    {
        return hudKey_;
    }
    public void SetHudKey(Color color )
    {
        hudKey_.color = color;
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

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public new void Awake()
    {
        base.Awake();           
        SetName("StatusHudInitialOptions");        
        Debug.Log("|||||||||||||| Awake StatusHud||||||||||||||||");

    }
   public new void Start()
    {
        base.Start();
        InstaciateCommands();  

        if (debugMode_)
            Debug.Log("|||||||||||||| Start StatusHud||||||||||||||||");
        
        GetStatusHud().hudPrincipal_ = this;
     
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {


    }    

   
}
