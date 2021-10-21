using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : GoapAction
{
    // Start is called before the first frame update
    void Start()
    {
        actionName = "Eat";

        preconditions = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();

        preconditions.Add(new KeyValuePair<string, bool>("HasFood", true));
        preconditions.Add(new KeyValuePair<string, bool>("IsHungry", true));

        effects.Add(new KeyValuePair<string, bool>("IsHungry", false));
        effects.Add(new KeyValuePair<string, bool>("HasFood", false));
    }

    public override bool IsDone()
    {
        return agent.worldState[effects[0].Key] == effects[0].Value;
    }

    public override bool PerformAction()
    {
        return true;
    }
}
