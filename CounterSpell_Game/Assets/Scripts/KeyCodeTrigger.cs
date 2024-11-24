using UnityEngine;

public class KeyCodeTrigger : MonoBehaviour
{
    public GameObject audioSourceObject; // Reference to the inactive GameObject with AudioSource
    private AudioSource audioSource; // Cached AudioSource component

    private void Start()
    {
        if (audioSourceObject != null)
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing on the assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("AudioSourceObject is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioSourceObject != null && audioSource != null)
        {
            // Temporarily activate the audio source GameObject
            audioSourceObject.SetActive(true);

            // Play the audio clip
            audioSource.Play();

            // Deactivate the audio source after the clip finishes
            StartCoroutine(DeactivateAudioSourceAfterPlayback());
        }
    }

    private System.Collections.IEnumerator DeactivateAudioSourceAfterPlayback()
    {
        // Wait for the audio clip to finish playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Deactivate the audio source GameObject
        audioSourceObject.SetActive(false);
    }
}
