using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_Farmer : State_Human
{
    public State_Human_Farmer(HumanBehaviour humanBehaviour, string nameOfState) : base(humanBehaviour, nameOfState)
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
            behaviour.foodCarried = Mathf.Clamp(behaviour.foodCarried += consumption, 0, behaviour.maxFoodCarried);

            if (behaviour.foodCarried == behaviour.maxFoodCarried)
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
        if (behaviour.foodCarried < behaviour.maxFoodCarried && behaviour.eatplace.currentAmount > 0 && behaviour.currentWorkValue < behaviour.maxWorkValue)
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
        FoodSource[] foodSources = FindObjectsOfType<FoodSource>();
        float closest = Mathf.Infinity;
        Vector3 goHere = Vector3.zero;
        for (int i = 0; i < foodSources.Length; i++)
        {
            if (foodSources[i].currentAmount < 10)
                continue;
            float distance = Vector3.Distance(behaviour.transform.position, foodSources[i].transform.position);
            if (distance < closest)
            {
                closest = distance;
                goHere = foodSources[i].transform.position;
                behaviour.eatplace = foodSources[i];
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
