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
         status_ =  status;
        amount_ = amount;
    }
    public override bool Exec()
    {        
        status_.SetHealth(status_.GetHealth()+ amount_);
                
        return true;
    }
    public void Set(int amount)  ///
    {        
        amount_ = amount;
    }
}
