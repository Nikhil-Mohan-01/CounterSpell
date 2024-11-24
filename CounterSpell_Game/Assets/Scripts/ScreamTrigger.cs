using System.Collections;
using UnityEngine;

public class ScreamTrigger : MonoBehaviour
{
    public GameObject screamTriggerObject; // Reference to the inactive GameObject with the AudioSource
    private AudioSource audioSource; // Cached AudioSource component

    private void Start()
    {
        if (screamTriggerObject != null)
        {
            audioSource = screamTriggerObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing on the ScreamTrigger GameObject.");
            }
        }
        else
        {
            Debug.LogError("ScreamTriggerObject is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && screamTriggerObject != null && audioSource != null)
        {
            // Temporarily activate the scream trigger GameObject
            screamTriggerObject.SetActive(true);

            // Play the scream audio
            audioSource.Play();

            // Deactivate the scream trigger GameObject after the audio clip finishes
            StartCoroutine(DeactivateScreamTriggerAfterPlayback());
        }
    }

    private IEnumerator DeactivateScreamTriggerAfterPlayback()
    {
        // Wait for the audio clip to finish
        yield return new WaitForSeconds(audioSource.clip.length);

        // Deactivate the scream trigger GameObject
        screamTriggerObject.SetActive(false);
    }
}
