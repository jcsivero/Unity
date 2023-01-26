using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : NPCBaseFSM
{


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        npcComponentAIController_.currentWP_ = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npcComponentAIController_.waypoints_ == null) 
            npcComponentAIController_.bot_.Wander();

        else
        {
            if (npcComponentAIController_.waypoints_.Length == 0)             
                npcComponentAIController_.bot_.Wander();
            
            else 
            {
                if (Vector3.Distance(npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position, npc_.transform.position) < npcComponentAIController_.accuracy_)
                {
                    npcComponentAIController_.currentWP_++;
                    if (npcComponentAIController_.currentWP_ >= npcComponentAIController_.waypoints_.Length)
                        npcComponentAIController_.currentWP_ = 0;

                }
                if (npcComponentAIController_.useNavMeshAI_)
                    npcComponentAIController_.agent_.SetDestination(npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position);
                else
                {
                    var direction = npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position - npc_.transform.position;
                    npc_.transform.rotation = Quaternion.Slerp(npc_.transform.rotation, Quaternion.LookRotation(direction), npcComponentAIController_.rotationSpeed_ * Time.deltaTime);
                    npcComponentAIController_.currentSpeedAI_ =  Time.deltaTime * npcComponentAIController_.speed_;
                    npc_.transform.Translate(0, 0, npcComponentAIController_.currentSpeedAI_);
            
                }


            }
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}