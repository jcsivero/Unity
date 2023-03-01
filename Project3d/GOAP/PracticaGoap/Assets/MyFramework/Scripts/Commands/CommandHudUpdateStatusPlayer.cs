using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateStatusPlayer : Command
{
    private StatusPlayer status_;
    public CommandHudUpdateStatusPlayer(StatusPlayer status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        
        GetStatusHud().commandHudUpdateHealth_.Exec();
                        
        return true;
    }

}
