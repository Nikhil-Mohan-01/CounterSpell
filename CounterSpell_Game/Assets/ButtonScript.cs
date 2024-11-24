using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public int keypadNumber;

    public UnityEvent KeypadClicked;



    private void OnMouseDown()
    {
        KeypadClicked.Invoke();
    }
}
