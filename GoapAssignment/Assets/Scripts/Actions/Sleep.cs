using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : GoapAction
{
    // Start is called before the first frame update
    void Start()
    {
        actionName = "Sleep";

        preconditions = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();

        preconditions.Add(new KeyValuePair<string, bool>("IsHungry", false));
        preconditions.Add(new KeyValuePair<string, bool>("Rested", false));

        effects.Add(new KeyValuePair<string, bool>("Rested", true));
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
