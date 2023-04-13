using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStatusNpcRobotHudUpdate : Command
{    
    private StatusNpcRobot status_;
    public CommandStatusNpcRobotHudUpdate(StatusNpcRobot status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
          if (status_ == null)
            return false;
        string newHudNpc="";
        Color color = Color.blue; ///por defecto HUD color azul.

        newHudNpc = status_.GetHealth().ToString() + "%";                
        
        if ((status_.GetGoapAgent().currentAction_ != null) && (status_.debugMode_))
        {
            newHudNpc += "\n" + status_.GetGoapAgent().currentAction_.actionName;

            if (status_.GetGoapAgent().currentAction_.actionName == "Chase") 
                color = Color.yellow;

            if (status_.GetGoapAgent().currentAction_.actionName == "Attack") 
                color = Color.black;

            if (status_.GetGoapAgent().currentAction_.actionName == "Hide") 
                color = Color.red;

            if (status_.GetGoapAgent().currentAction_.actionName == "Repair") 
                color = Color.green;
            
            if (status_.GetGoapAgent().currentAction_.actionName == "Repair") 
                color = Color.cyan;


            if (status_.GetGoapAgent().currentAction_.actionName == "Patrol") ///si estoy en modo Patrol, indico si estoy en wander o waypoints.
            {
                if ((status_.wayPointTag_.Length == 0) || (status_.wayPointTag_ == "Tag No founded"))
                    ///si estoy en modo wander
                    newHudNpc += "\n" +  "Modo Wander";
                else
                    newHudNpc += "\n" +  "To WayPoints " + status_.GetWayPointCurrent().ToString();
            }
            

        }

        status_.SetHud(newHudNpc,color);
        
        ///hago que el HUD del NPC siempre esté visible para el Jugador
        Vector3 draft = (status_.GetHud().transform.position -status_.GetTarget().transform.position ).normalized;
        draft = draft + status_.GetHud().transform.position;        
        status_.GetHud().transform.LookAt(draft);
                
                        
        return true;
    }

}
