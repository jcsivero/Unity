using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusPlayer : Status
{

    ///
    ///comandos usados por esta clase. Al poder modificar sus atributos, se crea uno por cada clase
    ///
    public GameObject bullet_; 
    public GameObject originOfFire_;

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
   
    override public  void Awake()
    {
        base.Awake();             
        SetName("StatusPlayer");  
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");
        
              
        

    }
    override public   void  Start()
    {
        base.Start();                
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");

        InstaciateCommands();                
        
    }

    protected new void Update()
    {
        base.Update();
        
        if (Input.GetMouseButtonDown(0))
             Fire();
        
    }

    private void InstaciateCommands()
    {
        
        
    

    }    
    void Fire()
    {
        GameObject b = Instantiate(bullet_,originOfFire_.transform.position, originOfFire_.transform.rotation);
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
            SetHealth(GetHealth()-10);
            GetHudWorld().SetValue<int>("HudHealthPlayer",GetHealth());
            
            
            if (GetHealth() <= 0)  
            {
                GetManagerMyEvents().TriggerEvent("OnLose");
                Destroy(gameObject);
            }
        }
    }
}
