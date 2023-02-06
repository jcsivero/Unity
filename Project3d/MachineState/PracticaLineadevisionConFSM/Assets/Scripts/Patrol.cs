using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : NPCBaseFSM
{



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);             
        npc_.currentWP_ = 0;        
        UpdateState();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();
        npc_.UpdateCurrentsSpeeds();

        if (npc_.GetStatusWorld().wayPointsNpcRobots_.Length == 0) 
            aiController_.Wander((npc_));

        else
        {
            if (npc_.GetStatusWorld().wayPointsNpcRobots_.Length == 0)             
            {
                    aiController_.Wander(npc_);
                    //Debug.Log("Modo Wander");
            }   
            
            
            else 
            {
                if (Vector3.Distance(npc_.GetStatusWorld().wayPointsNpcRobots_[npc_.currentWP_].transform.position, npc_.transform.position) < npc_.accuracyToWayPoints_)
                {
                    npc_.currentWP_++;
                    if (npc_.currentWP_ >= npc_.GetStatusWorld().wayPointsNpcRobots_.Length)
                        npc_.currentWP_ = 0;

                }
                if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
                {
                    if  (!npc_.GetAgentNavMesh().hasPath)
                    {
                        
                        Debug.Log("asignando nuevo path");                        
                        npc_.GetAgentNavMesh().SetDestination(npc_.GetStatusWorld().wayPointsNpcRobots_[npc_.currentWP_].transform.position);
                        //npc_.bot_.Seek(npc_.waypoints_[npc_.currentWP_].transform.position);
                    }
                        
                    
                        Debug.Log("Movimiento navmesh");
                }
                    
                else
                {
                    npc_.GetAgentNavMesh().ResetPath();
                    var direction = npc_.GetStatusWorld().wayPointsNpcRobots_[npc_.currentWP_].transform.position - npc_.transform.position;
                    npc_.transform.rotation = Quaternion.Slerp(npc_.transform.rotation, Quaternion.LookRotation(direction), npc_.rotationSpeed_ * Time.deltaTime);                    
                    npc_.transform.Translate(0, 0, npc_.GetCurrentSpeedAI());

                    Debug.Log("Movimiento manual");
            
                }


            }
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}