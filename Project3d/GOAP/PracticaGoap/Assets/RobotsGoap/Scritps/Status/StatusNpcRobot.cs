using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNpcRobot : StatusNpc
{    



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

[Header("=============== StatusNpcRobot")]
[Space(5)]      
[Header("Links to GameObjects")]
[SerializeField] private GameObject bullet_;
[SerializeField] private GameObject originOfFire_;
[SerializeField] private TextMesh  hudStateNpc_;       

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables de trigger o suscriber a eventos
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private const string ON_UPDATE_ALL_STATUS_NPC_ROBOT = "ON_UPDATE_ALL_STATUS_NPC_ROBOT";    
    private bool suscribeToOnUpdateAllStatusNpcRobot_ = false;
    
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
        SetName("StatusNpcRobot");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||"); 
        
        
        healthMax_ = GetHealth();
        

    }
    override public  void Start()
    {
        
        base.Start();
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");         
        SetTarget(GetLevelManager().GetActualPlayer());
        InstaciateCommands(); 

       
        if (!suscribeToOnUpdateAllStatusNpcRobot_)
            OnEnable();         
        
        GetWorld().SetOrAddCountEnemies(1);
        GetHudWorld().SetValue<int>("HudCountEnemies",GetWorld().GetCountEnemies());
        AppendCommand("StatusNpcRobotHudUpdate",this); ///se ejecutará en el primer Update() de GameManager.                

    }
    override public void OnEnable()   
    {
        base.OnEnable();
        if (!suscribeToOnUpdateAllStatusNpcRobot_) 
            suscribeToOnUpdateAllStatusNpcRobot_ = GetManagerMyEvents().StartListening(ON_UPDATE_ALL_STATUS_NPC_ROBOT,OnUpdateStatusNpcRobot); ///creo evento para actualizar  todos los StatusNpcRobots.
        ///Este evento es lanzado por GameManager,cuando ha actualizado todas las variables iniciales del estado del mundo.
        ///Después se puede utilizar para informar a todos los objetos a la vez y que se actualizen.
        ///Esto no lo hago directamente en el Start() porque no sabemos en que orden son ejecutados,y podría haber Start() que se ejecutan antes que el 
        ///Start() del GameManager, o del StatusWorld, , y entonces no tener todo actualizado, como target_ u otras variables.


        
    }
        /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    override public void OnDisable()
    {
      base.OnDisable();      
      GetManagerMyEvents().StopListening(ON_UPDATE_ALL_STATUS_NPC_ROBOT,OnUpdateStatusNpcRobot);
      suscribeToOnUpdateAllStatusNpcRobot_ = false;
      
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {        
        Debug.Log("----------------destruido objeto StatusNpcRobot-------------------- ");

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
            Debug.Log("colision detectada daño en robot " + GetHealth().ToString());
            SetHealth(GetHealth()-10);
            
            transform.LookAt(GetTarget().transform.position); //me giro hacia el jugador que me ha disparado.
            
            if (GetHealth() <=0.0f)    
            {                
                GetWorld().SetOrAddCountEnemies(-1);        
                GetHudWorld().SetValue<int>("HudCountEnemies",GetWorld().GetCountEnemies());

                GetWorld().SetOrAddTotalPoints(5);
                GetHudWorld().SetValue<int>("HudTotalPoints",GetWorld().GetTotalPoints());                 
                Destroy(this.gameObject);

            }
                
                            
        }
                     
                       
        
    }

    public override bool OnUpdateAllStatus()
    {
         base.OnUpdateAllStatus();
         ///código propio de esta clase.
        return true;
    }
    bool OnUpdateStatusNpcRobot()
    {
        
        return true;
    }
    protected new void Update()
    {
        if (GetLevelManager().paused)
            return;

        base.Update();
            AppendCommand("StatusNpcRobotGoapStatesUpdate",this);
            AppendCommand("StatusNpcRobotHudUpdate",this);    
            //ExecuteCommands();

 }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
private void InstaciateCommands()
{        
    CreateCommand("StatusNpcRobotHudUpdate",new CommandStatusNpcRobotHudUpdate(this), this);
    CreateCommand("StatusNpcRobotGoapStatesUpdate",new CommandStatusNpcRobotGoapStatesUpdate(this), this);
    
    
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

override public void Fire()
{
    GameObject b = Instantiate(bullet_, originOfFire_.transform.position, originOfFire_.transform.rotation);
    b.GetComponent<Rigidbody>().AddForce(originOfFire_.transform.forward * 1000);
}
override public void StopFiring()
{
    CancelInvoke("Fire");
}
override public void StartFiring()
{
    InvokeRepeating("Fire", 0.5f, 0.5f);
}    

override public void HealthRecovery()
{
    if (GetHealth() < healthMax_) ///para no sobrepasar la recarga de vida máxima
        SetHealth(GetHealth()+10);
        
            
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
