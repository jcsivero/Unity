using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionPatrolMode : GAction
{
    override public bool IsAchievableGivenCustomize(Dictionary<string, GenericData> conditions)
    {
        return true;
    }

    override public bool IsAchievable()
    {
        return true;
    }

    public override bool PrePerform()
    {
        /*target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;*/
        status_.anim_.SetTrigger("Patrol");
        return true;
    }

    public override bool PostPerform()
    {
        /*GWorld.Instance.GetWorld().ModifyState("treatingPatient", 1);
        GWorld.Instance.AddCublicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeCubicle", 1);*/
        return true;
    }
}
