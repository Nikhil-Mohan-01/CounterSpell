using System.Collections;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material daySkybox;  
    public Material nightSkybox;
    private bool isDay = true;  

    private float changeInterval = 10f; 
    private float timer = 0f; 

    void Update()
    {
        
        timer += Time.deltaTime;

        
        if (timer >= changeInterval)
        {
            ToggleSkybox();
            timer = 0f;
        }

        print(timer);
    }

    void ToggleSkybox()
    {
        if (daySkybox != null && nightSkybox != null)
        {
            
            RenderSettings.skybox = isDay ? nightSkybox : daySkybox;

            
            isDay = !isDay;

            
            DynamicGI.UpdateEnvironment();

            Debug.Log("Skybox changed to: " + (isDay ? "Day" : "Night"));
        }
        else
        {
            Debug.LogError("Skybox materials are not assigned in the Inspector.");
        }
    }
}
