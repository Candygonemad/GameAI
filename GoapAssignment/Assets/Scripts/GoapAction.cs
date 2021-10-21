using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    public List<KeyValuePair<string, bool>> preconditions;
    public List<KeyValuePair<string, bool>> effects;

    public GameObject target;

    public bool requiresInRange;
    public float range;

    public int cost;

    public GoapAgent agent;

    [HideInInspector]
    public string actionName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Do I need to be close to the target to perform action
    public bool RequiresInRange()
    {
        return requiresInRange;
    }

    //In Range? Yes = True No = False
    public bool InRange()
    {
        return Vector3.Distance(target.transform.position, gameObject.transform.position) <= range;
    }

    //Are you done performing action?
    public abstract bool IsDone();

    //Can this action actually run? Check the preconditions
    public bool CheckPreconditions(Dictionary<string, bool> worldState)
    {
        foreach (KeyValuePair<string, bool> precondition in preconditions)
        {
            if (worldState.ContainsKey(precondition.Key))
                if ((bool)(worldState[precondition.Key]) != precondition.Value)
                    return false;
        }

        return true;
    }

    //Do the action
    public abstract bool PerformAction();
}
