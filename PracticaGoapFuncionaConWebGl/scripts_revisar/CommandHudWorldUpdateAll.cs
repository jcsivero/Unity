using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudWorldUpdateAll : Command
{
    
    public CommandHudWorldUpdateAll()
    {
        
    }

    public override bool Exec()
    {        
        
        ///Actualizo vida del Player

        /*GetWorld().commandHudUpdateHealthPlayer_.Exec();
        GetWorld().commandHudUpdateHealthGuard_.Exec();
        GetWorld().commandHudUpdateTotalPoints_.Exec();
        GetWorld().commandHudUpdateCountEnemies_.Exec();
*/
        ///Actualizo número de vidas.
        return true;
    }

}