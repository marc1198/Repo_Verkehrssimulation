using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;

    private void Start()
    {
        Instantiate(SelectACarPrefab(), transform);
    }

    private GameObject SelectACarPrefab()                           // Zufallsauswahl für Autos
    {
        var RandomIndex = Random.Range(0, carPrefabs.Length);
        return carPrefabs[RandomIndex];
    }
}
