using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateHealth : Command
{
    public CommandHudUpdateHealth()
    {
        
    }

    public override bool Exec()
    {        
                
            GetStatusHud().SetHudHealthPlayer(GetStatusWorld().GetHealth()); 
        
        
        return true;
    }
    
}
