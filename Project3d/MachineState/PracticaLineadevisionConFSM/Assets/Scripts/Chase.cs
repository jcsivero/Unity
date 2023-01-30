using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : NPCBaseFSM
{

   void Awake()
    {
        Debug.Log("creada instancia chase");
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //UpdateState();        
        npcComponentAIController_.UpdateCurrentsSpeeds();
        
        if ((npcComponentAIController_.useNavMeshAI_) && (npcComponentAIController_.agent_ != null))
        {
            npcComponentAIController_.agent_.speed = npcComponentAIController_.speed_ * 2; 
            npcComponentAIController_.agent_.SetDestination(target_.transform.position);
            //npcComponentAIController_.bot_.Pursue();
        }
            
        else
        {
            npcComponentAIController_.agent_.ResetPath();
            var direction = target_.transform.position - npc_.transform.position;
            npc_.transform.rotation = Quaternion.Slerp(npc_.transform.rotation, Quaternion.LookRotation(direction), npcComponentAIController_.rotationSpeed_ * Time.deltaTime);             
            npc_.transform.Translate(0, 0, npcComponentAIController_.GetCurrentSpeedAI()*2); ///puesto que voy corriendo
    
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if ((npcComponentAIController_.useNavMeshAI_) && (npcComponentAIController_.agent_ != null))
          //  npcComponentAIController_.bot_.Pursue();
        
    }


}