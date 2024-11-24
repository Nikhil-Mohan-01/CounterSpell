using UnityEngine;

public class Keypad : MonoBehaviour
{

    public string password = "1234";
    private string userInput = "";

    public GameObject mainRDoor;
    public GameObject mainLDoor;

    private void Start()
    {
        userInput = "";
    }

    public void ButtonClicked(string number)
    {
        userInput += number;
        
        if (userInput.Length >= 4)
        {
            if (userInput == password)
            {
                //boolean for door = true
                Debug.LogWarning("Unlocked");
                Destroy(mainRDoor);
                Destroy(mainLDoor);
            } else
            {
                //boolean for door stays false
                Debug.LogWarning("Wrong Password");
                userInput = "";
            }
        }
    }
}
