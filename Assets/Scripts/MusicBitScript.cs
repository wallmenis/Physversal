using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBitScript : MonoBehaviour
{

    public AudioSource MusicBox;
    public AudioClip Track;
    bool played;
    // Start is called before the first frame update
    void Start()
    {
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!MusicBox.isPlaying)
        {
            if(!played)
            {
                played = true;
                MusicBox.clip = Track;
                MusicBox.Play();
            }
            else
            {
                transform.gameObject.SetActive(false);
            }

        }
    }
}
