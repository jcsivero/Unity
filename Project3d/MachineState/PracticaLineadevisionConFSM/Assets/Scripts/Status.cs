using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class  Status :  MonoBehaviour
{
    public GameObject origin_;
    public bool updateHud_;

    public Command tasks_;

     public abstract string GetName();
     public abstract int GetLifes();
     public abstract float GetHealth();
     

    public abstract void SetName(string draft);
    public abstract void SetHealth(float draft);
    public abstract void SetLifes(int draft);

    public abstract bool ExecutionTasks();
    virtual public void  SetOrigin(GameObject draft)
    {
        origin_ = draft;
    }

     virtual public  GameObject GetOrigin()
     {
        return origin_;
     }    
      virtual public  bool GetDelete(){
        return false;
     }

    virtual public void SetDelete(bool draft)
    {

    }
    virtual public  bool GetUpdateHud(){
        return updateHud_;
     }

    virtual public void SetUpdateHud(bool draft)
    {
        updateHud_ = draft;
    }

}

