using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SleepLocation : SourceObject
{
    public int foodStored;
    public int woodStored;
    public int stoneStored;

    public TextMeshProUGUI woodStoredText;
    public TextMeshProUGUI foodStoredText;
    public TextMeshProUGUI stoneStoredText;

    public float GiveFood()
    {
        if (foodStored >= 0)
        {
            foodStored -= (int)returnAmount;
            UpdateUi();
            return (int)returnAmount;
        }
        else
        {
            return 0;
        }
    }
    public float GiveWood()
    {
        if (woodStored >= 0)
        {
            woodStored -= (int)returnAmount;
            UpdateUi();
            return (int)returnAmount;
        }
        else
        {
            return 0;
        }
    }
    public float GiveStone()
    {
        if (stoneStored >= 0)
        {
            stoneStored -= (int)returnAmount;
            UpdateUi();
            return (int)returnAmount;
        }
        else
        {
            return 0;
        }
    }
    public void StoreFood(float addedAmount)
    {
        foodStored += (int)addedAmount;
        UpdateUi();
    }
    public void StoreWood(float addedAmount)
    {
        woodStored += (int)addedAmount;
        UpdateUi();
    }
    public void StoreStones(float addedAmount)
    {
        stoneStored += (int)addedAmount;
        UpdateUi();
    }

    void UpdateUi()
    {
        woodStoredText.text = woodStored.ToString();
        foodStoredText.text = foodStored.ToString();
        stoneStoredText.text = stoneStored.ToString();
    }
}
