using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Human_Hungry : State_Human
{
    public State_Human_Hungry(HumanBehaviour humanBehaviour, string nameOfState) : base(humanBehaviour, nameOfState)
    {
    }

    public override IEnumerator Action()
    {
        float consumption = 0;
        if (behaviour.eatplace.GetComponent<SleepLocation>())
        {
            consumption = behaviour.eatplace.GetComponent<SleepLocation>().GiveFood();
        }
        else
        {
            consumption = behaviour.eatplace.GainResource();
        }
        if (consumption == 0)
        {
            behaviour.CheckState(0);
        }
        else
        {
            behaviour.currentFoodValue += consumption;
            behaviour.UpdateUi();
            if (behaviour.currentFoodValue > behaviour.maxFoodValue * 0.9f)
            {
                GameManager gam = FindObjectOfType<GameManager>();
                if(gam.isNight)
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
        yield return new WaitForSeconds(0.5f);
        if (behaviour.currentFoodValue < behaviour.maxFoodValue * 0.9f)
        {
            StartCoroutine(Action());
        }
        else
        {
            //goes back from eating stuff
            if(FindObjectOfType<GameManager>().GetTime() == true)
            {
                print(4);
                behaviour.CheckState(4);
            }
            else
            {
                print(0);
                behaviour.CheckState(0);
            }
        }
    }
    public override IEnumerator Start()
    {
        SleepLocation[] houses = FindObjectsOfType<SleepLocation>();
        SleepLocation thisHouseHasFood = default;
        for (int i = 0; i < houses.Length; i++)
        {
            if(houses[i].foodStored > 0)
            {
                thisHouseHasFood = houses[i];

            }
        }
        if(thisHouseHasFood == default)
        {
            FoodSource[] foodObjects = FindObjectsOfType<FoodSource>();
            float closest = Mathf.Infinity;
            Vector3 goHere = Vector3.zero;
            for (int i = 0; i < foodObjects.Length; i++)
            {
                if (foodObjects[i].currentAmount < foodObjects[i].maxAmount * 0.1f)
                {
                    continue;
                }
                float distance = Vector3.Distance(behaviour.transform.position, foodObjects[i].transform.position);
                if (distance < closest)
                {
                    closest = distance;
                    goHere = foodObjects[i].transform.position;
                    behaviour.eatplace = foodObjects[i];
                }
            }
            behaviour.finding.GiveNewTarget(goHere);
        }
        else
        {
            behaviour.eatplace = thisHouseHasFood;
            behaviour.finding.GiveNewTarget(thisHouseHasFood.transform.position);
        }
        return base.Start();
    }
    public override IEnumerator DestinationReached()
    {
        StartCoroutine(Action());
        yield break;
    }
    public override IEnumerator BeforeStart(HumanBehaviour humanBehaviour, string nameOfState)
    {
        return base.BeforeStart(humanBehaviour, nameOfState);
    }
}
