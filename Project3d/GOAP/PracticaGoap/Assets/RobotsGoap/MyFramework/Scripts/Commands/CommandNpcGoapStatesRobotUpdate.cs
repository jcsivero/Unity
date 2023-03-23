using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNpcGoapStatesRobotUpdate : Command
{
 
    private StatusNpcRobot status_;
    public CommandNpcGoapStatesRobotUpdate(StatusNpcRobot status)
    {
        status_ =  status;
    }

    public override bool Exec()
    {        
        ///Actualizo o crea las npcGoapStates
        Vector3 direction =  status_.GetTarget().transform.position - status_.transform.position;                
        //direction.z = 0; //solo me interesa el ángulo en x
        direction.y = 0; //solo me interesa el ángulo en x
        Vector3 npcDirection = status_.transform.forward;

        status_.GetGoapAgent().npcGoapStates_.SetOrAddState("angle",GenericData.Create<float>(Vector3.Angle(direction, npcDirection)));
        status_.GetGoapAgent().npcGoapStates_.SetOrAddState("distance",GenericData.Create<float>(direction.magnitude));
        status_.GetGoapAgent().npcGoapStates_.SetOrAddState("health",GenericData.Create<int>(status_.GetHealth()));
        status_.GetGoapAgent().npcGoapStates_.SetOrAddState("visibleTarget",GenericData.Create<bool>(GetAIController().CanSeeTarget(status_,status_.GetTarget())));            
        if (status_.debugMode_)
        {
            Debug.Log("distance " + status_.GetGoapAgent().npcGoapStates_.GetState("distance").GetValue<float>().ToString());
            Debug.Log("angle " + status_.GetGoapAgent().npcGoapStates_.GetState("angle").GetValue<float>().ToString());
            Debug.Log("visibleTarget" + status_.GetGoapAgent().npcGoapStates_.GetState("visibleTarget").GetValue<bool>().ToString());

        }

        return true;
    }

}
