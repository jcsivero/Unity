using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot
{    

    public AIController ai_;
    // Start is called before the first frame update
    public Bot(AIController ai)
    {
        ai_ = ai;        
    }


    public void Seek(Vector3 location)
    {
        ai_.agent_.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - ai_.transform.position;
        ai_.agent_.SetDestination(ai_.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = ai_.target_.transform.position - ai_.transform.position;

        float lookAhead = targetDir.magnitude * ai_.GetCurrentSpeedTarget() / ai_.GetCurrentSpeedAI();
        Debug.DrawRay(ai_.target_.transform.position, ai_.target_.transform.forward * lookAhead,Color.red);
        Seek(ai_.target_.transform.position + ai_.target_.transform.forward * lookAhead);
    }

    Vector3 wanderTarget = Vector3.zero;
    public void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = ai_.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    public void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - ai_.target_.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(ai_.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(ai_.transform.position, hidePos);
            }
        }

        Seek(chosenSpot);

    }
    public void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - ai_.target_.transform.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 100;
            

            if (Vector3.Distance(ai_.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(ai_.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);
        Debug.DrawRay(chosenSpot, -chosenDir.normalized * distance, Color.red);

        Seek(info.point + chosenDir.normalized);
        //Seek(info.point);
    }

    public bool CanSeeTarget(GameObject target)
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = ai_.target_.transform.position - ai_.transform.position;
        if (Physics.Raycast(ai_.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
                //if (raycastInfo.transform.gameObject == target)
                    return true;
        }
        return false;
    }



}
