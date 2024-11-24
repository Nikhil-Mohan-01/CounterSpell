using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTrigger : MonoBehaviour
{
    public GameObject audioSourceObject; // Reference to the inactive GameObject with the AudioSource
    private AudioSource audioSource; // Cached AudioSource component

    private void Start()
    {
        if (audioSourceObject != null)
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource component found on the assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("Audio Source Object is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioSourceObject != null && audioSource != null)
        {
            // Temporarily activate the GameObject to play the audio
            audioSourceObject.SetActive(true);
            audioSource.Play();

            // Schedule to deactivate both the audio source and the trigger after the audio finishes playing
            StartCoroutine(DeactivateAfterAudio());
        }
    }

    private System.Collections.IEnumerator DeactivateAfterAudio()
    {
        // Wait for the audio to finish playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Deactivate the audio source GameObject
        audioSourceObject.SetActive(false);

        // Deactivate the trigger GameObject (this script's GameObject)
        gameObject.SetActive(false);
    }
}
