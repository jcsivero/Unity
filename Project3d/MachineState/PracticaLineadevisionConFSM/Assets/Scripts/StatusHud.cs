using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHud : Status
{
    public string name_ = "StatusHud";

    public override string GetName()
    {
        return name_;
    }
    /*public override float GetHealth()
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
*/


}
