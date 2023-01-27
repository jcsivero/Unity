using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : NPCBaseFSM
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        npcComponentAIController_.StartFiring();
    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npcComponentAIController_.UpdateCurrentsSpeeds();
        if ((npcComponentAIController_.useNavMeshAI_) && (npcComponentAIController_.agent_ != null))
        {
            npcComponentAIController_.agent_.ResetPath();
            Debug.Log("desactivando navmesh");
        }
            
        
        npc_.transform.LookAt(target_.transform.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npcComponentAIController_.StopFiring();
    }


}