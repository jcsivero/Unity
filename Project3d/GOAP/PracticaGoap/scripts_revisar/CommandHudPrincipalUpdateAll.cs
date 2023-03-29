using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudPrincipalUpdateAll : Command
{
    
    public CommandHudPrincipalUpdateAll()
    {
        
    }

    public override bool Exec()
    {        
        
        ///Actualizo vida del Player

/*        GetStatusHud().hudPrincipal_.commandHudUpdateHealthPlayer_.Exec();
        GetStatusHud().hudPrincipal_.commandHudUpdateHealthGuard_.Exec();
        GetStatusHud().hudPrincipal_.commandHudUpdateTotalPoints_.Exec();
        GetStatusHud().hudPrincipal_.commandHudUpdateCountEnemies_.Exec();
*/
        ///Actualizo número de vidas.
        return true;
    }

}