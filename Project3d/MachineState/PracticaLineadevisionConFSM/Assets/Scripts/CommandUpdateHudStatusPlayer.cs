using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateHudStatusPlayer : Command
{
    private StatusPlayer status_;
    public CommandUpdateHudStatusPlayer(StatusPlayer status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        GetStatusWorld().SetHealth(status_.GetHealth());
        GetStatusHud().commandUpdateHealthHud_.Exec();
                        
        return true;
    }

}
