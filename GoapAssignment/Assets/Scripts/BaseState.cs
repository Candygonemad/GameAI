using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseState
{
    //Multiple fsm states can use this to update themselves
    void Update(FSM fsm, GameObject gameObj);
}
