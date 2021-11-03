using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CameraController : MonoBehaviour
{
    public Transform cam;
    public List<Transform> humans;
    int next = -1;
    bool atleastOne;

    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;

    //info tab
    public HumanBehaviour humanInfo;

    public TextMeshProUGUI stateText;
    public TextMeshProUGUI foodValueText;
    public TextMeshProUGUI workValueText;
    public TMP_Dropdown dropDownObject;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NextInLine();
            atleastOne = true;
        }
        if(atleastOne)
        {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            if (Input.GetMouseButtonDown(0))
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void NextInLine()
    {
        if(next > -1)
        {
            humans[next].GetComponent<MoveToDestination>().hasCamera = false;
            humans[next].GetComponent<HumanBehaviour>().hasCamera = false;
        }
        next++;
        if(next >= humans.Count)
        {
            next = 0;
        }
        cam.SetParent(humans[next]);
        humanInfo = humans[next].GetComponent<HumanBehaviour>();
        UpdateUi();
        cam.position = humans[next].position + Vector3.up * 2;
        humans[next].GetComponent<MoveToDestination>().hasCamera = true;
        humans[next].GetComponent<HumanBehaviour>().hasCamera = true;
    }
    public void UpdateUi()
    {
        if (humanInfo != null)
        {
            if (humanInfo.lastState != null)
            {
                stateText.text = humanInfo.lastState.GetComponent<State_Human>().stateTag;
            }
            foodValueText.text = humanInfo.currentFoodValue.ToString();
            workValueText.text = humanInfo.currentWorkValue.ToString();

            //job filter....
            switch (humanInfo.role)
            {
                case Job.hobo:
                    dropDownObject.value = 0;
                    break;
                case Job.woodCutter:
                    dropDownObject.value = 1;
                    break;
                case Job.miner:
                    dropDownObject.value = 0;
                    break;
            }
            dropDownObject.RefreshShownValue();
        }
    }
    public void UnselectHuman()
    {
        stateText.text = string.Empty;
        foodValueText.text = string.Empty;
        workValueText.text = string.Empty;

        dropDownObject.value = 0;
        dropDownObject.RefreshShownValue();
    }
    public void ChangeJobOfHuman()
    {
        if (humanInfo != null)
        {
            switch(dropDownObject.value)
            {
                case 0:
                    humanInfo.role = Job.hobo;
                        break;
                case 1:
                    humanInfo.role = Job.woodCutter;
                    break;
                case 2:
                    humanInfo.role = Job.miner;
                    break;
                case 3:
                    print(3);
                    break;
                case 4:
                    print(4);
                    break;
                case 5:
                    print(5);
                    break;
            }
        }
    }
}
