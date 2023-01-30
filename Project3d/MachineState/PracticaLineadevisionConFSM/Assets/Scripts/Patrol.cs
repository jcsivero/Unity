using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : NPCBaseFSM
{

    void Awake()
    {
       
        Debug.Log("creada instancia PATROL" + this.name + " " );
        
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);             
        npcComponentAIController_.currentWP_ = 0;
        Debug.Log("desde patrol " + animator.name + " " + animator.gameObject.name);
        //UpdateState();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //UpdateState();
        
      


/*



        npcComponentAIController_.UpdateCurrentsSpeeds();

        if (npcComponentAIController_.waypoints_ == null) 
            npcComponentAIController_.bot_.Wander();

        else
        {
            if (npcComponentAIController_.waypoints_.Length == 0)             
            {
                    npcComponentAIController_.bot_.Wander();
                    //Debug.Log("Modo Wander");
            }   
            
            
            else 
            {
                if (Vector3.Distance(npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position, npc_.transform.position) < npcComponentAIController_.accuracy_)
                {
                    npcComponentAIController_.currentWP_++;
                    if (npcComponentAIController_.currentWP_ >= npcComponentAIController_.waypoints_.Length)
                        npcComponentAIController_.currentWP_ = 0;

                }
                if ((npcComponentAIController_.useNavMeshAI_) && (npcComponentAIController_.agent_ != null))
                {
                    if  (!npcComponentAIController_.agent_.hasPath)
                    {
                        
                        Debug.Log("asignando nuevo path");                        
                        npcComponentAIController_.agent_.SetDestination(npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position);
                        //npcComponentAIController_.bot_.Seek(npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position);
                    }
                        
                    
                        Debug.Log("Movimiento navmesh");
                }
                    
                else
                {
                    npcComponentAIController_.agent_.ResetPath();
                    var direction = npcComponentAIController_.waypoints_[npcComponentAIController_.currentWP_].transform.position - npc_.transform.position;
                    npc_.transform.rotation = Quaternion.Slerp(npc_.transform.rotation, Quaternion.LookRotation(direction), npcComponentAIController_.rotationSpeed_ * Time.deltaTime);                    
                    npc_.transform.Translate(0, 0, npcComponentAIController_.GetCurrentSpeedAI());

                    Debug.Log("Movimiento manual");
            
                }


            }
        }*/
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}