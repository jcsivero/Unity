using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusPlayer : Status
{

    public GameObject bullet_;    
    public CommandHudUpdateStatusPlayer commandHudUpdateStatusPlayer_;
    
    public new void Awake()
    {
        base.Awake();
        InstaciateCommands();     
        SetName("StatusPlayer");        
        Debug.Log("|||||||||||||| Awake StatusPlayer||||||||||||||||");

    }
    new void  Start()
    {
                
        AppendCommand(commandHudUpdateStatusPlayer_);
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
        
        commandHudUpdateStatusPlayer_ = new CommandHudUpdateStatusPlayer(this);
        
    

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
            AppendCommand(commandHudUpdateStatusPlayer_);                                    
            ExecuteCommands();
            
            if (GetHealth() <= 0)  
            {
                Destroy(gameObject);
            }
        }
    }
}
