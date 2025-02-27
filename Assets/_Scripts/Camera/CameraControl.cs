using UnityEngine;
using Cinemachine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

public class CameraControl : SingletonMonoBehaviour<CameraControl>
{
    [SerializeField] private GameObject currentCamera;

    [SerializeField] private List<GameObject> availableCameras;
    
    public void ChangeCamera(GameObject newCamera)
    {
        if(newCamera == currentCamera)
        {
            return;
        }

        currentCamera = newCamera;
        
        foreach(GameObject camera in availableCameras)
        {
            if(camera != currentCamera)
                camera.SetActive(false);
        }

        currentCamera.SetActive(true);
    }
}
