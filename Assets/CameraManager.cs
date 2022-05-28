using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject firstCamera;
    [SerializeField] private GameObject secondCamera;
    [SerializeField] private GameObject firstHUD;
    [SerializeField] private GameObject secondHUD;

    public void ChangeCamera()
    {
        if (firstCamera.activeSelf)
        {
            firstCamera.SetActive(false);
            secondCamera.SetActive(true);
            firstHUD.SetActive(false);
            secondHUD.SetActive(true);
        }
        else
        {
            firstCamera.SetActive(true);
            secondCamera.SetActive(false);
            firstHUD.SetActive(true);
            secondHUD.SetActive(false);
        }
    }
}
