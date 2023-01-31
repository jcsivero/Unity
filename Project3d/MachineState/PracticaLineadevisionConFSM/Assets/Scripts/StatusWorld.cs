using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusWorld : Status
{
    public string name_ = "StatusWorld";
    public int lifes_;
    public float health_=810;
    
    public int  numberOfLevels_;
    public int countEnemies_;    
    public int activeLevel_; //por defecto comienza en la escena 1. La 0 es el menú principal
    public int totalPoints_ = 0;
    public int  levelPoints_ = 0;

    public override bool ExecutionTasks()
    {
        tasks_.Exec(this);
        return true;
    }
    public override string GetName()
    {
        return name_;
    }
    public override float GetHealth()
    {
        return health_;
    }    
    public override int GetLifes()
    {
        return lifes_;
    }    


    
    public override void SetName(string draft)
    {
        name_ = draft;
    }
    public override void SetHealth(float draft)
    {
        health_ = draft;
    }    
    public override void SetLifes(int draft)
    {
        lifes_ = draft;
    }    


}
