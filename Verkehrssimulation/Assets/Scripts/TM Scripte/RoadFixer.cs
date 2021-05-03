using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    // Verschiedene Straßeausführungen die gesetzt werden können -> erweiterbar
    public GameObject deadEnd, roadStraight, corner, threeWay, fourWay;

    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int temporaryPosition)
    {
        //[left, up, right, down] -> Steuerung
        var result = placementManager.GetNeighbourTypesFor(temporaryPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();

        //Überprüfung wie viele Straßen an die neu gesetzte Straße anstoßen -> Erstellung einer Sackgasse wenn nu eine Straße verbunden
        if (roadCount == 0 || roadCount == 1) CreateDeadEnd(placementManager, result, temporaryPosition);
        else if (roadCount == 2)
        {
            if (CreateStraightRoad(placementManager, result, temporaryPosition)) return;
            CreateCorner(placementManager, result, temporaryPosition);
        }
        else if(roadCount == 3) CreateThreeWay(placementManager, result, temporaryPosition);
        else CreateFourWay(placementManager, result, temporaryPosition);

    }
    //Analog zu obendrüber Erstellung einer Kurve wenn z.B. oben und Links eine Straße vorhanden ist
    private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road && result[2] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, corner, Quaternion.Euler(0, 90, 0));
        else if (result[2] == CellType.Road && result[3] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, corner, Quaternion.Euler(0, 180, 0));
        else if (result[3] == CellType.Road && result[0] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, corner, Quaternion.Euler(0, 270, 0));
        else if (result[0] == CellType.Road && result[1] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, corner, Quaternion.Euler(0, 0, 0));
    }
    //T-Kreuzung erstellt wenn drei Straßen vorhanden sind
    private void CreateThreeWay(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if(result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road) 
            placementManager.modifyStructueModel(temporaryPosition, threeWay, Quaternion.Euler(0, 0, 0));
        else if(result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, threeWay, Quaternion.Euler(0, 90, 0));
        else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road) 
            placementManager.modifyStructueModel(temporaryPosition, threeWay, Quaternion.Euler(0, 270, 0));
    }
    //Kreuzung wird erstellt -> An allen kanten ist eine Straße vorhanden
    private void CreateFourWay(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        placementManager.modifyStructueModel(temporaryPosition, fourWay, Quaternion.Euler(0, 0, 0));
    }

    private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.modifyStructueModel(temporaryPosition, roadStraight, Quaternion.Euler(0, 0, 0));
            return true;
        }
        else if(result[1] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.modifyStructueModel(temporaryPosition, roadStraight, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }

    private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 270, 0));
        else if (result[2] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 0, 0));
        else if (result[3] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        else if (result[0] == CellType.Road)
            placementManager.modifyStructueModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 180, 0));
    }
}
