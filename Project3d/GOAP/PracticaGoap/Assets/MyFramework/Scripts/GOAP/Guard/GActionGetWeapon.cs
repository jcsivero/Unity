using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionGetWeapon : GAction
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
        if (npcGoapStates_.GetState("health").GetValue<int>()< 40) ///vida crítica
            return true;
        return false;
    }

    override public bool IsAchievable() ///puedo filtrar la acción y evitar que sea computada por el planificador teniendo en cuenta cualquier consideración
    {
        return true;
    }

    public override bool PrePerform()
    {
        
        if (status_.GetAIController().CleverHide(status_,false) != Vector3.zero)
        {
            /*target = inventory.FindItemWithTag("Cubicle");
            if (target == null)
                return false;*/
            status_.anim_.SetBool("Hide",true);
            Debug.Log("pasando a modo hide");
            return true;

        }
        return false;
    }

    public override void PostPerform(Reason reason) 
    {

        /*GWorld.Instance.GetWorld().ModifyState("treatingPatient", 1);
        GWorld.Instance.AddCublicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeCubicle", 1);*/
        status_.anim_.SetBool("Hide",false);
       
    }
    override public  bool OnPerform()
    {
        
        return status_.GetAIController().GoToCleverHide(status_);
    }
}
