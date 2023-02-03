using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMono : MonoBehaviour
{
    
    virtual public GameManager GetGameManager()
    {
        return  GameManager.Instance();
    }

    virtual public ManagerMyEvents GetManagerMyEvents()
    {
        return  ManagerMyEvents.Instance();
    }


    virtual public StatusWorld GetStatusWorld()
    {
        return  GetGameManager().statusWorld_;
    }
   virtual public StatusHud GetStatusHud()
    {
        return  GetGameManager().statusHud_;
    }



}
