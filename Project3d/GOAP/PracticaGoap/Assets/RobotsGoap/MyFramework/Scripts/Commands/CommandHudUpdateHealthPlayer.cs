using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateHealthPlayer : Command
{
    private Status status_;
    public CommandHudUpdateHealthPlayer(Status status)
    {
        status_ = status;    
    }

    public override bool Exec()
    {        
                
            GetStatusHud().SetHudHealthPlayer(status_.GetHealth()); 
        
        
        return true;
    }
    
}
