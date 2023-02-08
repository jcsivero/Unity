using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateWayPoints : Command
{
    public CommandUpdateWayPoints()
    {
        
    }
    public override bool Exec()
    {        
        //GetStatusWorld().wayPoints_.Clear() ///primero vacío la lista para 
        //GetStatusWorld().wayPoints_.Add = new List<GameObject>(GameObject.FindGameObjectsWithTag("WayPointsRobots"));        
        //GetStatusWorld().wayPointsNpcZombies_ = GameObject.FindGameObjectsWithTag("WayPointsZombies");        
//                GetStatusWorld();
                
        return true;
    }

}
