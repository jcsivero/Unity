using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBaseFSM : StateMachineBehaviour
{

    public GameObject npc_;
    public AIController npcComponentAIController_;
    
    public GameObject opponent_;
    
    public float speed_ = 2.0f;
    public float rotSpeed_ = 1.0f;
    public float accuracy_ = 3.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc_ != null)
            npc_ = animator.gameObject;


        if (npcComponentAIController_ != null)
            npcComponentAIController_ = npc_.GetComponent<AIController>();

        if (opponent_ != null)  
            opponent_ = npcComponentAIController_.GetTarget();
        
        
    }

}
