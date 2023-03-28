using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMono : MonoBehaviour
{
    private GameManager instanceGameManager_;
    private ManagerMyEvents instanceManagerMyEvents_;
    
    private AIController instanceAIController_;
    
    virtual public GameManager GetGameManager()
    {
        if (instanceGameManager_ == null)
            instanceGameManager_ = GameManager.Instance();
        return instanceGameManager_;
    }

   ///Ejecutar como minimo desde el método Start, para asegurar que los gameobjects están creados.

    virtual public ManagerMyEvents GetManagerMyEvents()
    {
        if (instanceManagerMyEvents_ == null)
          instanceManagerMyEvents_ =  ManagerMyEvents.Instance();
        return instanceManagerMyEvents_;
    }

    virtual public AIController GetAIController()
    {
        if (instanceAIController_ == null)
          instanceAIController_ =  GetGameManager().aiController_;
        return instanceAIController_;
    }
    virtual public World GetWorld()
    {
        return  GetGameManager().world_;
    }

    virtual public void SetHudWorld(HudWorld world)
    {         
         GetGameManager().hudWorld_ = world;
    }
   virtual public HudWorld GetHudWorld()
    {
        return  GetGameManager().hudWorld_;
    }

   virtual public LevelManager GetLevelManager()
   {
        if (GetGameManager().levelManager_ == null)
            GetGameManager().levelManager_ = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        return GetGameManager().levelManager_ ;
   }
   virtual public void  SetLevelManager(LevelManager levelManager)
   {
        GetGameManager().levelManager_ = levelManager;
   }
   virtual public HudLevel GetHudLevel()
    {
        if (GetLevelManager().hudLevel_ != null)  
            return GetLevelManager().hudLevel_;
        
        return null; ///indico que todavía no se ha ejecutado el método start del gameobject que contien el script levelmanager.cs, el cual se
        ///asignaría como level manager en el gamemanager.
    }
    virtual public void SetHudLevel(HudLevel level)
    {
         if (GetHudLevel() == null) ///si aún no hay hud de nivel en el gamemanager, lo localizo.


         GetLevelManager().hudLevel_ = level;
    }
   virtual public Commands GetCommands()
    {
        return  GetGameManager().commands_;

    }

   virtual public void AppendCommand(Command draft)
    {
        GetGameManager().commands_.AppendCommand(draft);
        
    }
    virtual public void ExecuteCommands()
    {
        GetGameManager().commands_.ExecuteCommands();
        
    }

}
