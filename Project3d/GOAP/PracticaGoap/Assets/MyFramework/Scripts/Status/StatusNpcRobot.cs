using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNpcRobot : StatusNpc
{    

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

 public CommandHudUpdateStatusNpcRobot commandHudUpdateStatusNpcRobot_;
 public CommandNpcGoapStatesRobotUpdate commandNpcGoapStatesRobotUpdate_;


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

[Header("=============== StatusNpcRobot")]
[Space(5)]      
[Header("Links to GameObjects")]
[SerializeField] private GameObject bullet_;
[SerializeField] private GameObject originOfFire_;
[SerializeField] private TextMesh  textHealthNpc_;       

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
    
    public new void Awake()
    {        
        base.Awake();        
        InstaciateCommands(); 
        SetName("StatusNpcRobot");        
        healthMax_ = GetHealth();

        Debug.Log("|||||||||||||| Awake StatusNpcRobot||||||||||||||||");

    }
    public new void Start()
    {
        base.Start();
       Debug.Log("|||||||||||||| Start StatusNpcRobot||||||||||||||||");
        if (!suscribeToOnUpdateAllStatusNpcRobot_)
            OnEnable(); 
        AppendCommand(commandHudUpdateStatusNpcRobot_); ///se ejecutará en el primer Update() de GameManager.
        GetStatusWorld().SetCountEnemies(1);
        AppendCommand(GetStatusHud().commandHudUpdateCountEnemies_);

    }
    public new void OnEnable()   
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
    public new void OnDisable()
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
            Debug.Log("colision detectada daño en robot " + GetHealth().ToString());
            commandAddOrSubHealth_.Set(-10);                        
            AppendCommand(commandAddOrSubHealth_);                                    
            
            transform.LookAt(GetTarget().transform.position); //me giro hacia el jugador que me ha disparado.
            
            if (GetHealth() <=0.0f)    
            {
                Destroy(this.gameObject);
                GetStatusWorld().SetTotalPoints(5);
                GetStatusWorld().SetCountEnemies(-1);
                AppendCommand(GetStatusHud().commandHudUpdateCountEnemies_);
                AppendCommand(GetStatusHud().commandHudUpdateTotalPoints_);

            }
                
                            
        }
                     
                       
        
    }

    bool OnUpdateStatusNpcRobot()
    {
        SetTarget(GetStatusWorld().GetTarget());
        return true;
    }
    protected new void Update()
    {
        base.Update();
        if (GetGameManager().ok_)
        {
            AppendCommand(commandHudUpdateStatusNpcRobot_);
            AppendCommand(commandNpcGoapStatesRobotUpdate_);
            ExecuteCommands();


        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
private void InstaciateCommands()
{        
    commandHudUpdateStatusNpcRobot_ = new CommandHudUpdateStatusNpcRobot(this);
    commandNpcGoapStatesRobotUpdate_ = new CommandNpcGoapStatesRobotUpdate(this);
}

public TextMesh GetHudHealth()
{
    return textHealthNpc_;
}
public void SetHudHealth(int draft)
{
    textHealthNpc_.text = draft.ToString() + " %";
}
override public void Fire()
    {
        GameObject b = Instantiate(bullet_, originOfFire_.transform.position, originOfFire_.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(originOfFire_.transform.forward * 700);
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
    {
        commandAddOrSubHealth_.Set(10);
        AppendCommand(commandAddOrSubHealth_);    

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
