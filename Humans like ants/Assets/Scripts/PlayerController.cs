using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    public GameObject selectedHuman;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(cameraRay, out _hit))
            {
                if (selectedHuman != null)
                {
                    GiveHumanDestination(_hit.point);
                }
                else
                {
                    SelectHuman(_hit.point);
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (selectedHuman != null)
            {
                selectedHuman.GetComponent<HumanBehaviour>().RemovePlayerControl();
                cam.GetComponent<CameraController>().UnselectHuman();
                selectedHuman = null;
            }
        }
    }
    public void GiveHumanDestination(Vector3 pos)
    {
        selectedHuman.GetComponent<HumanBehaviour>().finding.GiveNewTarget(pos);
        selectedHuman.GetComponent<HumanBehaviour>().CheckState(8);
    }
    public void SelectHuman(Vector3 pos)
    {
        Collider[] col = Physics.OverlapSphere(pos, 10);
        float closest = Mathf.Infinity;
        for (int i = 0; i < col.Length; i++)
        {
            if(col[i].GetComponent<HumanBehaviour>())
            {
                float distance = Vector3.Distance(col[i].transform.position, pos);
                if (distance < closest)
                {
                    closest = distance;
                    selectedHuman = col[i].gameObject;
                    cam.GetComponent<CameraController>().humanInfo = selectedHuman.GetComponent<HumanBehaviour>();
                    cam.GetComponent<CameraController>().UpdateUi();
                }
            }
        }
    }
}
