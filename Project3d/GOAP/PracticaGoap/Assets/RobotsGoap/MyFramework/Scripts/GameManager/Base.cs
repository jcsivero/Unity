using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base
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

   virtual public LevelManager GetLevelManager()
   {
        return GetGameManager().levelManager_;
   }
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
    virtual public World GetStatusWorld()
    {
        return  GetGameManager().world_;
    }
   virtual public Hud GetHudWorld()
    {
        return  GetGameManager().hudWorld_;
    }


   virtual public Hud GetHudLevel()
    {
        return GetLevelManager().hudLevel_;
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
