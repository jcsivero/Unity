using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : NPCBaseFSM
{

    private bool isHealthRecovery_ = false;              

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);        
        
    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();        

        if (aiController_.CleverHide(npc_) && (!isHealthRecovery_))
            npc_.StartHealthRecovery();
        
                        
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc_.StopHealthRecovery();
        isHealthRecovery_ = false;

    }


}   