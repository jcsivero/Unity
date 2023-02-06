using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateAllHud : Command
{
    
    public CommandUpdateAllHud()
    {
        
    }

    public override bool Exec()
    {        
        
        ///Actualizo vida del Player

        GetStatusHud().commandUpdateHealthHud_.Exec();
        
        ///Actualizo número de vidas.
        return true;
    }

}