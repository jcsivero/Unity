using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateAll : Command
{
    
    public CommandHudUpdateAll()
    {
        
    }

    public override bool Exec()
    {        
        
        ///Actualizo vida del Player

        GetStatusHud().commandHudUpdateHealth_.Exec();
        GetStatusHud().commandHudUpdateCountEnemies_.Exec();
        GetStatusHud().commandHudUpdateTotalPoints_.Exec();
        
        ///Actualizo número de vidas.
        return true;
    }

}