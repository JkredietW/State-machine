using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_WoodCutter : State_Human
{
    public State_Human_WoodCutter(HumanBehaviour humanBehaviour, string nameOfState) : base(humanBehaviour, nameOfState)
    {
    }

    public override IEnumerator Action()
    {
        float consumption = behaviour.eatplace.GainResource();
        behaviour.currentWorkValue = Mathf.Clamp(behaviour.currentWorkValue + behaviour.addedWork, 0, behaviour.maxWorkValue);
        if (consumption == 0)
        {
            behaviour.CheckState(0);
        }
        else
        {
            behaviour.woodCarried = Mathf.Clamp(behaviour.woodCarried += consumption, 0, behaviour.maxWoodCarried);

            if (behaviour.woodCarried == behaviour.maxWoodCarried)
            {
                GameManager gam = FindObjectOfType<GameManager>();
                if (gam.isNight)
                {
                    behaviour.CheckState(4);
                }
                else
                {
                    behaviour.CheckState(0);
                }

                StopCoroutine(Action());
            }
        }
        yield return new WaitForSeconds(1);
        if (behaviour.woodCarried < behaviour.maxWoodCarried && behaviour.eatplace.currentAmount > 0 && behaviour.currentWorkValue < behaviour.maxWorkValue)
        {
            StartCoroutine(Action());
        }
        else
        {
            //goes back from eating stuff
            if (FindObjectOfType<GameManager>().GetTime() == true)
            {
                behaviour.CheckState(4);
            }
            else
            {
                behaviour.CheckState(0);
            }
        }
    }

    public override IEnumerator DestinationReached()
    {
        StartCoroutine(Action());
        yield break;
    }

    public override IEnumerator Start()
    {
        WoodSource[] woodSources = FindObjectsOfType<WoodSource>();
        float closest = Mathf.Infinity;
        Vector3 goHere = Vector3.zero;
        for (int i = 0; i < woodSources.Length; i++)
        {
            if (woodSources[i].currentAmount < 10)
                continue;
            float distance = Vector3.Distance(behaviour.transform.position, woodSources[i].transform.position);
            if (distance < closest)
            {
                closest = distance;
                goHere = woodSources[i].transform.position;
                behaviour.eatplace = woodSources[i];
            }
        }
        if (closest == Mathf.Infinity)
        {
            behaviour.role = Job.hobo;
            behaviour.CheckState(0);
            behaviour.eatplace = null;
        }
        behaviour.finding.GiveNewTarget(goHere);
        yield break;
    }
    public override IEnumerator BeforeStart(HumanBehaviour humanBehaviour, string nameOfState)
    {
        return base.BeforeStart(humanBehaviour, nameOfState);
    }
}
