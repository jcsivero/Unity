using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBaseFSM : StateMachineBehaviour
{

    public GameObject npc_;
    public AIController npcComponentAIController_;
        
    public Animator animator_; 
    public AnimatorStateInfo stateInfo_;
    
    public GameObject target_;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        Debug.Log("creada instancia NFCBASEFSM");
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        Debug.Log("destruido objeto");
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
            npc_ = animator.gameObject;
            npcComponentAIController_ = npc_.GetComponent<AIController>();
            target_ = npcComponentAIController_.GetTarget();            
            animator_ = animator;
            stateInfo_ = stateInfo;   
            Debug.Log("desde onstateenter:"+target_.name);
            Debug.Log("desde onstateenter:"+npc_.name);

            //UpdateState();
        
    }

     public  void UpdateState()
    {     
        Vector3 direction =  target_.transform.position - npc_.transform.position;                
        //direction.z = 0; //solo me interesa el ángulo en x
        direction.y = 0; //solo me interesa el ángulo en x
        Vector3 npcDirection = npc_.transform.forward;
        //npcDirection.z = 0;
        npcDirection.y = 0;
        animator_.SetFloat("angle",Vector3.Angle(direction, npcDirection));        
        animator_.SetFloat("distance", Vector3.Distance(npc_.transform.position, target_.transform.position));   

        if (animator_.GetFloat("distance") < npcComponentAIController_.visDist_)
            animator_.SetBool("targetClose",true);
        else
            animator_.SetBool("targetClose",false);

        if (animator_.GetFloat("distance") < npcComponentAIController_.visDistToAttack_)
            animator_.SetBool("attackMode",true);
        else
            animator_.SetBool("attackMode",false);               

        
        if (animator_.GetFloat("angle") < npcComponentAIController_.visAngle_)
            animator_.SetBool("angleValid", true);
        else 
            animator_.SetBool("angleValid", false);
        
        if (((animator_.GetBool("targetClose")) || (animator_.GetBool("attackMode"))) && (animator_.GetBool("angleValid"))) ///para economizar, si no se cumplen las condiciones de distancia y ángulo, no realizo el RayCast
        ///para comprobar si es visible el objetivo.
        {
            if (npcComponentAIController_.bot_.CanSeeTarget(npcComponentAIController_.target_))
                animator_.SetBool("visibleTarget",true);
            else
                animator_.SetBool("visibleTarget",false);
        }            
        else  
            animator_.SetBool("visibleTarget",false);
    }       
}
