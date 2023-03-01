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
    public GoapStates npcStates_ = new GoapStates();
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


    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running_ = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        if (currentAction != null && currentAction.running_)
        {
            // si el navmesh no está calculando bien el remaining distance, se puede
            //calcular la distancia a mano.
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            //if (currentAction.agent.hasPath && distanceToTarget < 2f) //currentAction.agent.remainingDistance < 2f)
            if (distanceToTarget < 2f) 
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals_ orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions_, sg.Key.sgoals, npcStates_);
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
            if (currentAction.PrePerform())
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
            }

        }

    }
    
    abstract public void AddActions();
    
    abstract public void AddGoals();
}
