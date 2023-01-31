using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAddEnemy : Tasks
{
    public override bool Exec(StatusWorld obj, EventData data = null)
    {        
        obj.countEnemies_  += 1; 
        return true;
    }
}
