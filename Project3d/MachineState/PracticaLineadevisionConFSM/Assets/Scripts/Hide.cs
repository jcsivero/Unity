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
    private bool isHidding_ = false;
    private Vector3 posHide_;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();        

        if (isHidding_)
        {
            if (aiController_.Seek(npc_,posHide_))
            {
                if (!isHealthRecovery_)
                {
                    Debug.Log("Recuperando energia");
                    npc_.StartHealthRecovery();
                    isHealthRecovery_ = true;
                }                
            }

        }
        else
        {
            posHide_ = aiController_.CleverHide(npc_);

            ///si consiguió un punto de ocultación.
            if (posHide_ != Vector3.zero)
                isHidding_ = true;
        }
                        
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc_.StopHealthRecovery();
        isHealthRecovery_ = false;
        isHidding_ = false;

    }


}   