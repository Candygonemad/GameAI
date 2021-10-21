using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPickaxe : GoapAction
{
    // Start is called before the first frame update
    void Start()
    {
        actionName = "Get Pickaxe";

        preconditions = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();

        preconditions.Add(new KeyValuePair<string, bool>("HasPickaxe", false));
        preconditions.Add(new KeyValuePair<string, bool>("PickaxeAvailable", true));

        effects.Add(new KeyValuePair<string, bool>("HasPickaxe", true));
        effects.Add(new KeyValuePair<string, bool>("PickaxeAvailable", false));
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
