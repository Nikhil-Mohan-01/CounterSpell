using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTrigger : MonoBehaviour
{
    public WebCamPhotoCapture photoCapture; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            photoCapture.TakePhoto(); 
            Debug.Log("Trigger activated! Photo taken.");
        }
    }
}
