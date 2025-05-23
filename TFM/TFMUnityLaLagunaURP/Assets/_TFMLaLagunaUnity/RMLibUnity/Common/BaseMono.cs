using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMono : MonoBehaviour
{
    public bool debugMode_ =false ; ///
    protected string name_;
    virtual public  void Awake()
    {    
        SetName("BaseMono");      
        Debug.Log("|||||||||||||| Awake + " + GetName().ToString() +"||||||||||||||||");

    }


    virtual public void Start()
    {
        Debug.Log("|||||||||||||| Start + " + GetName().ToString() +"||||||||||||||||");

        if (GetGameManager().debugModeForce_ == DebugModeForce.debug)
            debugMode_ = true;

        if (GetGameManager().debugModeForce_ == DebugModeForce.noDebug)
            debugMode_ = false;
        

        if (!ReadyEngine())
            Debug.Log("<color=red> Error Grave: Engine no funcional. LevelManager no cargado o función GetInitialInformation de LevelManager no ejecutada con éxito</color>");
        
            
    }        
    virtual public GoapStates GetWorldStates()
    {
        return GetWorld().GetWorldStates();
    }
    virtual public  string GetName()
    {
        return name_;
    }
    public  void SetName(string draft)
    {
        name_ = draft;
    } 
    virtual public GameManager GetGameManager()
    {
        return GameManager.Instance();;
    }

   ///Ejecutar como minimo desde el método Start, para asegurar que los gameobjects están creados.

    virtual public ManagerMyEvents GetManagerMyEvents()
    {
        return  ManagerMyEvents.Instance();;
    }

    virtual public AIController GetAIController()
    {
        return  GetGameManager().aiController_;
    }
    virtual public bool ReadyEngine()
    {
        if (!GetGameManager().readyEngine_)
            GetGameManager().ReadyEngine();
        return GetGameManager().readyEngine_;        
    }
    virtual public World GetWorld()
    {

        return  GetGameManager().world_;
    }

    virtual public void SetHudWorld(HudWorldBase world)
    {         
         GetGameManager().hudWorld_ = world;
    }
   virtual public HudWorldBase GetHudWorld()
    {
        
        return GetGameManager().hudWorld_;
            
    }

   virtual public LevelManager GetLevelManager()
   {
 
        return GetGameManager().levelManager_ ;
   }
   virtual public void  SetLevelManager(LevelManager levelManager)
   {
        GetGameManager().levelManager_ = levelManager;
   }
   virtual public HudLevelBase GetHudLevel()
    {      
            return GetLevelManager().hudLevel_;
        
    }
    virtual public void SetHudLevel(HudLevelBase level)
    {

         GetLevelManager().hudLevel_ = level;
    }
   virtual public Commands GetCommands()
    {
        return  GetGameManager().commands_;

    }
  virtual public Command GetCommand(string name,object pointer = null)
    {
        return  GetCommands().GetCommand(name, pointer);

    }

   virtual public void AppendCommand(string name,object pointer = null)
    {
        GetGameManager().commands_.AppendCommand(name,pointer);
        

    }

    virtual public void CreateCommand(string name, Command command, object pointer=null)
    {
        GetCommands().CreateCommand(name, command,  pointer);
        
        
    }
    virtual public void ExecuteCommands()
    {
        GetGameManager().commands_.ExecuteCommands();
        
    }

}
