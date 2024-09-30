using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SettingsSetter : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mc;
    public AudioMixer au;
    public SettingsApplicatorScript sas;
    void Start()
    {
        sas = GetComponent<SettingsApplicatorScript>();
        sas.ApplySettings(mc, au);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
