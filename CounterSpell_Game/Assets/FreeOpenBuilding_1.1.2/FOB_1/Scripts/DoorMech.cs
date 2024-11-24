using System.Collections;
using UnityEngine;

public class DoorMech : MonoBehaviour
{
    public Vector3 OpenRotation, CloseRotation; // Target rotations for open and closed states
    public float rotSpeed = 2f; // Speed of door rotation
    public Transform doorMesh; // Reference to the door's visual mesh

    [Header("Optional")]
    public bool useLocalRotation = true; // Toggle between local and world rotation
    public bool reverseDoorRotation = false; // Reverse the rotation direction if needed

    private bool doorBool = false; // Tracks if the door is open or closed
    private bool playerInTrigger = false; // Tracks if the player is in the trigger zone
    private Quaternion initialRotation; // Store the initial rotation

    void Start()
    {
        // If no door mesh is assigned, try to find it in children
        if (doorMesh == null)
        {
            // Try to find a child with "mesh" or "visual" in its name
            doorMesh = transform.Find("door_mesh");
            if (doorMesh == null) doorMesh = transform.Find("door_visual");
            if (doorMesh == null) doorMesh = transform.Find("Mesh");

            // If still not found, use this transform
            if (doorMesh == null)
            {
                doorMesh = transform;
                Debug.LogWarning("Door mesh not assigned and couldn't find it in children. Using this transform.");
            }
        }

        // Store initial rotation
        initialRotation = useLocalRotation ? doorMesh.localRotation : doorMesh.rotation;
    }

    void Update()
    {
        // Check for interaction input
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            doorBool = !doorBool; // Toggle door state
            Debug.Log($"Door state changed to: {(doorBool ? "Open" : "Closed")}");
        }

        // Calculate target rotation
        Vector3 targetEuler = doorBool ? OpenRotation : CloseRotation;
        if (reverseDoorRotation) targetEuler *= -1;

        // Create target rotation quaternion
        Quaternion targetRotation;
        if (useLocalRotation)
        {
            // Use local rotation relative to initial state
            targetRotation = initialRotation * Quaternion.Euler(targetEuler);
            doorMesh.localRotation = Quaternion.Lerp(
                doorMesh.localRotation,
                targetRotation,
                rotSpeed * Time.deltaTime
            );
        }
        else
        {
            // Use world rotation
            targetRotation = Quaternion.Euler(targetEuler);
            doorMesh.rotation = Quaternion.Lerp(
                doorMesh.rotation,
                targetRotation,
                rotSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInTrigger = true; // Player entered trigger zone
            Debug.Log("Player entered door trigger zone");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInTrigger = false; // Player exited trigger zone
            Debug.Log("Player exited door trigger zone");
        }
    }

    // Helper method to visualize the trigger zone in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}