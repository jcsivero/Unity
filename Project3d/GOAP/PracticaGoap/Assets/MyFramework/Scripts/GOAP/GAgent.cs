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
    int priorityGoalActual_ ; ///indica la prioridad del objetivo en curso.Esta prioridad es de forma ascendente, prioridad 1 es más que 2.
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
        
        planner = new GPlanner();

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
    //función OnPerform(). 
    ///Nota: Tener en cuenta, que esta forma de terminar la acción indica que fue por cambio de las propias precondiciones de la acción, pero el plan
    ///seguirá ejecutándose con las siguientes acciones hasta intentar completar el objetivo. NO SE CREA UN PLAN NUEVO, hay que tenerlo en cuenta porque 
    ///si se quieren utilizar acciones que no tienen realmente final,como la acción de patrullar de un NPC por ejemplo, es mejor que estas acciones sean
    ///también un objetivo,  o dicho de otra forma, un objetivo por acción para las que nunca tienen por qué terminar.
    {
        currentAction.running_ = false;
        currentAction.PostPerform(false,true);        
    }

    void ExecutePlan()
    {
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


        }
        
    }
    Queue<GAction> GetPlanOrPriorityPlan() ///Devuelve un plan o intenta conseguir uno de mayor prioridad si ya había un plan. Devuelve null si no se consiguió plan
    {
        Queue<GAction> actionQueueDraft = null;                
        var sortedGoals = from entry in goals_ orderby entry.Value ascending select entry;

        foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
        {
            if (actionQueue == null) ///si no hay ningun plan, o sea, su cola de acciones está a null.
                actionQueueDraft = planner.plan(actions_, sg.Key.sgoals, npcGoapStates_);

            else  if (sg.Value < priorityGoalActual_) ///sino , solo compruebo los planes con más prioridad que el actual y en caso de conseguir uno más prioritario, lo cambio.
                      actionQueueDraft = planner.plan(actions_, sg.Key.sgoals, npcGoapStates_);

            

            if (actionQueueDraft != null)
            {                       
                currentGoal = sg.Key; ///solo cambio de objetivo si tiene mayor prioridad.
                priorityGoalActual_ = sg.Value;      
                break;                     
            }            

        }

        return actionQueueDraft;
    }

    void LateUpdate()
    {
        
        if (currentAction != null && currentAction.running_)
        {
            Queue<GAction> actionQueueDraft  = null;

            if (!currentAction.blockAction_) ///si la acción no está bloqueada, intento ver si han cambiado las condiciones para poderse ejecutar
            ///otro plan más prioritario, esto lo hago intentando crear otro plan.                 
                      actionQueueDraft  = GetPlanOrPriorityPlan();

                if (actionQueueDraft == null) ///si se sigue el plan actual
                {
                    if (currentAction.CheckConditions()) ///compruebo que las condiciones de la acción se siguen cumpliendo.                
                    {                        
                        if (!completeActionByDurationInvoked)
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
                    }
                    else
                        CompleteActionByConditions();
                }
                else
                {                    
                    CompleteActionByConditions();
                    actionQueue = actionQueueDraft; ///establezco la nueva cola de acciones puesto que se cambio de plan durante la ejecución de una acción.
                }                
                    
                
        }
        else
        {
            if (actionQueue == null)
                actionQueue =  GetPlanOrPriorityPlan(); ///intenta obtener un plan solo si no hay ninguno. Si no compruebo esto, podría crear un plan más prioritario en caso de encontrarlo.
            
            if (actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove)
                    goals_.Remove(currentGoal);
                actionQueue = null;
            }
            
            ExecutePlan();
            
        }

    }
    
    abstract public void AddActions();
    
    abstract public void AddGoals();
}
