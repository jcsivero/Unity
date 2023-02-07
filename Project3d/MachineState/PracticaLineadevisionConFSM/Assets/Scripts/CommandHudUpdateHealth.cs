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
                
        if (GetStatusHud().GetHealth() != GetStatusWorld().GetHealth() )
        {
                GetStatusHud().SetHealth(GetStatusWorld().GetHealth());
                GetStatusHud().SetHudHealthPlayer(GetStatusHud().GetHealth()); 
                
        }
        
        
        return true;
    }
    
}
