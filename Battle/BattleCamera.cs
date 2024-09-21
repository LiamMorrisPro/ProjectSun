using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    public GameObject arenaCentre;
    public GameObject camPos1;
    public GameObject camPos2;
    public GameObject camPos3;



    public void activateBattleCam()
    {
        cam.Priority = 10;
    }

    public void deactivateBattleCam()
    {
        cam.Priority = 0;
    }

    public void FocusTarget(GameObject target, GameObject camPos)
    {
        cam.transform.position = camPos.transform.position;
        cam.LookAt = target.transform;
    }

    
}
