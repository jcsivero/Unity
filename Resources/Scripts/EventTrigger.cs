using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public GameObject target;
    private const string EVENT_TOGGLE_SPIN = "Toggle Spin";
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("lanzo trigger");
            Vector3 newPosition = new Vector3(Random.Range(-2,2),Random.Range(0,2),0);

            EventDataReturned valor = GameManagerMyEvents.TriggerEvent<EventData>(target,EVENT_TOGGLE_SPIN,EventData.Create(EVENT_TOGGLE_SPIN)
            .Set("Position",newPosition)
            .Set("Name","Jhon")
            .Set("Age",33)
            );

            Debug.Log(valor.succes_ + " " + valor.exitCode_ + " " + (valor.objectReturned_ as Spin).speed_ );

            


        }
    }
}
