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
    virtual public StatusWorld GetStatusWorld()
    {
        return  GetGameManager().statusWorld_;
    }
   virtual public StatusHud GetStatusHud()
    {
        return  GetGameManager().statusHud_;
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
