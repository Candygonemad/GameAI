using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCosts : MonoBehaviour
{
    public GameObject agent;

    private bool toggle = false;

    public void ChangeActionCosts()
    {
        if(toggle)
        {
            agent.GetComponent<GetPickaxe>().cost = 10;
            agent.GetComponent<FindFood>().cost = 1;
        }
        else
        {
            agent.GetComponent<GetPickaxe>().cost = 2;
            agent.GetComponent<FindFood>().cost = 12;
        }

        toggle = !toggle;
        agent.GetComponent<GoapAgent>().fsm.PopState();
        agent.GetComponent<GoapAgent>().fsm.PushState(agent.GetComponent<GoapAgent>().idleState);
    }
}
