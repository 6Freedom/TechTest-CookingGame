using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject foodPrefab;
    public Transform anchor;
    public bool SpawnOnLaunch = false;

    private void Start()
    {
        if (SpawnOnLaunch) Spawn();
        SpawnOnLaunch = false;
    }
    
    //Fais apparaître la nourriture liée
    public void Spawn()
    {
        Debug.Log("Spawn food!");
        var go = Instantiate(foodPrefab);
        go.transform.position = anchor.transform.position;
    }
}
