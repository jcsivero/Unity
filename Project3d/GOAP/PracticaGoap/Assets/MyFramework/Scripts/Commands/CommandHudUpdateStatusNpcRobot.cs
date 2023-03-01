using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateStatusNpcRobot : Command
{    
    private StatusNpcRobot status_;
    public CommandHudUpdateStatusNpcRobot(StatusNpcRobot status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        
        status_.SetHudHealth(status_.GetHealth());   
        Vector3 draft = (  status_.GetHudHealth().transform.position -status_.GetTarget().transform.position ).normalized;
        draft = draft + status_.GetHudHealth().transform.position;        
        status_.GetHudHealth().transform.LookAt(draft);
                
                        
        return true;
    }

}
