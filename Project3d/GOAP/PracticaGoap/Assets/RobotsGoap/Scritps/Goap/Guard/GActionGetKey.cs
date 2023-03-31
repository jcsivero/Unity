using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionGetKey : GAction
{
    Vector3 goTo_ = Vector3.zero;
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

            return true;        
    }

    override public bool IsAchievable() ///puedo filtrar la acción y evitar que sea computada por el planificador teniendo en cuenta cualquier consideración
    {
        return true;
    }
    
    public override bool PrePerform()
    {
        target = GetLevelManager().gameObjectsByName_[targetTagOrName][0];        ///ahora el objetivo es la posición de la llave
        goTo_= status_.GetAIController().CalculatePointTarget(status_.GetOrigin(),target,false,status_.GetNavMeshRadius());
            return true;

    }

    public override void PostPerform(Reason reason) 
    {

        /*GWorld.Instance.GetWorld().ModifyState("treatingPatient", 1);
        GWorld.Instance.AddCublicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeCubicle", 1);*/
        if (reason == Reason.success) ///si terminó correctamente y no fue por cambiode condicoines o evento de interrupción, indico que tengo la llave.        
        {
             npcGoapStates_.SetOrAddState("IHaveKey",GenericData.Create<bool>(true)); ///creo un estado del npc indicando que ya tengo el arma             
             GetHudWorld().SetValue<Color>("HudKeyColor",Color.white);             
             Destroy(target); ///destruyo objeto.
             
        }   
       
    }
    override public  bool OnPerform()
    {
        return status_.GetAIController().Seek(status_,goTo_);
    }
}
