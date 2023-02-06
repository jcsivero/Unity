using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddOrSubLifes : Command
{
 
     private int amount_;
    private Status status_;
    public CommandAddOrSubLifes(Status status)
    {
        status_ =  status;
    }
    public CommandAddOrSubLifes(Status status,int amount)
    {
        amount_ = amount;
    }
    public override bool Exec()
    {        
        status_.SetLifes(status_.GetLifes()+ amount_);
                
        return true;
    }
    public void Set(int amount)  ///cantidad enemigos a sumar o restar en el mundo
    {
        
        amount_ = amount;
    }
}
