using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHudUpdateStatusNpcRobot : Command
{    
    private StatusNpcRobot status_;
    public CommandHudUpdateStatusNpcRobot(StatusNpcRobot status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        
        string newHudNpc;
        newHudNpc = status_.GetHealth().ToString() + "%";
        
        newHudNpc += "\n" + status_.GetGoapAgent().currentAction_.actionName;
        status_.SetHud(newHudNpc);
        
        ///hago que el HUD del NPC siempre esté visible para el Jugador
        Vector3 draft = (status_.GetHud().transform.position -status_.GetTarget().transform.position ).normalized;
        draft = draft + status_.GetHud().transform.position;        
        status_.GetHud().transform.LookAt(draft);
                
                        
        return true;
    }

}
