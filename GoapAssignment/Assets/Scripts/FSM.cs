using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    //Determines the order of what state to look at next
    Stack<BaseState> stateStack = new Stack<BaseState>();


    //Talks to the BaseState to update the fsm states
    public delegate void BaseState(FSM fsm, GameObject gameObj);

    public void Update(GameObject gameObject)
    {
        Debug.Log("Stack Count: " + stateStack.Count);
        //Update the current state using the delegate
        if(stateStack.Count != 0)
        {
            Debug.Log("Invoked");
            stateStack.Peek().Invoke(this, gameObject);
        }
    }

    //Push a state onto the stack
    public void PushState(BaseState state)
    {
        stateStack.Push(state);
    }

    //Take a state off the stack
    public void PopState()
    {
        stateStack.Pop();
    }
}
