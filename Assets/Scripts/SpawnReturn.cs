using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnReturn : MonoBehaviour
{
    public FootstepsScript fs;
    public PauseRespawnMenu prm;
    AudioSource au;
    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (prm.getIsPaused())
        {
            au.Pause();
        }
        else
        {
            au.UnPause(); 
        }
        fs.startFullAir();
        if (!au.isPlaying && !prm.getIsPaused())
        {
            fs.stopFullAir();
            transform.gameObject.SetActive(false);
        }
    }
}
