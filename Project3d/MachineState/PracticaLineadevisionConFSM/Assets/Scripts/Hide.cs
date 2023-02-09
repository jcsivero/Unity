using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : NPCBaseFSM
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);
                
    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();

        bool useNavMesh = false;

        if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
            useNavMesh = true;
        else 
            useNavMesh = false;        

        aiController_.ErasePathNavMesh(npc_);         
        aiController_.Hide(npc_,useNavMesh);
                        
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

    }


}