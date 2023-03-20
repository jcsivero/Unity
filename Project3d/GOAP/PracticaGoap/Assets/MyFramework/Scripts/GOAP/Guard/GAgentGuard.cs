using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAgentGuard : GAgent
{
  // Start is called before the first frame update
    new void Start()
    {
        base.Start();
       
  
    }
    
    override public void AddActions()
    {
        ///agrego acction de patrullaje
        //var gActionPatrolMode = gameObject.AddComponent<GActionPatrolMode>(); ///agregando así el componente, las precondiciones y efectos están vacios,así que los especificamos        
        //gActionPatrolMode.AddPreConditions("hola",GenericData.Create<int>(2));
        //gActionPatrolMode.AddEffects("patrolMode",GenericData.Create<int>(8));

    }
    
    override public void AddGoals()
    {
        SubGoal subGoal;
        subGoal = new SubGoal("Flee", GenericData.Create<int>(1), false);
        goals_.Add(subGoal, 2);

        subGoal  = new SubGoal("IsRepairMode", GenericData.Create<int>(1), false);
        goals_.Add(subGoal , 3);

        
        subGoal = new SubGoal("KillGirld", GenericData.Create<int>(1), false);
        goals_.Add(subGoal, 5);

    

    }
    
}
