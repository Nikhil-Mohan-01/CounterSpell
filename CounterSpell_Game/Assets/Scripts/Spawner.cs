using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Cube;
    private GameObject door;
    private bool isNearWindow;
    public Vector3 spawnPosition; 
    public int boards = 0;

    void Start()
    {
        spawnPosition = transform.position;    
    }


    void Update()
    {
        
        if (boards >= 0 && boards < 3 && (Input.GetKeyDown(KeyCode.E)) && isNearWindow)
        {
            SpawnBoardObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isNearWindow = true;
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