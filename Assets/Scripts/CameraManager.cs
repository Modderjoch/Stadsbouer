using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform[] cameraPositions;
    [SerializeField] private Camera _camera;

    public void NextPlayerCamera(int currentPlayer)
    {
        _camera.transform.SetParent(cameraPositions[currentPlayer], false);
    }
}
