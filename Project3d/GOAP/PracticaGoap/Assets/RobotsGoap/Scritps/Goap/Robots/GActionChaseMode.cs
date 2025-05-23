using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionChaseMode : GAction
{
      private float speedPrevious_;
  override public bool IsAchievableGiven(GoapStates conditions,bool planMode = false)
    {
        
        if (planMode)
            foreach (KeyValuePair<string, GenericData> p in preconditions_.GetStates())
            {
                if (!conditions.HasState(p.Key))
                    return false;
            }
        ///si están todas las precondiciones en el estado del mundo y los estados del nps(del NPC) entonces paso a la función virtual en la que se puede
        ///comprobar los valores de las precondiciones según se necesite.
        ///Por defecto, solo con que aparezcan las precondiciones es suficiente para dar la acción por válida para agreagar a la cola, pero esto solo
        //es cierto mientras se crea un plan, después esas precondiciones realmente pueden no pertenecer ni al estado del mundo ni del npc.
        
        return IsAchievableGivenCustomize(conditions);
    }
    override protected bool IsAchievableGivenCustomize(GoapStates conditions)
    {
        if (npcGoapStates_.GetState("distance").GetValue<float>()< status_.GetVisDistance())
            if (npcGoapStates_.GetState("distance").GetValue<float>()> status_.GetVisDistanceToAttack())
                if (npcGoapStates_.GetState("angle").GetValue<float>() < status_.GetVisAngle())
                    if (npcGoapStates_.GetState("visibleTarget").GetValue<bool>())  
                        return true;
            
                
        return false;
    }

    override public bool IsAchievable() ///puedo filtrar la acción y evitar que sea computada por el planificador teniendo en cuenta cualquier consideración
    {
        return true;
    }

    public override bool PrePerform()
    {
        
        
        /*target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;*/
        status_.anim_.SetBool("Chase",true);
        speedPrevious_ = status_.GetSpeedMax();
        status_.SetSpeedMax(speedPrevious_*2);             
        return true;
    }

    public override void PostPerform(Reason reason) 
    {
        status_.SetSpeedMax(speedPrevious_);     
        status_.anim_.SetBool("Chase",false);
        //status_.anim_.SetTrigger("Idle");
        
    }
    override public  bool OnPerform()
    {
        status_.GetAIController().Pursue(status_);
        return false;
    }
}
