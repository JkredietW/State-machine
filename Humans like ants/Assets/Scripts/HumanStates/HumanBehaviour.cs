using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Job
{
    hobo,
    woodCutter,
    miner,
    farmer,
    soldier,
}
public class HumanBehaviour : StateMachine
{
    //0=idle,1=walking,2=hungry,3=eating,4=timeToSleep,5=sleeping,6=goingForJob,7=isWorking8=playerControlled
    public bool isControlledByPlayer;
    public string nameOfHuman;
    public Job role;
    [Space]
    public float maxFoodValue;
    public float currentFoodValue;
    public float neededFoodValue;
    public float foodLoss;

    public float foodLossTime;
    [Space]
    public float maxWorkValue;
    public float currentWorkValue;
    public float addedWork;
    public float workLoss;
    [Space]
    public float woodCarried;
    public float foodCarried;
    public float stoneCarried;

    public float maxWoodCarried;
    public float maxFoodCarried;
    public float maxStoneCarried;


    //debug
    public bool debug;

    [HideInInspector] public bool hasCamera;


    //privates
    [HideInInspector]public Vector3 idleLocation;
    [HideInInspector]public PathFinding finding;
    [HideInInspector]public Grid grid;
    [HideInInspector]public SourceObject eatplace;
    [HideInInspector]public SleepLocation restplace;
    [HideInInspector]public GameManager gm;
    public bool hasDestination;
    public int lasteStateNumber;

    public GameObject lastState;

    private void Awake()
    {
        finding = GetComponent<PathFinding>();
        grid = FindObjectOfType<Grid>();
        gm = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        StartCoroutine(DecreaseFood());
        CheckState(0);
    }
    void SecAfterStart()
    {
        CheckState(0);
    }
    private void Update()
    {
        UpdateUi();
    }
    public void UpdateUi()
    {
        if (isControlledByPlayer)
        {
            Camera.main.GetComponent<CameraController>().UpdateUi();
        }
        else if(Camera.main.GetComponent<CameraController>().humanInfo == this)
        {
            Camera.main.GetComponent<CameraController>().UpdateUi();
        }
    }
    IEnumerator DecreaseFood()
    {
        currentFoodValue -= foodLoss;
        if (hasCamera)
        {
            Camera.main.GetComponent<CameraController>().UpdateUi();
        }
        if (currentFoodValue < neededFoodValue)
        {
            if(state)
            {
                if(state.stateTag != "food")
                {
                    CheckState(2);
                }
            }
        }
        if(currentFoodValue < 0)
        {
            //dead
            print(gameObject.name + " has died");
            Destroy(gameObject);
        }
        if (debug)
        {
            if (state)
            {
                print(state.stateTag);
            }
        }
        yield return new WaitForSeconds(foodLossTime);
        StartCoroutine(DecreaseFood());
    }
    public void CheckState(int returnToState)
    {
        Destroy(lastState);
        lastState = null;
        if (debug)
        {
            //print(returnToState);
        }
        if(isControlledByPlayer)
        {
            lastState = new GameObject("tempStateObjectplayer" + GetComponent<PathFinding>().humanId);
            lastState.AddComponent<State_Human_PlayerControlledMovement>();
            SetState(lastState.GetComponent<State_Human>(), this, "player");
            hasDestination = true;
            return;
        }

        if (returnToState == 0)
        {
            if(role != Job.hobo && currentWorkValue < maxWorkValue)
            {
                CheckState(6);
                return;
            }
            lastState = new GameObject("tempStateObjectIdle" + GetComponent<PathFinding>().humanId);
            lastState.AddComponent<State_Human_Idle>();
            SetState(lastState.GetComponent<State_Human>(), this, "idle");
        }
        else if(returnToState == 2)
        {
            lastState = new GameObject("tempStateObjectFood" + GetComponent<PathFinding>().humanId);
            lastState.AddComponent<State_Human_Hungry>();
            SetState(lastState.GetComponent<State_Human>(), this, "food");
        }
        else if(returnToState == 4)
        {
            lastState = new GameObject("tempStateObjectSleep" + GetComponent<PathFinding>().humanId);
            lastState.AddComponent<State_Human_Sleep>();
            SetState(lastState.GetComponent<State_Human>(), this, "sleep");
        }
        else if(returnToState == 5)
        {
            //sleeping
            if (gm.GetTime() == false)
            {
                idleLocation = RollTargetLocation();
                finding.GiveNewTarget(idleLocation);
            }
        }
        else if(returnToState == 6)
        {
            if(role == Job.hobo)
            {
                lastState = new GameObject("tempStateObjectIdle" + GetComponent<PathFinding>().humanId);
                lastState.AddComponent<State_Human_Idle>();
                SetState(lastState.GetComponent<State_Human>(), this, "idle");
            }
            else if(role == Job.woodCutter)
            {
                lastState = new GameObject("tempStateObjectWood" + GetComponent<PathFinding>().humanId);
                lastState.AddComponent<State_Human_WoodCutter>();
                SetState(lastState.GetComponent<State_Human>(), this, "wood");
            }
            else if (role == Job.miner)
            {
                lastState = new GameObject("tempStateObjectMine" + GetComponent<PathFinding>().humanId);
                lastState.AddComponent<State_Human_Miner>();
                SetState(lastState.GetComponent<State_Human>(), this, "mine");
            }
            else if (role == Job.farmer)
            {
                lastState = new GameObject("tempStateObjectFarm" + GetComponent<PathFinding>().humanId);
                lastState.AddComponent<State_Human_Farmer>();
                SetState(lastState.GetComponent<State_Human>(), this, "farm");
            }
        }
        else if(returnToState == 7)
        {

        }
        else if(returnToState == 8)
        {
            isControlledByPlayer = true;
            lastState = new GameObject("tempStateObjectplayer" + GetComponent<PathFinding>().humanId);
            lastState.AddComponent<State_Human_PlayerControlledMovement>();
            SetState(lastState.GetComponent<State_Human>(), this, "player");
            hasDestination = true;
            return;
        }
        lasteStateNumber = returnToState;
        hasDestination = true;
    }
    //still needs to be migrated
    IEnumerator GatherResource()
    {
        float distance = Vector3.Distance(transform.position, eatplace.transform.position);
        if (distance < 5)
        {
            float consumption = eatplace.GainResource();
            if (consumption == 0)
            {
                CheckState(0);
            }
            else
            {
                currentFoodValue += consumption;
                if (currentFoodValue > maxFoodValue * 0.9f)
                {
                    CheckState(0);
                }
            }
        }
        yield return new WaitForSeconds(1);
        if (currentWorkValue < maxWorkValue * 0.9f)
        {
            StartCoroutine(GatherResource());
        }
        else
        {
            CheckState(0);
        }
    }
    public void RemovePlayerControl()
    {
        isControlledByPlayer = false;
        CheckState(0);
    }
    public Vector3 RollTargetLocation()
    {
        Vector3 location = new Vector3(Random.Range(-49, 49), 0, Random.Range(-49, 49));
        Cell target = grid.CellFromWorldPosition(location);
        target.gridY += 1;
        if (target.isWall)
        {
            RollTargetLocation();
        }
        return location;
    }

    public void DestinationReached()
    {
        StartCoroutine(state.DestinationReached());
    }
    public void TimeChanged(bool isNight)
    {
        if (state.stateTag != "food")
        {
            if (isNight)
            {
                CheckState(4);
            }
            else
            {
                if(state.stateTag == "sleep")
                {
                    CheckState(0);
                }
            }
        }
    }
}
