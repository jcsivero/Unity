using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToToilet : GAction
{

    public override bool PrePerform()
    {
        target = GWorld.Instance.RemoveToilet();
        if (target == null)
            return false;
        inventory.AddItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeToilet", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.AddToilet(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeToilet", 1);
        beliefs.RemoveState("needRelief");
        return true;
    }
}
