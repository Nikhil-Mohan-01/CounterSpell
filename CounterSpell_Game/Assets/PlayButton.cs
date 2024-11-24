using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Button clicked! Loading Game...");
        SceneManager.LoadScene("Scenes/Game"); // Replace with the exact scene name
    }
}
