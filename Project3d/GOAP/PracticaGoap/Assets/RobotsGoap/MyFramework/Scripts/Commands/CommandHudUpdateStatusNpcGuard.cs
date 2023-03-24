using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateStatusNpcGuard : Command
{    
    private StatusNpcGuard status_;
    public CommandHudUpdateStatusNpcGuard(StatusNpcGuard status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        
        string newHudNpc;
        Color color = Color.blue; ///por defecto HUD color azul.

        newHudNpc = status_.GetHealth().ToString() + "%";                
        
       if ((status_.GetGoapAgent().currentAction_ != null) && (status_.debugMode_))
        {
            newHudNpc += "\n" + status_.GetGoapAgent().currentAction_.actionName;

            if (status_.GetGoapAgent().currentAction_.actionName == "Flee") 
                color = Color.yellow;

            if (status_.GetGoapAgent().currentAction_.actionName == "GoToKillGirld") 
                color = Color.black;

            if (status_.GetGoapAgent().currentAction_.actionName == "HideFollow")             
                color = Color.red;
                             

            if (status_.GetGoapAgent().currentAction_.actionName == "Cure") 
                color = Color.green;


        }

        status_.SetHud(newHudNpc,color);
        
        ///hago que el HUD del NPC siempre esté visible para el Jugador
        Vector3 draft = (status_.GetHud().transform.position -status_.GetTarget().transform.position ).normalized;
        draft = draft + status_.GetHud().transform.position;        
        status_.GetHud().transform.LookAt(draft);
                
                        
        return true;
    }

}
