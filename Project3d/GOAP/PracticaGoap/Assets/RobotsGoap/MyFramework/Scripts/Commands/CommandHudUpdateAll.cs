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

        GetStatusWorld().commandHudUpdateHealthPlayer_.Exec();
        GetStatusWorld().commandHudUpdateHealthGuard_.Exec();
        GetStatusWorld().commandHudUpdateTotalPoints_.Exec();
        GetStatusWorld().commandHudUpdateCountEnemies_.Exec();

        ///Actualizo número de vidas.
        return true;
    }

}