using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNpcGoapStatesGuardUpdate : Command
{
 
    private StatusNpcGuard status_;
    public CommandNpcGoapStatesGuardUpdate(StatusNpcGuard status)
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
        
        status_.GetGoapAgent().npcGoapStates_.SetOrAddState("distance",GenericData.Create<float>(direction.magnitude));
        status_.GetGoapAgent().npcGoapStates_.SetOrAddState("health",GenericData.Create<int>(status_.GetHealth()));
        if (status_.debugMode_)
        {
            Debug.Log("distance " + status_.GetGoapAgent().npcGoapStates_.GetState("distance").GetValue<float>().ToString());

        }

        return true;
    }

}
