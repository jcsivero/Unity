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
        GetStatusWorld().wayPointsNpcRobots_ = GameObject.FindGameObjectsWithTag("WayPointRobots");        
        GetStatusWorld().wayPointsNpcZombies_ = GameObject.FindGameObjectsWithTag("WayPointZombies");        
                
        return true;
    }

}
