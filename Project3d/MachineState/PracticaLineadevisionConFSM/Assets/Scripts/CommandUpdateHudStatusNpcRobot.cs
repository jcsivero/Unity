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
        status_.SetHudHealth();
                        
        return true;
    }

}
