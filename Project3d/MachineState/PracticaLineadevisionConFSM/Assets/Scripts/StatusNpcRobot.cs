using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNpcRobot : StatusNpc
{    

    public GameObject bullet_;
    public GameObject originOfFire_;
   [SerializeField] public TextMesh  textHealthNpc_;       

    public override void InstaciateCommands()
    {
        InstaciateCommands();

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    override public void Awake()
    {        
        base.Awake();        
        InstaciateCommands(); 
     

    }
    public override void Start()
    {
       SetTarget(GetStatusWorld().GetTarget());
        
    }
}
