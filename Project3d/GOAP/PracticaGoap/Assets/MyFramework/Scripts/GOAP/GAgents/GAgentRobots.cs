using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAgentRobots : GAgent
{
  // Start is called before the first frame update
    new void Start()
    {
        base.Start();
       
     /*   SubGoal s1 = new SubGoal("rested", 1, false);
        goals.Add(s1, 2);
        Invoke("GetTired", Random.Range(10, 20));

        SubGoal s2 = new SubGoal("research", 1, false);
        goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("relief", 1, false);
        goals.Add(s3, 3);
        Invoke("NeedRelief", Random.Range(10, 20));*/

    ////creo los beliefs iniciales.

        //beliefs.SetOrAddState("angle")
    }
    
    override public void AddActions()
    {
        ///agrego acction de patrullaje
        //var gActionPatrolMode = gameObject.AddComponent<GActionPatrolMode>(); ///agregando así el componente, las precondiciones y efectos están vacios,así que los especificamos        
        ///gActionPatrolMode.AddPreConditions("hola",GenericData.Create<int>(2));
        //gActionPatrolMode.AddEffects("adios",GenericData.Create<int>(8));

    }
    
    override public void AddGoals()
    {
        SubGoal s1 = new SubGoal("patrolMode", GenericData.Create<int>(1), false);
        goals_.Add(s1, 1);
    }
    
}
