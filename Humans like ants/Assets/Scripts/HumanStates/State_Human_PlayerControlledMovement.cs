using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_PlayerControlledMovement : State_Human
{
    public State_Human_PlayerControlledMovement(HumanBehaviour humanBehaviour, string nameOfState) : base(humanBehaviour, nameOfState)
    {
    }
    public override IEnumerator BeforeStart(HumanBehaviour humanBehaviour, string nameOfState)
    {
        return base.BeforeStart(humanBehaviour, nameOfState);
    }

    public override IEnumerator Action()
    {
        return base.Action();
    }

    public override IEnumerator DestinationReached()
    {
        return base.DestinationReached();
    }

    public override IEnumerator Start()
    {
        return base.Start();
    }
}
