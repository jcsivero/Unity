using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Commands 
{
    private Dictionary<object, Dictionary<string,Command>> available_;
     ///listado de comandos disponibles, son los que se han creado. así se pueden instanciar solo los que se necesitan.
    public Queue<Command> commands_; // cola de comandos pendientes de  ejecutar.
       
    public Commands ()
    {
        Debug.Log("+++++++++++++++++++ Creada instancia Commands");
        commands_ = new Queue<Command>();
        available_ = new Dictionary<object, Dictionary<string,Command>>();
        
    }
    public void AppendCommand(string name,object pointer = null) ///agrega solo comandos ya creados, o sea, que existan.
    {
        
        if (available_.ContainsKey(pointer))
            if (available_[pointer].ContainsKey(name))        
                commands_.Enqueue(available_[pointer][name]);
                
    }

 
    public void ExecuteCommands()
    {
        foreach (Command draft in commands_)
                draft.Exec();
        commands_.Clear();
    }

    public void CreateCommand(string name, Command command, object pointer=null)
    {
        if (pointer == null)
            pointer = this;            

        if (!available_.ContainsKey(pointer))
            available_.Add(pointer, new Dictionary<string, Command>());
        
        Dictionary<string, Command> pointerCommands = available_[pointer];


        if (!pointerCommands.ContainsKey(name))
            pointerCommands.Add(name,command);
    }

    public Command GetCommand(string name,object pointer = null)
    {
        if (available_.ContainsKey(pointer))
            if (available_[pointer].ContainsKey(name))        
                return available_[pointer][name];
        return null;
    }
    
}
