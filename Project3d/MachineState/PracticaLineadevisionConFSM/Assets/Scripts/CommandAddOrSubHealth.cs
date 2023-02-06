using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddOrSubHealth : Command
{
    private int amount_;
    private Status status_;
    public CommandAddOrSubHealth(Status status)
    {
        status_ =  status;
    }
    public CommandAddOrSubHealth(Status status,int amount)
    {
        amount_ = amount;
    }
    public override bool Exec()
    {        
        status_.SetHealth(status_.GetHealth()+ amount_);
                
        return true;
    }
    public void Set(int amount)  ///cantidad enemigos a sumar o restar en el mundo
    {
        
        amount_ = amount;
    }
}
