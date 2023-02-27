using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Commands 
{
    public Queue<Command> commands_; // cola de comandos pendientes de  ejecutar.
       
    public Commands ()
    {
        Debug.Log("+++++++++++++++++++ Creada instancia Commands");
        commands_ = new Queue<Command>();
        
    }
    public Commands (Command draft)
    {
        commands_ = new Queue<Command>();
        AppendCommand(draft);
        
    }

    public void AppendCommand(Command draft)
    {
        commands_.Enqueue(draft);
    }
    public void ExecuteCommands()
    {
        foreach (Command draft in commands_)
                draft.Exec();
        commands_.Clear();
    }
}
