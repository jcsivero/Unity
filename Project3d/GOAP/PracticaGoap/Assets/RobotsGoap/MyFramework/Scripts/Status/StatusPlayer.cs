using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusPlayer : Status
{

    ///
    ///comandos usados por esta clase. Al poder modificar sus atributos, se crea uno por cada clase
    ///
    [HideInInspector] public CommandAddOrSubHealth commandAddOrSubHealth_;
    [HideInInspector] public CommandAddOrSubLifes commandAddOrSubLifes_; 

    public GameObject bullet_;        
    [Header("Attributtes")]
    
    [SerializeField] private int healthPlayer_;

override public void  SetHealth(int health)
{     

    healthPlayer_ = health;
}
override public int  GetHealth()
{   
         
    return healthPlayer_;
}
   
    public new void Awake()
    {
        base.Awake();             
        SetName("StatusPlayer");  
              
        Debug.Log("|||||||||||||| Awake StatusPlayer||||||||||||||||");

    }
    new void  Start()
    {
        InstaciateCommands();                
        
    }

    protected new void Update()
    {
        base.Update();
        if (GetGameManager().ok_)
        {
        if (Input.GetMouseButtonDown(0))
             Fire();

        }
    }

    private void InstaciateCommands()
    {
        
        GetStatusWorld().commandHudUpdateHealthPlayer_= new CommandHudUpdateHealthPlayer(this);
        commandAddOrSubHealth_ = new CommandAddOrSubHealth(this);
        commandAddOrSubLifes_ = new CommandAddOrSubLifes(this);
        
    

    }    
    void Fire()
    {
        Vector3 position = transform.forward;
        position.y +=0.5f;
        //position.z *=2;
        //position.x *=2;        
        GameObject b = Instantiate(bullet_,transform.position + position, transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(b.transform.forward * 1000);
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
            AppendCommand(GetStatusWorld().commandHudUpdateHealthPlayer_);                                    
            ExecuteCommands();
            
            if (GetHealth() <= 0)  
            {
                Destroy(gameObject);
            }
        }
    }
}
