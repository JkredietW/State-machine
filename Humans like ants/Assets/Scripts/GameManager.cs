using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool debug;

    public HumanBehaviour[] AllHumans;

    public bool isNight;

    void Start()
    {
        AllHumans = FindObjectsOfType<HumanBehaviour>();
    }
    public void GiveTimeToHumans()
    {
        isNight = !isNight;
        if (debug)
        {
            print(isNight);
        }

        foreach (HumanBehaviour item in AllHumans)
        {
            item.TimeChanged(isNight);
        }
    }
    public bool GetTime()
    {
        return isNight;
    }
}
