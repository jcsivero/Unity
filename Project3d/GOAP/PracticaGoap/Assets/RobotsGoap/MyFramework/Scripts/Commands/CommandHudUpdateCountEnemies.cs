using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateCountEnemies : Command
{
    public CommandHudUpdateCountEnemies()
    {
        
    }

    public override bool Exec()
    {        
                
                GetStatusHud().hudPrincipal_.SetHudCountEnemies(GetStatusWorld().GetCountEnemies());                
        
        
        return true;
    }
    
}
