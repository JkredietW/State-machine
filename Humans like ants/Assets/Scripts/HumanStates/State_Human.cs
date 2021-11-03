using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State_Human : MonoBehaviour
{
    public string stateTag;
    protected HumanBehaviour behaviour;
    protected float distanceToDestination;

    public State_Human(HumanBehaviour humanBehaviour, string nameOfState)
    {
        behaviour = humanBehaviour;
        stateTag = nameOfState;
    }

    public virtual IEnumerator BeforeStart(HumanBehaviour humanBehaviour, string nameOfState)
    {
        behaviour = humanBehaviour;
        stateTag = nameOfState;
        StartCoroutine(Start());
        yield break;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }
    public virtual IEnumerator Action()
    {
        yield break;
    }
    public virtual IEnumerator DestinationReached()
    {
        yield break;
    }
}
