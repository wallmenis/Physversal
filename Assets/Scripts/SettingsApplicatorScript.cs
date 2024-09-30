using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SettingsApplicatorScript : MonoBehaviour
{
    public float mainCameraFov;
    public float mainVolume;
    public bool fullscreen;
    public int qualityIndex;
    //https://docs.unity3d.com/ScriptReference/JsonUtility.html
    // Start is called before the first frame update
    void Start()
    {
        //https://discussions.unity.com/t/how-to-simply-check-if-a-file-exists-on-the-hard-drive/5165
        if (System.IO.File.Exists("physversal_settings.json"))
        {   
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText("physversal_settings.json"), this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplySettings(Camera mainCamera, AudioMixer mixer)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        mainCamera.fieldOfView = mainCameraFov;
        Screen.fullScreen = fullscreen;
        mixer.SetFloat("volume", (mainVolume - 100f)/2f);
        SaveSettings();
    }

    private void SaveSettings()
    {
        System.IO.File.WriteAllText("physversal_settings.json", JsonUtility.ToJson(this));
    }
}
