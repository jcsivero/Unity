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
        
        if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
        {
            npc_.GetAgentNavMesh().speed = npc_.GetSpeedMax(); 
            
            //aiController_.Seek(npc_,target_.transform.position);                       
            aiController_.Pursue(npc_);
        }
            
        else
        {
            if  (npc_.GetAgentNavMesh() != null) ///solo asigno nueva ruta en caso de que no tenga. Esto lo hago solo con los waypoints, puesto que son fijos.
            ///es para ahorrar recursos, ya que con NavMesh, el mismo complemento se encarga de llevar al NPC hasta el destino.                
                if (npc_.GetAgentNavMesh().hasPath)
                    npc_.GetAgentNavMesh().ResetPath();     
            
            aiController_.Pursue(npc_,false);

    
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc_.SetSpeedMax(speedPrevious_);        
        //if ((npc_.useNavMeshAI_) && (npc_.GetAgentNavMesh() != null))
          //  npc_.bot_.Pursue();
        
    }


}