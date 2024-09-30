using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBsounds : MonoBehaviour
{
    Rigidbody rb;
    AudioSource au;
    public AudioClip touch;
    public AudioSource pubau;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Debug.Log(pubau + " " + this.gameObject.ToString()) ;
        au = pubau;
        if(pubau == null)
        {
            //Debug.Log(pubau + " " + this.gameObject.ToString());
            au = GetComponent<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.LogError(au + " " + this.gameObject.ToString());
        au.PlayOneShot(touch);
    }
}
