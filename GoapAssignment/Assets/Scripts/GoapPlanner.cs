using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner : MonoBehaviour
{
    public GoapAgent agent;

    public List<GoapAction> availableActions;
    public Queue<GoapAction> actionPlan;
    
    //Plan the sequence of actions
    public Queue<GoapAction> Plan()
    {
        Queue<GoapAction> actionDecision = new Queue<GoapAction>();

        GoapAction addAction = null;
        int lowestCost = int.MaxValue;

        Debug.Log("Available Actions: " + availableActions.Count);

        //Go through the available actions and get the lowest cost one that meets its preconditions
        foreach (GoapAction action in availableActions)
        {
            if (action.CheckPreconditions(agent.worldState))
            {
                if (addAction == null)
                {
                    addAction = availableActions[0];
                    lowestCost = addAction.cost;
                }
                else if (action.cost < lowestCost)
                {
                    addAction = action;
                    lowestCost = action.cost;
                }
            }
        }

        if (addAction == null)
            return null;

        //Add the lowest cost action to the queue
        actionDecision.Enqueue(addAction);

        //Update the world state so that we can make new actions afterward
        Dictionary<string, bool> changedWorldState = PopulateState(agent.worldState, addAction.effects);

        //Make the action plan using the new changed world state
        actionPlan = MakeActionDecision(actionDecision, changedWorldState, lowestCost);

        Debug.Log("Action Plan Count: " + actionPlan.Count);

        return actionPlan;
    }

    //Recursively call this method to get the next layer of actions
    public Queue<GoapAction> MakeActionDecision(Queue<GoapAction> actionDecision, Dictionary<string, bool> changedWorldState, int runningCost)
    {
        Debug.Log("GOT to action decision");
        List<GoapAction> nextActions = new List<GoapAction>();

        //Add actions that pass the precondition test
        foreach (GoapAction action in availableActions)
        {
            Debug.Log("Action Cost: " + action.cost);
            if (action.CheckPreconditions(changedWorldState))
            {
                Debug.Log("Action Added: " + action.cost);
                nextActions.Add(action);
            }
        }
        
        //Are there actions that can be made
        if(nextActions.Count > 0)
        {
            int lowestCost = nextActions[0].cost;
            GoapAction nextAction = nextActions[0];

            //Find the lowest cost action in the actions that passed the precondition test
            for (int i = 1; i < nextActions.Count; i++)
            {
                if (nextActions[i].cost < lowestCost)
                {
                    lowestCost = nextActions[i].cost;
                    nextAction = nextActions[i];
                }
            }

            //Add the lowest cost action to the action plan
            actionDecision.Enqueue(nextAction);

            //Update the world state
            changedWorldState = PopulateState(changedWorldState, nextAction.effects);

            //Recursive call
            actionDecision = MakeActionDecision(actionDecision, changedWorldState, runningCost + lowestCost);

                
        }

        return actionDecision;
    }

    //Are there any actions in the action plan? If so there is an action plan
    public bool HasActionPlan()
    {
        return actionPlan.Count > 0;
    }

    //Change the world state based on the action effects
    public Dictionary<string, bool> PopulateState(Dictionary<string, bool> currentState, List<KeyValuePair<string, bool>> changes)
    {
        Dictionary<string, bool> newState = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, bool> s in currentState)
        {
            newState.Add(s.Key, s.Value);
        }

        foreach (KeyValuePair<string, bool> c in changes)
        {
            if(newState.ContainsKey(c.Key))
            {
                newState[c.Key] = c.Value;
            }
        }

        return newState;
    }
}
