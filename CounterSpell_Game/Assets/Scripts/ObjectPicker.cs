using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public float pickUpRange = 3.0f; 
    public float holdDistance = 2.0f; 
    public Transform player; 
    public Transform orientation; 
    private GameObject pickedObject; 
    private Vector3 holdOffset; 
    private Rigidbody pickedRigidbody;

    void Update()
    {
        
        UpdateHoldPoint();

        
        if (Input.GetMouseButtonDown(0))
        {
            if (pickedObject == null)
            {
                TryPickUpObject(); 
            }
            else
            {
                DropObject(); 
            }
        }
    }

    void UpdateHoldPoint()
    {
        
        float yRotation = orientation.eulerAngles.y;
        float radians = yRotation * Mathf.Deg2Rad;

        
        holdOffset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * holdDistance;

        
        if (pickedObject != null)
        {
            Vector3 targetPosition = player.position + holdOffset;

            
            pickedObject.transform.position = Vector3.Lerp(
                pickedObject.transform.position,
                targetPosition,
                Time.deltaTime * 10f
            );

            
            pickedObject.transform.rotation = Quaternion.Lerp(
                pickedObject.transform.rotation,
                Quaternion.LookRotation(holdOffset),
                Time.deltaTime * 10f
            );
        }
    }

    void TryPickUpObject()
    {
        
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Pickable") || hit.collider.CompareTag("Keys"))
            {
                pickedObject = hit.collider.gameObject;
                pickedRigidbody = pickedObject.GetComponent<Rigidbody>();

                if (pickedRigidbody != null)
                {
                    pickedRigidbody.isKinematic = true;
                    pickedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                }

                
                Key key = pickedObject.GetComponent<Key>();
                if (key != null)
                {
                    key.picked = true;
                }

                Debug.Log($"Picked up: {pickedObject.name}");
            }
        }
    }

    void DropObject()
    {
        if (pickedObject != null)
        {
          
            Key key = pickedObject.GetComponent<Key>();
            if (key != null)
            {
                key.picked = false;
            }

            
            if (pickedRigidbody != null)
            {
                pickedRigidbody.isKinematic = false;
            }

            
            pickedObject = null;
            pickedRigidbody = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position + holdOffset, 0.1f);
        }
    }
}