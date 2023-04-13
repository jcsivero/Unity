using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNpcGuard : StatusNpc
{    

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

[Header("=============== StatusNpcGuard")]
[Space(5)]      
[Header("Links to GameObjects")]

[SerializeField] private TextMesh  hudStateNpc_;       

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private const string ON_UPDATE_ALL_STATUS_NPC_GUARD = "ON_UPDATE_ALL_STATUS_NPC_GUARD";    
    private bool suscribeToOnUpdateAllStatusNpcGuard_ = false;

   
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Métodos Sobreescritos
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Eventos  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    
    override public  void Awake()
    {        
        base.Awake();       
        SetName("StatusNpcGuard");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");                         
        healthMax_ = GetHealth();
        

    }
    override public void Start()
    {
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");         
        
        SetTarget(GetLevelManager().GetActualPlayer());        
        InstaciateCommands(); 
        if (debugMode_)        
            
            
        if (!suscribeToOnUpdateAllStatusNpcGuard_)
            OnEnable(); 

        GetHudWorld().SetValue<int>("HudHealthGuard",GetHealth());
        AppendCommand("StatusNpcGuardHudUpdate",this); ///se ejecutará en el primer Update() de GameManager.                

    }
    public new void OnEnable()   
    {
        base.OnEnable();
        if (!suscribeToOnUpdateAllStatusNpcGuard_) 
            suscribeToOnUpdateAllStatusNpcGuard_ = GetManagerMyEvents().StartListening(ON_UPDATE_ALL_STATUS_NPC_GUARD,OnUpdateStatusNpcGuard); ///creo evento para actualizar  todos los StatusNpcGuards.
        ///Este evento es lanzado por GameManager,cuando ha actualizado todas las variables iniciales del estado del mundo.
        ///Después se puede utilizar para informar a todos los objetos a la vez y que se actualizen.
        ///Esto no lo hago directamente en el Start() porque no sabemos en que orden son ejecutados,y podría haber Start() que se ejecutan antes que el 
        ///Start() del GameManager, o del StatusWorld, , y entonces no tener todo actualizado, como target_ u otras variables.


        
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    public new void OnDisable()
    {
      base.OnDisable();      
      GetManagerMyEvents().StopListening(ON_UPDATE_ALL_STATUS_NPC_GUARD,OnUpdateStatusNpcGuard);
      suscribeToOnUpdateAllStatusNpcGuard_ = false;
      
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {        
        Debug.Log("----------------destruido objeto StatusNpcGuard-------------------- ");

    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "bullet")
        {                        
            GetManagerMyEvents().TriggerEvent(this.gameObject, ON_GOAP_BREAK_ONLY_THIS_GAGENT_NPC); ///ejecuto el evento que provocará un cambio en el plan GOAP            
            SetHealth(GetHealth()-10);
            GetHudWorld().SetValue<int>("healthGuard",GetHealth());
            
            if (GetHealth() <=0.0f)    
            {

                Destroy(this.gameObject);   
                 GetManagerMyEvents().TriggerEvent("ON_VICTORY");                             

            }
                
                            
        }
                                            
        
    }

    override public bool OnUpdateAllStatus() ///para sobreescribier el evento de actualizar todos los npc.
    {
        base.OnUpdateAllStatus();

        return true;
    }
    bool OnUpdateStatusNpcGuard()
    {
        
        return true;
    }
    protected new void Update()
    {
       if (GetLevelManager().paused)
        return;
        base.Update();
                    
            AppendCommand("StatusNpcGuardGoapStatesUpdate",this);
            AppendCommand("StatusNpcGuardHudUpdate",this);        
            //ExecuteCommands();

    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
private void InstaciateCommands()
{        

    
    CreateCommand("StatusNpcGuardHudUpdate",new CommandStatusNpcGuardHudUpdate(this), this);
    CreateCommand("StatusNpcGuardGoapStatesUpdate",new CommandStatusNpcGuardGoapStatesUpdate(this), this);
    
}

public TextMesh GetHud()
{
    return hudStateNpc_;
}
public void SetHud(string draft, Color color)
{
    hudStateNpc_.color = color;
    hudStateNpc_.text = draft;    
    
}


override public void HealthRecovery()
{
    if (GetHealth() < healthMax_) ///para no sobrepasar la recarga de vida máxima
    {
        SetHealth(GetHealth()+10);
        GetHudWorld().SetValue<int>("HudHealthGuard",GetHealth());
        

    }
}
override public void StartHealthRecovery()
{
    InvokeRepeating("HealthRecovery", 0.5f, 2.0f);
}
override public void StopHealthRecovery()
{
    CancelInvoke("HealthRecovery");
}
}
