using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class GAction : BaseMono
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;

    public GoapStateFloat[] preConditionsFloat_;
    public GoapStateString[] preConditionsString_;
    public GoapStateInt[] preConditionsInt_;
    public GoapStateInt[] afterEffectsInt_;
    public GoapStateFloat[] afterEffectsFloat_;
    public GoapStateString[] afterEffectsString_;
    public NavMeshAgent agent;
    public StatusNpc status_;
        

    public GoapStates preconditions_;
    public GoapStates effects_;
    public GoapStates npcStates_;
    public GInventory inventory_;
    

    public bool running_ = false;

    public GAction()
    {
        preconditions_ = new GoapStates();
        effects_ = new GoapStates();
    }

    public void Awake()
    {        

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

        agent = this.gameObject.GetComponent<NavMeshAgent>();
        inventory_ = this.GetComponent<GAgent>().inventory_;
        npcStates_ = this.GetComponent<GAgent>().npcStates_;
        status_ = gameObject.GetComponent<StatusNpc>(); //para acceder al componente Status del Npc en caso de tenerlo agregado.
    }

    public void AddPreConditions(string key, GenericData value)
    {
        preconditions_.SetOrAddState(key, value);
    }
    public void AddEffects(string key, GenericData value)
    {
        preconditions_.SetOrAddState(key, value);
    }
    public bool IsAchievableGiven(Dictionary<string, GenericData> conditions)
    {
        foreach (KeyValuePair<string, GenericData> p in preconditions_.GetStates())
        {
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        ///si están todas las precondiciones en el estado del mundo y los estados del nps(del NPC) entonces paso a la función virtual en la que se puede
        ///comprobar los valores de las precondiciones según se necesite.
        ///Por defecto, solo con que aparezcan las precondiciones es suficiente para dar la acción por válida para agreagar a la cola.
        ///Por lo que por rendimiento, siempre será preferible eliminar estados del mundo o del NPC si no que cumplen las condiciones y así evitar
        //tener que entrar a la fucnión para comprobar sus valores, esto debería ser así siempre que sea posible naturalmente.
        return IsAchievableGivenCustomize(conditions);
    }
    
    abstract public bool IsAchievableGivenCustomize(Dictionary<string, GenericData> conditions);

    abstract public bool IsAchievable();
    
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
