using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class GAction : BaseMono
{
    [Header("=============== Common")]
    [Space(5)]               

    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;

    public bool mandatory_ = true; ///indica si esta acción es obligatoria para poder continuar con la siguiente acción del plan en caso de ser interrumpida
    ///por ya no cumplirse las precondicones o bien si se ejecutó el evento onGoapBreak. si es true, la acción es obligatoria y se crearía un nuevo plan en caso de 
    ///interrupción, si es false, se detendrá solo la acción en curso, se ejecutará su posperfom y se continuará con la siguiente acción del plan en caso de haber alguna.
    ///si fuera la última acción del plan, evidentemente se creará un plan nuevo.
    public bool blockAction_ = true; ///si es false, se intenta crear un nuevo plan en cada frame y si se consigue uno  de mayor prioridad entonces se 
    ///finaliza la acción y se pasa a ejecutar el siguiente plan.Esto es útil para cambiar de plan en base a los cambios en las condiciones de todos
    ///los objetivos y no solo en los de la propia acción, osea, si es true este valor, solo se anulará la acción y creará un nuevo plan si cambian 
    ///las condiciones de la propia acción o si ya ha terminado.
    ///Tenga en cuenta que las acciones no bloqueantes pueden consumir muchos recursos.

   [Header("=============== Preconditions")]
    [Space(5)]               
    public GoapStateFloat[] preConditionsFloat_;
    public GoapStateString[] preConditionsString_;
    public GoapStateInt[] preConditionsInt_;

   [Header("=============== Efectos")]
    [Space(5)]               

    public GoapStateInt[] afterEffectsInt_;
    public GoapStateFloat[] afterEffectsFloat_;
    public GoapStateString[] afterEffectsString_;
    public GAgent goapAgent_;
    public StatusNpc status_;
        

    public GoapStates preconditions_;
    public GoapStates effects_;
    public GoapStates npcGoapStates_;
    public GInventory inventory_;
    

    public bool running_ = false;

    public GAction()
    {
        preconditions_ = new GoapStates();
        effects_ = new GoapStates();
    }

    public void Awake()
    {        
        goapAgent_ = this.gameObject.GetComponent<GAgent>();
        inventory_ = goapAgent_.inventory_;
        npcGoapStates_ = goapAgent_.npcGoapStates_;
        status_ = gameObject.GetComponent<StatusNpc>(); //para acceder al componente Status del Npc en caso de tenerlo agregado.

        ///Agrego precondiciones y efectos de tipo entero agregadas en el inspector a la lista de  precondiciones y efectos globales.

        if (preConditionsInt_ != null)
            foreach (GoapStateInt w in preConditionsInt_)
            {
                preconditions_.SetOrAddState(w.key, GenericData.Create<int>(w.value));
            }

        if (afterEffectsInt_ != null)
            foreach (GoapStateInt w in afterEffectsInt_)
            {
                effects_.SetOrAddState(w.key, GenericData.Create<int>(w.value));
            }
        
         ///Agrego precondiciones y efectos de tipo float agregadas en el inspector a la lista de  precondiciones y efectos globales.

        if (preConditionsFloat_ != null)
            foreach (GoapStateFloat w in preConditionsFloat_)
            {
                preconditions_.SetOrAddState(w.key, GenericData.Create<float>(w.value));
            }

        if (afterEffectsFloat_ != null)
            foreach (GoapStateFloat w in afterEffectsFloat_)
            {
                effects_.SetOrAddState(w.key, GenericData.Create<float>(w.value));
            }
        ///Agrego precondiciones y efectos de tipo String agregadas en el inspector a la lista de  precondiciones y efectos globales.

        if (preConditionsString_ != null)
            foreach (GoapStateString w in preConditionsString_)
            {
                preconditions_.SetOrAddState(w.key, GenericData.Create<string>(w.value));
            }

        if (afterEffectsString_ != null)
            foreach (GoapStateString w in afterEffectsString_)
            {
                effects_.SetOrAddState(w.key, GenericData.Create<string>(w.value));
            }


    }

    public void AddPreConditions(string key, GenericData value)
    {
        preconditions_.SetOrAddState(key, value);
    }
    public void AddEffects(string key, GenericData value)
    {
        effects_.SetOrAddState(key, value);
    }
    
    ///comprueba si se sigue cumpliendo el filtrado de esta acción en concreto y si las precondiciones del estado del mundo y del npc siguen siendo válidas.
    ///normalmente se comprueba antes de realizar la acción.
    public bool CheckConditions(out Reason reason,bool breakPlanOrAcction)
    {
        
        if ((IsAchievable()) && (IsAchievableGiven(GetAllStates(),false))) ///si todas el filtrado y todas las precondiciones tando del mundo como del npc están disponibles y se cumplen,
        {
            ///se procede al PrePerform, el cuál todavía puede volver a filtrar y por consiguiente romper el plan creado, obligando a crear un nuevo plan.
            ///en esta comprobación no se tienen en cuenta las precondiciones volátiles que se pusieron para poder crear algunos planes, pero que no están ni en el estado del mundo ni en el del NPC
            if (!breakPlanOrAcction)            
            {
                reason = Reason.success;  
                return true;
            }  
                
            else
            {
                reason = Reason.onGoapBreakCalled;  
                return false;
            }
        }   
            else
            {
                reason = Reason.conditionsAtionsChanged;  
                return false;
            }        
            
    }
    public GoapStates GetAllStates() ///devuelve los estados del mundo junto con los estados del NPC actual.
    {
        GoapStates allStates = new GoapStates(GetStatusWorld().GetGoapStates());
        allStates.SetOrAddStates(npcGoapStates_);
        return allStates;
    }
    ///puedo filtrar la acción y evitar que sea computada por el planificador teniendo en cuenta cualquier consideración
    virtual public bool IsAchievable()
    {
          return true;
    }
    ///ya con las acciones filtradas, compruebo todo el estado, primero compruebo simplemente que estén todas las condiciones presentes.
    ///si es así, esta función llama a IsAchievableGivenCustomize, donde puedo ya manualmente introducir el código para comprobar los valores de cada estado concretamente.
    ///El valor planMode indica que se están comprobando las precondiciones mientras se está intentando crear un plan, o esa, desde la función BuildGraph() de la clase GPlanner.
    ///Esto es debido a que muchas precondiciones no tienen por que pertenecer ni al estado del mundo ni al del NPC, simplemente están agregadas como
    ///cadena de precondiciones para poder llegar a un objetivo. El problema es que estas precondiciones adhoc, no se almacen ni en el estado del mundo ni del NPC,
    ///por lo que a la hora de comprobar si se siguen cumpliendo las precondicones antes de ejecutar una accion(esto se realiza comprobandeo las precondiciones de la acción con el estado
    //del mundo y del NPC) daría error. Así que la solución es que se permita esta comprobación light de precondiciones a la hora de crear un plan con la varialbe
    ///planMode = true y cuando se comprueban estas precondiciones antes de ejecutar cada acción, no se compruebe todo el estado de variables del mundo y del NPC, simplemente
    ///se haga la comprobación manual que se programa para cada acción en concreto.
    virtual public bool IsAchievableGiven(GoapStates conditions,bool planMode = false )
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
    
    ///esta funciónes llamada por IsAchievableGiven y sirve para comprobar las condiciones con su valor correspondiente y deseado, no limitándose simplemetne a comprobar que la precondición
    ///exista en el estado.
    virtual protected bool IsAchievableGivenCustomize(GoapStates conditions)
    {
        return true;
    }

    ///se ejecuta justo antes de iniciar la acción. Se sabe en esta ejecución que el estado del mundo y del NPC se sigue cumpliendo, o sea, todas las precondiciones.
    ///Aún así, si en el PrePerform se devuelve false, el plan entero se detendrá forzando la creación de uno nuevo. Por ejemplo en el PrePerform, se puede realizar
    ///comprobaciones extras antes de iniciar la acción.
    virtual public bool PrePerform()
    {
        return true;
    }
    
    ///esta función es llamada durante la duración de la acción, en cada frame y ya se ha comprobado justo antes de cada llamada, que las condiciones no han cambiado.
    /// si esta función devuelve true, terminará el plan llamando al posperform, indicando que se completó la acción.
    ///después se completará completamente dependiendo de si se estableción un tiempo de duración.
    ///Para continuar la ejecución, esta función devolverá false indicando que no ha terminado su acción mientras siga teniendo cosas que hacer.

    virtual public bool OnPerform()
    {
        return true;
    }

        
    ///Esta función no debe ser llamada directamente, en su lugar, se llamará automáticamente si han cambiado las condiciones mientras se ejecuta la acción
    /// o bien si ha terminado el tiempo asignado a la acción. Si se quiere provocar terminar la acción y así que se ejecute esta función, se deberá hacer cambiando
    ///las condiciones para que la acción en ejecución deje de ser válida o bien devolver false en la función OnPerform.
    ///En este último caso, hay que tener en cuenta que podría volver a generarse el mismo plan de acciones, puesto que las precondiciones seguirían siendo válidas.
    //como valor por defecto, el posperform es llamado 
    /// suponiendo que se terminó la acción puesto que cumplió su objetivo, esto es, llegar a algún punto en concreto, realizar varias tareas y cumplirlas...
    ///Esta forma de terminar es cuando la funcion OnPerForm devuelve true.
    ///el timeOut a true significa que se ejecuta el PostPerform después de que la tarea ha terminado y se llegó al final del tiempo establecido en sus propiedaes,
    ///es como un tiempo de espera.
    ///finishedByConditions, indica que se llegó al postperform por algún cambio en las condiciones, de forma que ya no se cumple

    virtual public  void PostPerform(Reason reason)
    {
    
    }

}
