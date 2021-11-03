using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_Idle : State_Human
{
    public State_Human_Idle(HumanBehaviour humanBehaviour , string nameOfState) : base(humanBehaviour, nameOfState)
    {
    }

    public override IEnumerator Action()
    {
        behaviour.currentWorkValue = Mathf.Clamp(behaviour.currentWorkValue -= behaviour.workLoss, 0, behaviour.maxWorkValue);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Action());
    }

    public override IEnumerator Start()
    {
        StartCoroutine(Action());
        if (behaviour.idleLocation == Vector3.zero)
        {
            behaviour.idleLocation = behaviour.RollTargetLocation();
            behaviour.finding.GiveNewTarget(behaviour.idleLocation);
        }
        yield break;
    }
    public override IEnumerator DestinationReached()
    {
        behaviour.idleLocation = Vector3.zero;
        yield return new WaitForSeconds(1);
        GameManager gam = FindObjectOfType<GameManager>();
        if (gam.isNight)
        {
            behaviour.CheckState(4);
        }
        else
        {
            behaviour.CheckState(0);
        }
    }
    public override IEnumerator BeforeStart(HumanBehaviour humanBehaviour, string nameOfState)
    {
        return base.BeforeStart(humanBehaviour, nameOfState);
    }
}
