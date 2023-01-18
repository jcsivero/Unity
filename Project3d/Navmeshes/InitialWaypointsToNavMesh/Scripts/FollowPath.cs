using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    //Transform goal;
    //float speed = 5.0f;
    //float accuracy = 1.0f;
    //float rotSpeed = 2.0f;
    public GameObject wpManager;
    GameObject[] wps;
    UnityEngine.AI.NavMeshAgent agent;
    //GameObject currentNode;
    //int currentWP = 0;
    Graph g;

    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        //g = wpManager.GetComponent<WPManager>().graph;
        //currentNode = wps[1];
    }
    public void GoToHeli() {
        //g.AStar(currentNode,wps[0]);
        //currentWP = 0;
        agent.SetDestination(wps[0].transform.position);
    }
    public void GoToRuin() {
        agent.SetDestination(wps[7].transform.position);
        //g.AStar(currentNode,wps[7]);
        //currentWP = 0;
    }
    public void GoToTank() {
        agent.SetDestination(wps[5].transform.position);
        //g.AStar(currentNode,wps[5]);
        //currentWP = 0;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        /* if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;
        //el nodo más cercano en este momento
        currentNode = g.getPathPoint(currentWP);

        //si estamos suficientemente cerca del WP actual, nos movemos al siguiente
        if (Vector2.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy) {
            currentWP++;
        }
        if (currentWP < g.getPathLength()){
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, 
										this.transform.position.y, 
										goal.position.z);
		    Vector3 direction = lookAtGoal - this.transform.position;
		    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
												Quaternion.LookRotation(direction), 
												Time.deltaTime*rotSpeed);
		    this.transform.Translate(0,0,speed*Time.deltaTime);
        }
        */
    }
}
