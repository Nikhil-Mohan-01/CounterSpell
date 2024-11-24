using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool picked; 
    private GameObject door; 
    private bool isNearDoor; 

    void Start()
    {
        picked = false; 
        door = null; 
        isNearDoor = false;
    }

    void Update()
    {
        
        if (picked && isNearDoor && Input.GetKeyDown(KeyCode.F))
        {
            UnlockDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (picked && other.CompareTag("Door"))
        {
            door = other.gameObject;
            isNearDoor = true;
            Debug.Log("Key is near the door. Press 'F' to unlock.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Door") && other.gameObject == door)
        {
            door = null;
            isNearDoor = false;
        }
    }

    private void UnlockDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
            Debug.Log("Door unlocked!");

            
            Destroy(gameObject);
        }
    }
}