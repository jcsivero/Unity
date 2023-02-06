using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNpcRobot : StatusNpc
{    

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables públicas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

 public CommandUpdateHudStatusNpcRobot commandUpdateHudStatusNpcRobot_;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Variables privadas propias de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    override public void Awake()
    {        
        base.Awake();        
        InstaciateCommands(); 
        SetName("StatusNpcRobot");

    }
    public override void Start()
    {
        base.Start();
       
        if (!suscribeToOnUpdateAllStatusNpcRobot_)
            OnEnable(); 
               
        
    }
    public override void OnEnable()   
    {
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
    public override void OnDisable()
    {
      base.OnDisable();
      Debug.Log("Unsuscribe Trigger " +ON_UPDATE_ALL_STATUS_NPC_ROBOT );
      GetManagerMyEvents().StopListening(ON_UPDATE_ALL_STATUS_NPC_ROBOT,OnUpdateStatusNpcRobot);
      suscribeToOnUpdateAllStatusNpcRobot_ = false;
      
    }
       /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        OnDisable();
        Debug.Log("destruido objeto AIController " + ON_UPDATE_ALL_STATUS_NPC_ROBOT);

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
            commandAddOrSubHealth_.Set(-10);
            AppendCommand(commandAddOrSubHealth_);
            AppendCommand(commandUpdateHudStatusNpcRobot_);
            ExecuteCommands();
            
            if (GetHealth() <=0)    
                Destroy(this.gameObject);
                            
        }
                     
                       
        
    }

    bool OnUpdateStatusNpcRobot()
    {
        SetTarget(GetStatusWorld().GetTarget());
        return true;
    }
    void Update()
    {
        if (GetGameManager().ok_)
        {
            
        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////Funciones exclusivas  de esta clase
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public override void InstaciateCommands()
{        
    commandUpdateHudStatusNpcRobot_ = new CommandUpdateHudStatusNpcRobot(this);
}

public void SetHudHealth()
{
    textHealthNpc_.text = GetHealth().ToString() + " %";
}
public void Fire()
    {
        GameObject b = Instantiate(bullet_, originOfFire_.transform.position, originOfFire_.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(originOfFire_.transform.forward * 500);
    }
public void StopFiring()
{
    CancelInvoke("Fire");
}
public void StartFiring()
{
    InvokeRepeating("Fire", 0.5f, 0.5f);
}    
}
