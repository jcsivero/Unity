using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : GAgent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
       
        SubGoal s1 = new SubGoal("rested", 1, false);
        goals.Add(s1, 2);
        Invoke("GetTired", Random.Range(10, 20));

        SubGoal s2 = new SubGoal("research", 1, false);
        goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("relief", 1, false);
        goals.Add(s3, 3);
        Invoke("NeedRelief", Random.Range(10, 20));


    }

    void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke("GetTired", Random.Range(10, 20));
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
