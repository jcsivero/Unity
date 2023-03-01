using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, GenericData> sgoals;
    public bool remove;

    public SubGoal(string s, GenericData i, bool r)
    {
        sgoals = new Dictionary<string, GenericData>();
        sgoals.Add(s, i);
        remove = r;
    }
}

abstract public class GAgent : BaseMono
{
    public List<GAction> actions_ = new List<GAction>();
    public Dictionary<SubGoal, int> goals_ = new Dictionary<SubGoal, int>();
    public GoapStates npcGoapStates_ = new GoapStates();
    public GInventory inventory_ = new GInventory();

    private StatusNpc status_;

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    // Start is called before the first frame update
    public void Start()
    {
        status_ = gameObject.GetComponent<StatusNpc>(); //para acceder al componente Status del Npc en caso de tenerlo agregado.
        
        
        AddGoals(); ///agrego los objetivos de este NPC.

        AddActions(); ///agrego acciones que no fueron agregadas como componenetes desde el inspector. Esto son acciones que tienen un tipo de datos que no es serializable.
        
        GAction[] acts = this.GetComponents<GAction>();         ////paso a una lista las acciones agregadas como componentes al GameObject del NPC desde el inspector.
                ////estas acciones se agregarán a las acciones agregadas manualmente por la función AddActions()
        foreach (GAction a in acts)
            actions_.Add(a);
        
        
        
    }


    bool completeActionByDurationInvoked = false;
    void CompleteActionByDuration() ///termina la acción por tiempo. solo cuando la variable duración es mayor de 0.
    {
        currentAction.running_ = false;
        currentAction.PostPerform(true,false);
        completeActionByDurationInvoked = false;
    }

    void CompleteAction() ///terminó la acción por algún cambio en el estado de las precondiciones o bien porque terminó devolviendo false la
    //función OnPerform()
    {
        currentAction.running_ = false;
        currentAction.PostPerform(false,false);        
    }

    void CompleteActionByConditions() ///terminó la acción por algún cambio en el estado de las precondiciones o bien porque terminó devolviendo false la
    //función OnPerform()
    {
        currentAction.running_ = false;
        currentAction.PostPerform(false,true);        
    }

    void LateUpdate()
    {
        if (currentAction != null && currentAction.running_)
        {
            if (!completeActionByDurationInvoked)
            {
                if (currentAction.CheckConditions())
                {
                    if (currentAction.OnPerform())
                    {
                        ///se completó la acción porque devuelve true OnPerform, o sea, se llegó al destino, se cumplió lo que se quería hacer....
                        if ((!completeActionByDurationInvoked) && (currentAction.duration > 0)) ///si configuré duración posterior de la acción antes de llamar al PostPerform y
                        //dar por terminada la acción.
                        {
                            Invoke("CompleteActionByDuration", currentAction.duration);
                            completeActionByDurationInvoked = true;
                        }
                        else
                            CompleteAction(); ///en caso contrario termino la acción haciendo lo que haya en PostPerform

                    }                        
                }
                else
                    CompleteActionByConditions();

            }

            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals_ orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions_, sg.Key.sgoals, npcGoapStates_);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals_.Remove(currentGoal);
            }
            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            ///se debe de cumplir todas las condiciones justo antes de ejecutar la acción, por si el estado del mundo o del npc han cambiado.
            ///si es así, se dará por iniciada la acción. Sino, se creará un plan nuevo.
            if (currentAction.CheckConditions())
            {
                Debug.Log("superado checkconditions");
                if (currentAction.PrePerform())
                {
                    Debug.Log("superado preperform");
                    currentAction.running_ = true;
                }
                    
                else
                    actionQueue = null;

            }
            else
                    actionQueue = null;

            /*if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.target != null)
                {
                    currentAction.running_ = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }*/

        }

    }
    
    abstract public void AddActions();
    
    abstract public void AddGoals();
}
