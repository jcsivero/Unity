using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAgentRobots : GAgent
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
        SubGoal s1 = new SubGoal("PatrolMode", GenericData.Create<int>(1), false);
        goals_.Add(s1, 1);

        SubGoal s2 = new SubGoal("HideMode", GenericData.Create<int>(1), false);
        goals_.Add(s2, 1);

        SubGoal s3 = new SubGoal("ChaseMode", GenericData.Create<int>(1), false);
        goals_.Add(s3, 1);

        SubGoal s4 = new SubGoal("IdleMode", GenericData.Create<int>(1), false);        
        goals_.Add(s4, 1);

        SubGoal s5 = new SubGoal("ChaseMode", GenericData.Create<int>(1), false);
        goals_.Add(s5, 1);

    }
    
}
