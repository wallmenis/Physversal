using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsScript : MonoBehaviour
{
    //public float footstepTime;
    public AudioClip[] audioClips;
    public AudioClip bigFall;
    public AudioClip pickup;
    public AudioClip throwClip;
    public AudioClip air;
    public PauseRespawnMenu prm;
    public float airStartThress;
    public float timeToAir;
    public float fullairfalloff;

    float tmpToAir;
    float startVol;
    int prevClip;
    float diff_fall;
    float prev_fall;
    float tmpfafo;

    bool fullair;
    
    //AudioClip[] audioClipsFinal;
    AudioSource au;
    PlayerController pc;
    Rigidbody rb;
    PlayerInventory pi;
    // Start is called before the first frame update
    void Start()
    {
        fullair = false;
        tmpfafo = 0f;
        tmpToAir = 0f;
        diff_fall = 0f;
        prev_fall = 0f;
        prevClip = 0;
        //audioClipsFinal = new AudioClip[audioClips.Length];
        au = GetComponent<AudioSource>();
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        pi = GetComponent<PlayerInventory>();
        startVol = au.volume;

        /*for (int j = 0; j < audioClips.Length; j++)
        {
            float[] samples = new float[audioClips[j].samples * audioClips[j].channels];
            //Debug.LogWarning(samples.Length);
            audioClips[j].GetData(samples, 0);
            int length_of_sound = (int)(footstepTime * audioClips[j].frequency * audioClips[j].channels);
            audioClipsFinal[j] = AudioClip.Create(audioClips[j].name, length_of_sound / audioClips[j].channels, audioClips[j].channels, audioClips[j].frequency, false);
            float[] samples2 = new float[length_of_sound];
            for (int i = 0; i < samples2.Length; i++)
            {
                samples2[i] = samples[i];
            }
            audioClipsFinal[j].SetData(samples2, 0);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        diff_fall = rb.velocity.y - prev_fall;
        //Debug.Log(tmpfafo);
        if (fullair)
        {
            tmpfafo = fullairfalloff;
        }
        else
        {
            if (tmpfafo > 0f)
            {
                tmpfafo -= Time.deltaTime;
            }
            
        }
        //Debug.Log(diff_fall + " " + rb.velocity.y);
        /*if(fullair)
        {
            /*
            if (au.isPlaying && au.clip!=air)
            {
                //au.loop = false;
                au.Stop();
            }
            if(!au.isPlaying)
            {
                //au.loop = true;
                au.clip = air;
                au.Play();
            }
            tmpToAir = 1f;
        }*/

        if (!prm.getIsPaused())
        {
            au.UnPause();
            if (prev_fall < 0f * Time.deltaTime && rb.velocity.y == 0f)
            {
                if (!au.isPlaying)
                {
                    au.PlayOneShot(bigFall);
                }

            }
            if (pi.getItemListChanged())
            {
                if (pi.getHasPickedUpItem())
                {
                    au.PlayOneShot(pickup);
                }
                else
                {
                    au.PlayOneShot(throwClip);
                }
            }
            prev_fall = rb.velocity.y;

            if (pc.getIfCanJump() && pc.getMovementInput().magnitude > 0f && tmpfafo <=0f)
            {
                if (!au.isPlaying)
                {
                    int randomNum = 0;

                    if (Random.Range(0, 2) > 0)
                    {
                        randomNum = Random.Range(prevClip + 1, audioClips.Length);
                    }
                    else
                    {
                        randomNum = Random.Range(0, prevClip - 1);
                    }
                    if (prevClip == 0)
                    {
                        randomNum = Random.Range(1, audioClips.Length);
                    }
                    else if (prevClip == audioClips.Length)
                    {
                        randomNum = Random.Range(0, audioClips.Length - 1);
                    }
                    if (randomNum < 0)
                    {
                        randomNum = 0;
                    }
                    else if (randomNum >= audioClips.Length)
                    {
                        randomNum = audioClips.Length - 1;
                    }
                    prevClip = randomNum;
                    //https://docs.unity3d.com/ScriptReference/Random.Range.html
                    //au.clip = audioClipsFinal[Random.Range(0, audioClipsFinal.Length)];
                    au.clip = audioClips[randomNum];
                    au.Play();
                }
            }
            if (!pc.getIfCanJump() && -rb.velocity.y > airStartThress  || tmpfafo > 0f)
            {
                
                if (!au.isPlaying)
                {

                    au.clip = air;
                    au.Play();
                }
                else if (au.clip == air)
                {
                    if (tmpToAir / timeToAir < 1f)
                    {
                        tmpToAir += Time.deltaTime;
                    }
                    if(tmpfafo>0f)
                    {
                        tmpToAir = timeToAir + 0.01f;
                    }
                    Debug.Log(!pc.getIfCanJump());
                    au.volume = tmpToAir / timeToAir * startVol;
                }
                else
                {
                    tmpToAir = 0f;
                }
            }
            else
            {
                if (au.isPlaying && au.clip == air)
                {
                    au.Stop();
                    tmpToAir = 0f;
                }
            }
        }
        else
        {
            au.Pause();
        }



    }

    private void FixedUpdate()
    {
        
    }

    public void startFullAir()
    {
        fullair = true;
    }
    
    public void stopFullAir()
    {
        fullair = false;
    }
}
