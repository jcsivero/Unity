using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateKey : Command
{
    public CommandHudUpdateKey()
    {
        
    }

    public override bool Exec()
    {        
                
                GetStatusHud().SetHudKey(Color.white);        
        
        return true;
    }
    
}
