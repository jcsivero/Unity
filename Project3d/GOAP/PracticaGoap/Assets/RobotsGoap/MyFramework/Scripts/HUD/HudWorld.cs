using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HudWorld : Hud
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
////Eventos  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public new void Awake()
    {
        base.Awake();           
        
        Debug.Log("|||||||||||||| Awake HudWorld||||||||||||||||");

    }
   public new void Start()
    {
        base.Start();

        InstaciateCommands();  

        if (debugMode_)
            Debug.Log("|||||||||||||| Start StatusHud||||||||||||||||");
        SetHudWorld(this);
        
     
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {


    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     


    override public Image GetHudWeapon()
    {
        return hudWeapon_;
    }
    override public void SetHudWeapon(Color color )
    {
        hudWeapon_.color = color;
    }

    override public Image GetHudKey()
    {
        return hudKey_;
    }
    override public void SetHudKey(Color color )
    {
        hudKey_.color = color;
    }
    override public string GetHudHealthPlayer()
    {
        return hudTextHealthPlayer_.text;
    }
    override public void SetHudHealthPlayer(float drat)
    {
        hudTextHealthPlayer_.text = drat.ToString() + " %";
    }


   override public string GetHudHealthGuard()
    {
        return hudTextHealthGuard_.text;
    }
    override public void SetHudHealthGuard(float drat)
    {
        hudTextHealthGuard_.text = drat.ToString() + " %";
    }   
    override public string GetHudCountEnemies()
    {
        return hudTextCountEnemies_.text;
    }
override     public void SetHudCountEnemies(float drat)
    {
        hudTextCountEnemies_.text = "Enemigos Actuales : " + drat.ToString();
    }

    override public string GetHudTotalPoints()
    {
        return hudTextTotalPoints_.text;
    }
    override public void SetHudTotalPoints(float drat)
    {
        hudTextTotalPoints_.text = "Puntos " + drat.ToString() ;
    }

   
}
