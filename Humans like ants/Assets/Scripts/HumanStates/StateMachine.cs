using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State_Human state;

    public void SetState(State_Human _state, HumanBehaviour humanBehaviour, string stateName)
    {
        state = _state;
        StartCoroutine(state.BeforeStart(humanBehaviour, stateName));
    }
}
