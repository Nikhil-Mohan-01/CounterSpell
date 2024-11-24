using System.Collections;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material daySkybox;
    public Material nightSkybox;
    private bool isDay = true;
    private bool hasSwitchedToNight = false; // Tracks if it has switched to night

    private float changeInterval = 30f; // Change interval (day/night cycle)
    private float timer = 0f;

    public GameObject key; // Reference to the key GameObject
    public Vector3 nightKeyPosition; // The position to move the key to at night
    public Vector3 dayKeyPosition; // The position to move the key to during the day

    public GameObject door; // Reference to the door GameObject

    public GameObject audioSourceObject1; // Reference to inactive GameObject with first AudioSource
    public GameObject audioSourceObject2; // Reference to inactive GameObject with second AudioSource
    public GameObject audioSourceObject3; // Reference to inactive GameObject with third AudioSource
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;

    public GameObject keyCodeTrigger; // Reference to the inactive KeyCodeTrigger GameObject

    private void Start()
    {
        // Get AudioSource components
        if (audioSourceObject1 != null)
        {
            audioSource1 = audioSourceObject1.GetComponent<AudioSource>();
            if (audioSource1 == null)
            {
                Debug.LogError("AudioSource component missing on the first assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("AudioSourceObject1 is not assigned.");
        }

        if (audioSourceObject2 != null)
        {
            audioSource2 = audioSourceObject2.GetComponent<AudioSource>();
            if (audioSource2 == null)
            {
                Debug.LogError("AudioSource component missing on the second assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("AudioSourceObject2 is not assigned.");
        }

        if (audioSourceObject3 != null)
        {
            audioSource3 = audioSourceObject3.GetComponent<AudioSource>();
            if (audioSource3 == null)
            {
                Debug.LogError("AudioSource component missing on the third assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("AudioSourceObject3 is not assigned.");
        }

        // Ensure the KeyCodeTrigger starts inactive
        if (keyCodeTrigger != null)
        {
            keyCodeTrigger.SetActive(false);
        }

        door.SetActive(false); // Ensure the door starts inactive
    }

    void Update()
    {
        // Increment the timer
        if (!hasSwitchedToNight) // Only update the timer if night hasn't occurred
        {
            timer += Time.deltaTime;

            // Check if the interval has passed
            if (timer >= changeInterval)
            {
                ToggleSkybox();
                timer = 0f; // Reset the timer
            }
        }
    }

    void ToggleSkybox()
    {
        if (daySkybox != null && nightSkybox != null)
        {
            // If already switched to night, do nothing
            if (!isDay && hasSwitchedToNight)
            {
                return;
            }

            // Switch between day and night skyboxes
            RenderSettings.skybox = isDay ? nightSkybox : daySkybox;

            // Update the "isDay" state
            isDay = !isDay;

            // Handle key, door, and KeyCodeTrigger states based on time of day
            if (!isDay) // Nighttime
            {
                MoveKeyToNightPosition();
                SetDoorState(true); // Activate the door at night
                PlayNightAudio(); // Play audio for nighttime
                ActivateKeyCodeTrigger(); // Activate KeyCodeTrigger
                hasSwitchedToNight = true; // Mark as switched to night
            }
            else // Daytime
            {
                MoveKeyToDayPosition();
                SetDoorState(false); // Deactivate the door during the day
                DeactivateKeyCodeTrigger(); // Deactivate KeyCodeTrigger
            }

            // Trigger global illumination update
            DynamicGI.UpdateEnvironment();

            Debug.Log("Skybox changed to: " + (isDay ? "Day" : "Night"));
        }
        else
        {
            Debug.LogError("Skybox materials are not assigned in the Inspector.");
        }
    }

    void MoveKeyToNightPosition()
    {
        if (key != null)
        {
            key.transform.position = nightKeyPosition;
            Debug.Log("Key moved to nighttime position: " + nightKeyPosition);
        }
    }

    void MoveKeyToDayPosition()
    {
        if (key != null)
        {
            key.transform.position = dayKeyPosition;
            Debug.Log("Key moved to daytime position: " + dayKeyPosition);
        }
    }

    void SetDoorState(bool state)
    {
        if (door != null)
        {
            door.SetActive(state);
            Debug.Log("Door is now " + (state ? "active (nighttime)" : "inactive (daytime)"));
        }
    }

    void PlayNightAudio()
    {
        if (audioSourceObject1 != null && audioSource1 != null)
        {
            // Temporarily activate the first audio source GameObject
            audioSourceObject1.SetActive(true);

            // Play the first audio clip
            audioSource1.Play();

            // After the first audio finishes, play the second audio
            StartCoroutine(PlaySecondNightAudio());
        }
    }

    private System.Collections.IEnumerator PlaySecondNightAudio()
    {
        // Wait for the first audio clip to finish
        yield return new WaitForSeconds(audioSource1.clip.length);

        // Deactivate the first audio source GameObject
        audioSourceObject1.SetActive(false);

        if (audioSourceObject2 != null && audioSource2 != null)
        {
            // Temporarily activate the second audio source GameObject
            audioSourceObject2.SetActive(true);

            // Play the second audio clip
            audioSource2.Play();

            // After the second audio finishes, play the third audio
            StartCoroutine(PlayThirdNightAudio());
        }
    }

    private System.Collections.IEnumerator PlayThirdNightAudio()
    {
        // Wait for the second audio clip to finish
        yield return new WaitForSeconds(audioSource2.clip.length);

        // Deactivate the second audio source GameObject
        audioSourceObject2.SetActive(false);

        if (audioSourceObject3 != null && audioSource3 != null)
        {
            // Temporarily activate the third audio source GameObject
            audioSourceObject3.SetActive(true);

            // Play the third audio clip
            audioSource3.Play();

            // Deactivate the third audio source after playback
            yield return new WaitForSeconds(audioSource3.clip.length);
            audioSourceObject3.SetActive(false);
        }
    }

    void ActivateKeyCodeTrigger()
    {
        if (keyCodeTrigger != null)
        {
            keyCodeTrigger.SetActive(true);
            Debug.Log("KeyCodeTrigger activated at nighttime.");
        }
    }

    void DeactivateKeyCodeTrigger()
    {
        if (keyCodeTrigger != null)
        {
            keyCodeTrigger.SetActive(false);
            Debug.Log("KeyCodeTrigger deactivated during daytime.");
        }
    }
}
