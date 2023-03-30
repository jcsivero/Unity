using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HudWorld : HudWorldBase
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
    override public  void Awake()
    {
        base.Awake();           
        SetName("HudWorld1");
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        InitializeValues();    ///inicializo las variables propias de esta clase en AWake, para que otros objetos puedas actualizarlas desde Start, ya que 
        ///si pongo esta inicialización en Start, puede ejecutarse depués de otros métodos start que ya hayan puesto datos.

    }
   override public  void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");
        
    
        
        InstaciateCommands();  
      
                
     
    }    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void InstaciateCommands()
    {


    }    
    private void InitializeValues()
    {
        SetValue<Color>("HudWeaponColor",hudWeapon_.color);
        SetValue<Color>("HudKeyColor",hudKey_.color);
        
        SetValue<int>("HudHealthPlayer",0);
        SetValue<int>("HudHealthGuard",0);
        SetValue<int>("HudTotalPoints",0);
        SetValue<int>("HudCountEnemies",0);
        ///Creo e inicializo variables con los valores actuales del Hud, por si fueron puestos desde el inspector.
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////     
    override public void UpdateHud()
    {
        if (updatePending_)
        {
            SetHudCountEnemies();
            SetHudHealthGuard();
            SetHudHealthPlayer();
            SetHudKey();
            SetHudWeapon();
            SetHudTotalPoints();
            updatePending_ = false;

        }
    }


     public void SetHudWeapon( )
    {

        hudWeapon_.color = GetValue<Color>("HudWeaponColor");
    }

     public void SetHudKey( )
    {
        hudKey_.color = GetValue<Color>("HudKeyColor");
    }
     
 
     public void SetHudHealthPlayer()
    {
        hudTextHealthPlayer_.text = GetValue<int>("HudHealthPlayer").ToString() + " %";
        
    }

    public void SetHudHealthGuard()
    {
        hudTextHealthGuard_.text =GetValue<int>("HudHealthGuard").ToString() + " %";
    }   
    public string GetHudCountEnemies()
    {
        return hudTextCountEnemies_.text;
    }
     public void SetHudCountEnemies()
    {
        hudTextCountEnemies_.text =  "Enemigos Actuales : " + GetValue<int>("HudCountEnemies").ToString();
    }
  
    public void SetHudTotalPoints()
    {
        hudTextTotalPoints_.text = "Puntos " + GetValue<int>("HudTotalPoints").ToString() ;
    }

   
}
