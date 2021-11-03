using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceObject : MonoBehaviour
{
    public float returnAmount;
    public float currentAmount;
    public float maxAmount;
    public float regen;
    public float GainResource()
    {
        if (currentAmount >= returnAmount)
        {
            currentAmount -= returnAmount;
            return returnAmount;
        }
        else
        {
            return 0;
        }
    }
    private void Start()
    {
        StartCoroutine(Regen());
    }
    IEnumerator Regen()
    {
        if (currentAmount < maxAmount)
        {
            currentAmount = Mathf.Clamp(currentAmount + regen, 0, maxAmount);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(Regen());
    }
}
