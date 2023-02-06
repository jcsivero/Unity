using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateHudStatusNpcRobot : Command
{    
    private StatusNpcRobot status_;
    public CommandUpdateHudStatusNpcRobot(StatusNpcRobot status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        
        status_.SetHudHealth(status_.GetHealth());        
        status_.GetHudHealth().transform.LookAt(status_.GetTarget().transform);
                
                        
        return true;
    }

}
