using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanPuddle : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePuddle();
        if (target == null)
            return false;
        inventory.AddItem(target);
        GWorld.Instance.GetWorld().ModifyState("uncleanPuddle", -1);
        return true;
    }

    public override bool PostPerform()
    {
        inventory.RemoveItem(target);
        Destroy(target);

        return true;
    }
}
