﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : NPCBaseFSM
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);        
        //npc_.ErasePathNavMesh();
        npc_.StartFiring();
    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();          
        //npc_.ErasePathNavMesh();        
        Vector3 look = target_.transform.position;
        look.y = npc_.transform.position.y;
        npc_.transform.LookAt(look);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc_.StopFiring();        

    }


}