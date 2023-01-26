using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBaseFSM : StateMachineBehaviour
{

    public GameObject npc_;
    public AIController npcComponentAIController_;
    
    public GameObject target_;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc_ == null)
            npc_ = animator.gameObject;


        if (npcComponentAIController_ == null)
            npcComponentAIController_ = npc_.GetComponent<AIController>();

        if (target_ == null)  
            target_ = npcComponentAIController_.GetTarget();
        
        
    }

}
