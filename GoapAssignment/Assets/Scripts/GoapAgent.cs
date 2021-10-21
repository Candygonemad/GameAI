using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoapAgent : MonoBehaviour
{
    //Used to compare to preconditions and will be changed by effects of actions
    public Dictionary<string, bool> worldState;

    //What the agent wants the world state to be
    public KeyValuePair<string, bool> goal;

    //Keeps track of the states
    public FSM fsm;

    //The different states that will be pushed and popped
    public FSM.BaseState idleState;
    public FSM.BaseState moveToState;
    public FSM.BaseState performActionState;

    //Determines what the plan of actions will be
    public GoapPlanner planner;

    public float speed = 2f;

    public Text currentStateText;

    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM();

        //Setting up the world states and goal
        worldState = new Dictionary<string, bool>();
        worldState.Add("HasPickaxe", false);
        worldState.Add("PickaxeAvailable", true);
        worldState.Add("HasStones", false);
        worldState.Add("HasGold", false);
        worldState.Add("HasFood", false);
        worldState.Add("IsHungry", true);
        worldState.Add("Rested", false);

        goal = new KeyValuePair<string, bool>("Rested", true);

        CreateIdleState();
        CreateMoveToState();
        CreatePerformActionState();

        fsm.PushState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update(this.gameObject);
    }

    //Whevenever the idle state gets updated/invoked it will start PLANNING
    public void CreateIdleState()
    {
        idleState = (fsm, gameObject) =>
        {
            Queue<GoapAction> plan = planner.Plan();

            if (plan != null)
            {
                fsm.PopState();
                fsm.PushState(performActionState);
            }
            else
            {
                
            }
        };
    }

    //Whevenever the MoveTo state gets updated/invoked it will move the agent and then pop it if it is done moving
    public void CreateMoveToState()
    {
        moveToState = (fsm, gameObject) =>
        {
            GoapAction currentAction = planner.actionPlan.Peek();
            if (MoveAgent(currentAction))
                fsm.PopState();
        };
    }

    //Whevenever the PerformAction state gets updated/invoked it will perform the current action if it is range
    public void CreatePerformActionState()
    {
        performActionState = (fsm, gameObject) =>
        {

            GoapAction currentAction = planner.actionPlan.Peek();

            //Is the action done, move onto the next action
            if (currentAction.IsDone())
            {
                    
                planner.actionPlan.Dequeue();

                //Have we achieved our goal? If so clear the Queue, then Reset the world state
                if (currentAction.effects[0].Key == "Rested" && worldState[currentAction.effects[0].Key] == currentAction.effects[0].Value)
                {
                    planner.actionPlan.Clear();
                    ResetWorldState();
                }
            }

            if(planner.HasActionPlan())
            {
                currentAction = planner.actionPlan.Peek();

                currentStateText = GameObject.Find("CurrentState").GetComponent<Text>();
                currentStateText.text = currentAction.actionName;

                bool inRange;
                //Either move to the target or perform action

                if (currentAction.RequiresInRange())
                {
                    inRange = currentAction.InRange();
                }
                    
                else
                {
                    inRange = true;
                }

                if (inRange)
                {
                    currentAction.PerformAction();
                    worldState = planner.PopulateState(worldState, planner.actionPlan.Peek().effects);
                }
                else
                {
                    fsm.PushState(moveToState);
                }
            }
            //If theres not actions to take go to idle and plan
            else
            {
                fsm.PopState();
                fsm.PushState(idleState);
            }

        };
    }

    //Move the agent towards the target at some speed over time
    public bool MoveAgent(GoapAction action)
    {
        float amount = speed * Time.deltaTime;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, action.target.transform.position, amount);

        if(action.InRange())
        {
            return true;
        }

        return false;
    }

    //Give default values to the states
    public void ResetWorldState()
    {
        worldState = new Dictionary<string, bool>();
        worldState.Add("HasPickaxe", false);
        worldState.Add("PickaxeAvailable", true);
        worldState.Add("HasStones", false);
        worldState.Add("HasGold", false);
        worldState.Add("HasFood", false);
        worldState.Add("IsHungry", true);
        worldState.Add("Rested", false);
    }
}
