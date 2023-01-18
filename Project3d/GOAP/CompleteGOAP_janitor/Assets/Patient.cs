using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 4);

        SubGoal s2 = new SubGoal("isTreated", 1, true);
        goals.Add(s2, 5);

        SubGoal s3 = new SubGoal("isHome", 1, true);
        goals.Add(s3, 1);

        SubGoal s4 = new SubGoal("relief", 1, false);
        goals.Add(s4, 2);
        Invoke("NeedRelief", Random.Range(10, 20));


    }

    void NeedRelief()
    {
        beliefs.ModifyState("needRelief", 0);
        Invoke("Bursting", 10);
        Invoke("NeedRelief", Random.Range(20, 30));
    }
    void Bursting()
    {
        if (beliefs.HasState("needRelief"))
            beliefs.ModifyState("bursting", 0);
    }

}
