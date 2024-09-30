using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public SettingsApplicatorScript sas;
    public Camera mc;
    public AudioMixer am;
    public Slider fovSlider;
    public Slider volSlider;
    public TMP_Dropdown dt;
    public TextMeshProUGUI fovText;
    public TextMeshProUGUI volText;
    public GameObject PreviousMenu;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SAS: " + sas);
        //sas = GetComponent<SettingsApplicatorScript>();
        fovSlider.value = sas.mainCameraFov;
        volSlider.value = sas.mainVolume;
        fovText.text = sas.mainCameraFov.ToString();
        volText.text = sas.mainVolume.ToString();
        dt.SetValueWithoutNotify(sas.qualityIndex);
        //fovText.text = sas.mainCameraFov.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFov(float fov)
    {
        if (sas != null)
        {
            sas.mainCameraFov = fov;
            Debug.Log(fov);
            fovText.text = sas.mainCameraFov.ToString();
        }
    }

    public void SetQalitySettings(int qindex)
    {
        if (sas != null)
        {
            sas.qualityIndex = qindex;
        }   
    }

    public void SetVolume(float volume)
    {
        if (sas != null)
        {
            sas.mainVolume = volume;
            volText.text = sas.mainVolume.ToString();
        }

    }

    public void SetFullscreen(bool fulsc)
    {
        if (sas != null)
        {
            sas.fullscreen = fulsc;
        } 
    }

    public void Apply()
    {
        if (sas != null)
        {
            sas.ApplySettings(mc, am);
        } 
    }

    public void GoBack()
    {
        PreviousMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
