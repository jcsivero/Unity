using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateTotalPoints : Command
{
    public CommandHudUpdateTotalPoints()
    {
        
    }

    public override bool Exec()
    {        
                
                GetStatusHud().SetHudTotalPoints(GetStatusWorld().GetTotalPoints()); 
   
        
        return true;
    }
    
}
