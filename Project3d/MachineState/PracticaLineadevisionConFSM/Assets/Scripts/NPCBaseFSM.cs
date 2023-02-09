using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBaseFSM : StateMachineBehaviour
{

    public StatusNpc npc_;

    public AIController aiController_;
        
    public Animator animator_; 
    public AnimatorStateInfo stateInfo_;
    
    public GameObject target_;

    void Awake()
    {
       
        Debug.Log("+++++++ Creada instancia " + this.name + " +++++++++++++++++" );
        
    }
        /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        Debug.Log("----------- Destruida instancia " + this.name + " ----------------" );
        
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
            npc_ = animator.gameObject.GetComponent<StatusNpc>();            
            aiController_ = npc_.GetAIController();            
            target_ = npc_.GetTarget();            
            animator_ = animator;
            stateInfo_ = stateInfo;   
            //UpdateState();
        
    }

     public  void UpdateState()
    {     
        //animator_.SetFloat("animationVelocity",npc_.GetCurrentSpeedAI());
        Vector3 direction =  target_.transform.position - npc_.transform.position;                
        //direction.z = 0; //solo me interesa el ángulo en x
        direction.y = 0; //solo me interesa el ángulo en x
        Vector3 npcDirection = npc_.transform.forward;
        //npcDirection.z = 0;
        npcDirection.y = 0;
        animator_.SetFloat("angle",Vector3.Angle(direction, npcDirection));        
        animator_.SetFloat("distance", Vector3.Distance(npc_.transform.position, target_.transform.position));   
        animator_.SetInteger("health",npc_.GetHealth());

        if (animator_.GetFloat("distance") < npc_.visDist_)
            animator_.SetBool("targetClose",true);
        else
            animator_.SetBool("targetClose",false);

        if (animator_.GetFloat("distance") < npc_.visDistToAttack_)
            animator_.SetBool("attackMode",true);
        else
            animator_.SetBool("attackMode",false);               

        
        if (animator_.GetFloat("angle") < npc_.visAngle_)
            animator_.SetBool("angleValid", true);
        else 
            animator_.SetBool("angleValid", false);
        
        if (((animator_.GetBool("targetClose")) || (animator_.GetBool("attackMode"))) && (animator_.GetBool("angleValid"))) ///para economizar, si no se cumplen las condiciones de distancia y ángulo, no realizo el RayCast
        ///para comprobar si es visible el objetivo.
        {
            if (aiController_.CanSeeTarget(npc_,npc_.GetTarget()))
                animator_.SetBool("visibleTarget",true);
            else
                animator_.SetBool("visibleTarget",false);
        }            
        else  
            animator_.SetBool("visibleTarget",false);
    }       
}
