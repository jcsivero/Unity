using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : GAction
{
    public GameObject puddlePrefab;
    
    public override bool PrePerform()
    {
        if (GWorld.Instance.GetWorld().HasState("freeToilet"))
            return false;
        target = this.gameObject;
        if (target == null)
            return false;
        
        return true;
    }

    public override bool PostPerform()
    {
        Vector3 location = new Vector3(this.transform.position.x, puddlePrefab.transform.position.y, this.transform.position.z);
        GameObject p = Instantiate(puddlePrefab, location, puddlePrefab.transform.rotation);
        GWorld.Instance.AddPuddle(p);
        GWorld.Instance.GetWorld().ModifyState("uncleanPuddle", 1);
        beliefs.RemoveState("needRelief");
        beliefs.RemoveState("bursting");
        return true;
    }
}
