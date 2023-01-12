using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCubicle : GAction
{

    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;

        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("treatingPatient", 1);
        GWorld.Instance.AddCublicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeCubicle", 1);
        return true;
    }
}
