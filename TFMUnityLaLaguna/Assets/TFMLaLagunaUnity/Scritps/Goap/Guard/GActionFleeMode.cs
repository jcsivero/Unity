using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionFleeMode : GAction
{
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
                
                        return true;
       return false;
    }

    override public bool IsAchievable() ///puedo filtrar la acción y evitar que sea computada por el planificador teniendo en cuenta cualquier consideración
    {
        return true;
    }
private float speedPrevious_;
    public override bool PrePerform()
    {
        status_.anim_.SetBool("Run",true);
        speedPrevious_ = status_.GetSpeedMax();
        status_.SetSpeedMax(speedPrevious_*2);   
        GetWorldStates().SetOrAddState("protectme",GenericData.Create<bool>(true)); ///indico que deben de protegerme el resto de NPCS
        status_.NavMeshErasePath(); //borro cualquier posible path para que funcione correctamente el metodo flee;
        return true;
    }

    public override void PostPerform(Reason reason) 
    {

        /*GWorld.Instance.GetWorld().ModifyState("treatingPatient", 1);
        GWorld.Instance.AddCublicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeCubicle", 1);*/
        status_.anim_.SetBool("Run",false);
        status_.SetSpeedMax(speedPrevious_);  
        GetWorldStates().RemoveState("protectme");///indico que ya no deben de protegerme.
        
       
    }
    override public  bool OnPerform()
    {
        status_.GetAIController().Flee(status_,status_.GetTarget().transform.position);
        return false; ///mientras se cumplan las condiciones siempre estaré huyendo.
    }
}
