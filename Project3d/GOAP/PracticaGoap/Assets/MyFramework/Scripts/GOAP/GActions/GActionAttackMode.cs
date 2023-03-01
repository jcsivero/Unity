using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionAttackMode : GAction
{
  override public bool IsAchievableGiven(GoapStates conditions)
    {
        foreach (KeyValuePair<string, GenericData> p in preconditions_.GetStates())
        {
            if (!conditions.HasState(p.Key))
                return false;
        }
        ///si están todas las precondiciones en el estado del mundo y los estados del nps(del NPC) entonces paso a la función virtual en la que se puede
        ///comprobar los valores de las precondiciones según se necesite.
        ///Por defecto, solo con que aparezcan las precondiciones es suficiente para dar la acción por válida para agreagar a la cola.
        ///Por lo que por rendimiento, siempre será preferible eliminar estados del mundo o del NPC si no que cumplen las condiciones y así evitar
        //tener que entrar a la fucnión para comprobar sus valores, esto debería ser así siempre que sea posible naturalmente.
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
        
        
        /*target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;*/
        status_.anim_.SetTrigger("Patrol");
        return true;
    }

    public override bool PostPerform(bool timeOut = false,bool finishedByConditions = false) 
    {
        /*GWorld.Instance.GetWorld().ModifyState("treatingPatient", 1);
        GWorld.Instance.AddCublicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("freeCubicle", 1);*/
        return true;
    }
    override public  bool OnPerform()
    {
        return true;
    }
}
