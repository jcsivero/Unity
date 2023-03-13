using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GActionPatrolMode : GAction
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
        ///comprubo manualmente las condiciones con los valores que me interesa
        ///Aquí tengo la ventaja de que no necesito comprobar si existe un valor, puesto que IsArchievableGiven ya comprobó que existan todas.
        
        /*if (npcGoapStates_.GetState("distance").GetValue<float>()> status_.GetVisDistance())
            return true; 
        else
            return false;*/ ///no hace falta precondición, puesto que el modo patrol será el último de los objetivos posibles.
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
                status_.anim_.SetTrigger("Idle");
                return true;

    }
    override public  bool OnPerform()
    {
        status_.GetAIController().PatrolMode(status_,false);
        return false; ///devuevlo false puesto que en modo patrol esta acción no debe de terminar nunca a  no ser que cambien las condiciones.

    }
}
