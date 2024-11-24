using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Cube; 
    public Vector3 spawnPosition = new Vector3(0, 0, 0); 
    public int boards = 0;


    void Update()
    {
        
        if (boards >= 0 && boards < 3 && (Input.GetKeyDown(KeyCode.E)))
        {
            SpawnBoardObject();
        }
    }

    void SpawnBoardObject()
    {
        if (boards <= 3)
        {
            
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            
            Instantiate(Cube, spawnPosition, randomRotation);
            boards += 1;
        }
    }
}