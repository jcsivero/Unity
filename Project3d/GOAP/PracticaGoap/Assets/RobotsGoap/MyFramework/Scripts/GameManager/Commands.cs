using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Commands : Base
{
    //private Dictionary<object, Dictionary<string,Command>> available_;
    private Dictionary<object, CompositeData> available_;
     ///listado de comandos disponibles, son los que se han creado. así se pueden instanciar solo los que se necesitan.
    public Queue<Command> commands_; // cola de comandos pendientes de  ejecutar.
       
    public Commands ()
    {
        SetName("Commands");
        Debug.Log("Iniciado instancia desde contructor " +GetName());

        if (GetGameManager().debugModeForce_ == DebugModeForce_.debug)
            debugMode_ = true;

        if (GetGameManager().debugModeForce_ == DebugModeForce_.noDebug)
            debugMode_ = false;
            
        
        commands_ = new Queue<Command>();
        //available_ = new Dictionary<object, Dictionary<string,Command>>();
        available_ = new  Dictionary<object, CompositeData>();
        
    }

    ///agrega un comando a la cola de ejecución pero solo si existe, o sea, si ha sido previamente creado.
    override public void AppendCommand(string name,object pointer = null) ///agrega solo comandos ya creados, o sea, que existan.
    {
        
        if (available_.ContainsKey(pointer))
            if (available_[pointer].GetIfExistValue(name))
                commands_.Enqueue(available_[pointer].Get<Command>(name));
                
    }

 
    override public void ExecuteCommands()
    {
        foreach (Command draft in commands_)
                draft.Exec();
        commands_.Clear();
    }

    override public void CreateCommand(string name, Command command, object pointer=null)
    {        
        string typeCommands;
        if (pointer == null)
        {
            pointer = this; 
            typeCommands = "Comandos Globales"           ;
        }
        else
            typeCommands = "Comandos Propios del Objecto";
            

        if (!available_.ContainsKey(pointer))            
            available_.Add(pointer, new CompositeData(typeCommands));
        
        available_[pointer].Set<Command>(name, command);
        
    }

    override public Command GetCommand(string name,object pointer = null)
    {        
        if (available_.ContainsKey(pointer))
            return available_[pointer].Get<Command>("name");
        return null;
    }
    
}
