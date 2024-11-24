using System.Collections;
using UnityEngine;

public class WebCamPhotoCapture : MonoBehaviour
{
    public GameObject photoPrefab; 
    public float spawnDistance;
    public float spawnYOffset; 
    public Transform player; 
    private WebCamTexture webcamTexture;
    private Texture2D capturedPhoto;

    void Start()
    {
        
        if (WebCamTexture.devices.Length > 0)
        {
            webcamTexture = new WebCamTexture();
            webcamTexture.Play();
            Debug.Log("Webcam started!");
        }
        else
        {
            Debug.LogError("No webcam detected!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PhotoTrigger"))
        {
            TakePhoto();
        }
    }

    public void TakePhoto()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            StartCoroutine(CapturePhoto());
        }
        else
        {
            Debug.LogError("Webcam is not playing or not initialized.");
        }
    }

    private IEnumerator CapturePhoto()
    {
        
        yield return new WaitForEndOfFrame();

        if (webcamTexture.width > 16 && webcamTexture.height > 16) 
        {
            Debug.Log("Capturing photo...");

            
            capturedPhoto = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
            capturedPhoto.SetPixels(webcamTexture.GetPixels());
            capturedPhoto.Apply();

            Debug.Log("Photo captured!");

            
            SpawnPhotoObject();
        }
        else
        {
            Debug.LogError("Webcam data is not ready.");
        }
    }

    private void SpawnPhotoObject()
    {
        if (photoPrefab == null)
        {
            Debug.LogError("Photo Prefab is not assigned!");
            return;
        }

        
        Vector3 spawnPosition = player.position + player.forward * spawnDistance + Vector3.up * spawnYOffset;

        
        GameObject spawnedPhoto = Instantiate(photoPrefab, spawnPosition, Quaternion.LookRotation(-player.forward));

        
        Renderer renderer = spawnedPhoto.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = capturedPhoto;
            Debug.Log("Photo applied to spawned object!");
        }
        else
        {
            Debug.LogError("Spawned object does not have a Renderer!");
        }
    }

    private void OnApplicationQuit()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }
}
