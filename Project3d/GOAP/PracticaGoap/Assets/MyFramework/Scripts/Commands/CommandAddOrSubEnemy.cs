using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddOrSubEnemy : Command
{
    private int amount_;

    public CommandAddOrSubEnemy()
    {
        
    }
    public CommandAddOrSubEnemy(int amount)
    {
        amount_ = amount;
    }
    public override bool Exec()
    {        
        GetStatusWorld().SetOrAddCountEnemies(GetStatusWorld().GetCountEnemies()+ amount_);
        
        return true;
    }
    public void Set(int amount)  ///cantidad enemigos a sumar o restar en el mundo
    {
        
        amount_ = amount;
    }
}
