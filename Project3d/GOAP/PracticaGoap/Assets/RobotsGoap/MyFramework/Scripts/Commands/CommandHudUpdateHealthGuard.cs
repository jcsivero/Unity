using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateHealthGuard : Command
{
    private Status status_;
    public CommandHudUpdateHealthGuard(Status status)
    {
        status_ = status;    
    }

    public override bool Exec()
    {        
                
            GetStatusHud().SetHudHealthGuard(status_.GetHealth()); 
        
        
        return true;
    }
    
}
