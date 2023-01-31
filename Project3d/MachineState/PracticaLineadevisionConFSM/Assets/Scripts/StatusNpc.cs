using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNpc : Status
{
    public string name_ = "StatusNpc";
    public int lifes_;
    public float health_;
    public bool delete_;

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

     public override bool GetDelete()
    {
        return delete_;
        
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

        public override void SetDelete(bool draft)
    {
        delete_ = draft;
        
    }
}
