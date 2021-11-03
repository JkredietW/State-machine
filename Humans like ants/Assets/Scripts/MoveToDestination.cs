using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDestination : MonoBehaviour
{
    public LayerMask hitLayers;
    Grid gridReference;
    public float movementSpeed;

    int humadId;

    public bool hasCamera;
    public LineRenderer line;
    public GameObject lineprefab;

    int nextCellId;
    bool justOnce;

    private HumanBehaviour human;
    private void Awake()
    {
        gridReference = FindObjectOfType<Grid>();
        line = Instantiate(lineprefab, transform).GetComponent<LineRenderer>();
        human = GetComponent<HumanBehaviour>();
    }
    private void Start()
    {
        humadId = GetComponent<PathFinding>().humanId;
    }

    public void MoveToNextCell()
    {
        if(gridReference.pathList[humadId].finalPath != null)
        {
            int maxCount = gridReference.pathList[humadId].finalPath.Count - 1;
            nextCellId = maxCount;
        }
    }
    private void Update()
    {
        if (human.hasDestination)
        {
            //move
            if (gridReference.pathList[humadId].finalPath != null)
            {
                if(nextCellId > gridReference.pathList[humadId].finalPath.Count - 1)
                {
                    int maxCount = gridReference.pathList[humadId].finalPath.Count - 1;
                    nextCellId = maxCount;
                }
                if (nextCellId > -1)
                {
                    if(justOnce == false)
                    {
                        justOnce = true;
                        int maxCount = gridReference.pathList[humadId].finalPath.Count - 1;
                        nextCellId = maxCount;
                    }
                    float distanceToNextCell = Vector3.Distance(transform.position, gridReference.pathList[humadId].finalPath[nextCellId].position);
                    if (distanceToNextCell < 0.5f)
                    {
                        nextCellId--;
                    }
                }
                if (nextCellId >= 0)
                {
                    Vector3 moveDir = gridReference.pathList[humadId].finalPath[nextCellId].position - transform.position;
                    transform.Translate(movementSpeed * Time.deltaTime * moveDir.normalized);
                    if (hasCamera)
                    {
                        line.SetPosition(1, moveDir.normalized * 2);
                    }
                    else
                    {
                        line.SetPosition(1, Vector3.zero);
                    }
                }
                if (gridReference.pathList[humadId].finalPath.Count >= 0)
                {
                    float distance = Vector3.Distance(transform.position, gridReference.pathList[humadId].finalPath[0].position);
                    if (distance <= 0.5f)
                    {
                        if (human.hasDestination)
                        {
                            human.hasDestination = false;
                            human.DestinationReached();
                        }
                    }
                }
                else if (human.hasDestination)
                {
                    human.hasDestination = false;
                    human.DestinationReached();
                }
            }
        }
    }
}
