using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : NPCBaseFSM
{


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);
        npc_.SetSpeedMax(npc_.GetSpeedInitial()*2);        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateState();                                
        
        if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
        {
            npc_.GetAgentNavMesh().speed = npc_.speedMax_ * 2; 
            
            //aiController_.Seek(npc_,target_.transform.position);                       
            aiController_.Pursue(npc_);
        }
            
        else
        {
            npc_.GetAgentNavMesh().ResetPath();            
            aiController_.Pursue(npc_,false);
            /*var direction = target_.transform.position - npc_.transform.position;
            npc_.transform.rotation = Quaternion.Slerp(npc_.transform.rotation, Quaternion.LookRotation(direction), npc_.rotationSpeed_ * Time.deltaTime);             
            npc_.transform.Translate(0, 0, npc_.GetCurrentSpeedAI()*2); ///puesto que voy corriendo*/
    
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
          //  npc_.bot_.Pursue();
        
    }


}