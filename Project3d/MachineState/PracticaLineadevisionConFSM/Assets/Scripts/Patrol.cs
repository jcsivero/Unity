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

        if (npc_.GetStatusWorld().wayPointsNpcRobots_.Length == 0)             
        {
                if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
                    aiController_.Wander(npc_);
                else 
                    aiController_.Wander(npc_,false);
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
                if  (!npc_.GetAgentNavMesh().hasPath) ///solo asigno nueva ruta en caso de que no tenga. Esto lo hago solo con los waypoints, puesto que son fijos.
                ///es para ahorrar recursos, ya que con NavMesh, el mismo complemento se encarga de llevar al NPC hasta el destino.                
                {
                    
                    Debug.Log("asignando nuevo path");                        
                    //npc_.GetAgentNavMesh().SetDestination(npc_.GetStatusWorld().wayPointsNpcRobots_[npc_.currentWP_].transform.position);
                    aiController_.Seek(npc_,npc_.GetStatusWorld().wayPointsNpcRobots_[npc_.currentWP_].transform.position);
                    
                }                                    
                    
            }
                
            else
            {
                if  (npc_.GetAgentNavMesh() != null) ///solo asigno nueva ruta en caso de que no tenga. Esto lo hago solo con los waypoints, puesto que son fijos.
                    ///es para ahorrar recursos, ya que con NavMesh, el mismo complemento se encarga de llevar al NPC hasta el destino.                
                    if (npc_.GetAgentNavMesh().hasPath)
                        npc_.GetAgentNavMesh().ResetPath();     

                aiController_.Seek(npc_, npc_.GetStatusWorld().wayPointsNpcRobots_[npc_.currentWP_].transform.position,false);

            }


        }

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}