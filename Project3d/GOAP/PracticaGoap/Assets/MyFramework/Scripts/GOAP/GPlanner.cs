using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Node parent;
    public float cost;
    public GoapStates state_;
    public GAction action;

    // Constructor
    public Node(Node parent, float cost, GoapStates allStates, GAction action)
    {

        this.parent = parent;
        this.cost = cost;
        this.state_ = new GoapStates(allStates);
        this.action = action;
    }
    public Node(Node parent, float cost, GoapStates allStates, GoapStates npcStates, GAction action)
    {

        this.parent = parent;
        this.cost = cost;
        this.state_ = new GoapStates(allStates);
        state_.SetOrAddStates(npcStates);
        this.action = action;
    }
}

public class GPlanner : Base
{

    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, GenericData> goal, GoapStates npcStates)
    {

        List<GAction> usableActions = new List<GAction>();

        foreach (GAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0.0f, GetStatusWorld().GetGoapStates(), npcStates, null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else if (leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }
        List<GAction> result = new List<GAction>();
        Node n = cheapest;

        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();

        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The Plan is: ");
        foreach (GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }
        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, GenericData> goal)
    {

        bool foundPath = false;
        foreach (GAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state_))
            {
                GoapStates currentState = new GoapStates(parent.state_);
                foreach (KeyValuePair<string, GenericData> eff in action.effects_.GetStates())
                {
                    if (!currentState.HasState(eff.Key))
                        currentState.SetOrAddState(eff.Key,eff.Value);

                    
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach (GAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

    private bool GoalAchieved(Dictionary<string, GenericData> goal, GoapStates state)
    {

        foreach (KeyValuePair<string, GenericData> g in goal)
        {
            if (!state.HasState(g.Key))
                return false;
        }
        return true;
    }
}
