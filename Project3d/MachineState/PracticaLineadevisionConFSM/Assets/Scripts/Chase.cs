using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : NPCBaseFSM
{

    private float speedPrevious_;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);
        speedPrevious_ = npc_.GetSpeedMax();
        npc_.SetSpeedMax(speedPrevious_*2);        


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();  
        npc_.ErasePathNavMesh();               
        aiController_.Pursue(npc_);
                   
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc_.SetSpeedMax(speedPrevious_);     


        
    }


}