using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellStone : GoapAction
{
    // Start is called before the first frame update
    void Start()
    {
        actionName = "Sell Stone";

        preconditions = new List<KeyValuePair<string, bool>>();
        effects = new List<KeyValuePair<string, bool>>();

        preconditions.Add(new KeyValuePair<string, bool>("HasStones", true));
        preconditions.Add(new KeyValuePair<string, bool>("HasGold", false));

        effects.Add(new KeyValuePair<string, bool>("HasGold", true));
        effects.Add(new KeyValuePair<string, bool>("HasStones", false));
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
