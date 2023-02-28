using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public GoapStateInt[] preConditionsInt;
    public GoapStateInt[] afterEffectsInt;
    public NavMeshAgent agent;


    public Dictionary<string, GenericData> preconditions;
    public Dictionary<string, GenericData> effects;


    public GInventory inventory;
    public GoapStates beliefs;

    public bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, GenericData>();
        effects = new Dictionary<string, GenericData>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if (preConditionsInt != null)
            foreach (GoapStateInt w in preConditionsInt)
            {
                preconditions.Add(w.key, GenericData.Create<int>(w.value));
            }

        if (afterEffectsInt != null)
            foreach (GoapStateInt w in afterEffectsInt)
            {
                effects.Add(w.key, GenericData.Create<int>(w.value));
            }
        inventory = this.GetComponent<GAgent>().inventory;
        beliefs = this.GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievableGiven(Dictionary<string, GenericData> conditions)
    {
        foreach (KeyValuePair<string, GenericData> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        ///si están todas las precondiciones en el estado del mundo y los beliefs(del NPC) entonces paso a la función virtual en la que se puede
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
