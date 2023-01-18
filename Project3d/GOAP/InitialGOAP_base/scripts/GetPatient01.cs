using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GAction
{
    GameObject resource;
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePatient();
        if (target == null)
            return false;
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("patientWaiting", -1);
        
        return true;
    }
}
