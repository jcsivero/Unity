using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : GAction
{

    public override bool PrePerform()
    {
        target = GWorld.Instance.RemoveOffice();
        if (target == null)
            return false;
        inventory.AddItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeOffice", -1);
        return true;

    }

    public override bool PostPerform()
    {
        GWorld.Instance.AddOffice(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeOffice", 1);
        return true;
    }
}
