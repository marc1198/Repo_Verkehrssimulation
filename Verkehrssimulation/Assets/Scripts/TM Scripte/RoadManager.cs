using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public RoadFixer roadFixer;

    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPositionsToRecheck = new List<Vector3Int>();

    private Vector3Int startPosition;
    private bool placementMode = false;

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }

    // Erstellung von Straßenzellen
    public void PlaceRoad(Vector3Int position)
    {
        if (!placementManager.CheckIfPositionInBound(position)) return;
        if (!placementManager.CheckIfPositionIsFree(position)) return;
        if(placementMode == false)
        {
            temporaryPlacementPositions.Clear();
            roadPositionsToRecheck.Clear();

            placementMode = true;
            startPosition = position;

            temporaryPlacementPositions.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.deadEnd, CellType.Road); // Base-Prefab
            
        }
        else
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPositions.Clear();

            foreach (var positionsToFix in roadPositionsToRecheck)
                roadFixer.FixRoadAtPosition(placementManager, positionsToFix);

            roadPositionsToRecheck.Clear();

            temporaryPlacementPositions = placementManager.GetPathBetween(startPosition,position);

            foreach(var temporaryPosition in temporaryPlacementPositions)
            {
                if (!placementManager.CheckIfPositionIsFree(temporaryPosition)) continue;
                placementManager.PlaceTemporaryStructure(temporaryPosition, roadFixer.deadEnd, CellType.Road);
            }
        }

        FixRoadPrefabs();

    }

    // Abstimmung aller Straßentexturen auf ihre Nachbarzellen
    private void FixRoadPrefabs()
    {
        foreach(var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighboursOfTypeFor(temporaryPosition, CellType.Road);
            foreach (var roadposition in neighbours)
                if (roadPositionsToRecheck.Contains(roadposition) == false)
                    roadPositionsToRecheck.Add(roadposition);
        }

        foreach (var positionToFix in roadPositionsToRecheck)
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
    }

    // Wandlung des von A* berechneten Straßenzuges in feste Straßenzellen
    public void FinishPlacingRoad()
    {
        placementMode = false;
        placementManager.AddtemporaryStructuresToStructureDictionary();
        temporaryPlacementPositions.Clear();
        startPosition = Vector3Int.zero;
    }
}
