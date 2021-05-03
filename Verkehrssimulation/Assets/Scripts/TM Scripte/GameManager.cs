using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Schnittstelle von Simulation zum Bediener (Eingabehandling)
public class GameManager : MonoBehaviour
{
    public RoadManager roadmanager;
    public CameraMovement cameraMovement;
    public InputManager inputManager;

    private void Start()
    {
        inputManager.OnMouseClick += roadmanager.PlaceRoad;
        inputManager.OnMouseHold += roadmanager.PlaceRoad;
        inputManager.OnMouseUp += roadmanager.FinishPlacingRoad;
    }

    private void HandleMouseClick(Vector3Int position)
    {
        roadmanager.PlaceRoad(position);
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
    }
}
