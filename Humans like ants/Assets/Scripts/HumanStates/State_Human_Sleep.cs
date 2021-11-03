using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_Sleep : State_Human
{
    public State_Human_Sleep(HumanBehaviour humanBehaviour, string nameOfState) : base(humanBehaviour, nameOfState)
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
        //go into house
        if(behaviour.woodCarried > 0)
        {
            behaviour.restplace.StoreWood(behaviour.woodCarried);
            behaviour.woodCarried = 0;
        }
        if (behaviour.stoneCarried > 0)
        {
            behaviour.restplace.StoreStones(behaviour.stoneCarried);
            behaviour.stoneCarried = 0;
        }
        if (behaviour.foodCarried > 0)
        {
            behaviour.restplace.StoreFood(behaviour.foodCarried);
            behaviour.foodCarried = 0;
        }

        if (!FindObjectOfType<GameManager>().isNight)
        {
            behaviour.CheckState(0);
        }
        else
        {
            behaviour.currentWorkValue = 0;
        }
        yield break;
    }

    public override IEnumerator Start()
    {
        SleepLocation[] houses = FindObjectsOfType<SleepLocation>();
        float closest = Mathf.Infinity;
        Vector3 goHere = Vector3.zero;
        for (int i = 0; i < houses.Length; i++)
        {
            float distance = Vector3.Distance(behaviour.transform.position, houses[i].transform.position);
            if (distance < closest)
            {
                closest = distance;
                goHere = houses[i].transform.position;
                behaviour.restplace = houses[i];
            }
        }
        behaviour.finding.GiveNewTarget(goHere);
        yield break;
    }
}
