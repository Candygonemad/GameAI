using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wake : GoapAction
{
    // Start is called before the first frame update
    void Start()
    {
        preconditions = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();

        preconditions.Add(new KeyValuePair<string, bool>("IsHungry", false));
        preconditions.Add(new KeyValuePair<string, bool>("Rested", true));

        effects.Add(new KeyValuePair<string, bool>("Rested", false));
        effects.Add(new KeyValuePair<string, bool>("IsHungry", true));
    }

    public override bool IsDone()
    {
        return (agent.worldState[effects[0].Key] == effects[0].Value) && (agent.worldState[effects[1].Key] == effects[1].Value);
    }

    public override bool PerformAction()
    {
        return true;
    }
}
