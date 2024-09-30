using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;


public class LaserEnemyScript : MonoBehaviour
{
    public GameObject LightNing;
    public GameObject LaserLight;
    public GameObject Head;
    public GameObject Target;

    public float chargeTimeAudio;
    public float chargeTime;
    public float shootDelay;
    public float maxCharge;

    float tmpDelay;
    float nowFire;
    float charge;
    float randomTime;

    GameObject tmpLightNing;
    GameObject tmpLaserLight;

    LineRenderer lineRenderer;

    AudioSource au;
    Rigidbody rb;

    public AudioClip charge_sound;
    AudioClip charge_sound_final;
    public AudioClip sustain_sound;
    public AudioClip shoot_sound;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = Head.GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        if (Target == null)
        {
            Target = GameObject.Find("Player");
        }

        au = Head.GetComponent<AudioSource>();
        nowFire = 0f;
        tmpDelay = 0f;
        float[] samples = new float[charge_sound.samples * charge_sound.channels];
        //Debug.LogWarning(samples.Length);
        charge_sound.GetData(samples, 0);
        int length_of_sound = (int)(chargeTimeAudio * charge_sound.frequency * charge_sound.channels);
        charge_sound_final = AudioClip.Create(charge_sound.name, length_of_sound / charge_sound.channels, charge_sound.channels, charge_sound.frequency, false);
        float[] samples2 = new float[length_of_sound];
        for (int i = 0; i < samples2.Length; i++)
        {
            samples2[i] = samples[i + samples.Length - length_of_sound];
        }
        charge_sound_final.SetData(samples2, 0); //(int)(charge_sound.length * charge_sound.frequency) - length_of_sound
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit2;
        Physics.Raycast(Head.transform.position, Head.transform.forward, out hit2);
        //Debug.LogError(hit2.collider + " " + (hit2.collider.gameObject.transform.position == hit2.point));
        Vector3[] points = new Vector3[2];
        points.SetValue(Head.transform.position + Head.transform.forward *3.3f, 0);
        points.SetValue(hit2.point,1);
        lineRenderer.SetPositions(points);
        /*
        tmpLaserLight = Instantiate(LaserLight);
        Vector3 tvec = Head.transform.position + Head.transform.forward * 3f - hit2.collider.gameObject.transform.position;
        //tmpLaserLight.GetComponent<LightNingScript>().stretch = hit2.distance;
        tmpLaserLight.GetComponent<LightNingScript>().stretch = hit2.distance;
        tmpLaserLight.GetComponent<LightNingScript>().lifeTime = Time.deltaTime * 1.01f;
        tmpLaserLight.transform.position = (Head.transform.position + Head.transform.forward * 3f); //+ hit2.point) / 2f;
        tmpLaserLight.transform.LookAt(hit2.point);*/
        
        randomTime = Random.value/2f; // is in [0, 1]
        if (Time.timeScale > 0f && !rb.isKinematic)
        {
            au.UnPause();
            //Debug.Log("Diff: " + diff);
            if (nowFire < chargeTime)
            {
                nowFire += Time.deltaTime;
                Head.transform.LookAt(Target.transform);
                //Head.transform.Rotate(-Head.transform.right, blenderRotation);
                if (charge < maxCharge)
                {
                    charge += Time.deltaTime * maxCharge / chargeTimeAudio;
                    //ammo -= Time.deltaTime * chargeSpeed;
                    //Debug.Log("Charging " + charge);
                    if (!au.isPlaying)
                    {
                        au.loop = false;
                        au.clip = charge_sound_final;
                        au.Play();
                    }
                }
                else
                {

                    if (au.clip == charge_sound_final)
                    {
                        au.Stop();
                    }
                    if (!au.isPlaying)
                    {
                        au.loop = true;
                        au.clip = sustain_sound;
                        au.Play();
                    }
                }
                //Debug.Log("Charging" + nowFire);
            }
            else if (tmpDelay < shootDelay + randomTime)
            {
                //Debug.Log("Waiting" + tmpDelay);
                tmpDelay += Time.deltaTime;
            }
            else
            {
                //Debug.Log("Shooting");
                tmpDelay = 0f;
                au.loop = false;
                au.Stop();
                Shoot();
                charge = 0f;
                nowFire = 0f;
            }
        }
        else
        {
            nowFire = 0f;
            charge = 0f;
            tmpDelay = 0f;
            au.Pause();
        }
    }

    void Shoot()
    {
        //Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        //Vector3 shootAt = pickable.getAimTransform().forward * 100f;
        Rigidbody otherEntityRB = null;
        RaycastHit hit;
        Physics.Raycast(Head.transform.position, Head.transform.forward, out hit);
        if (hit.collider != null)
        {
            //shootAt = hit.point;
            otherEntityRB = hit.transform.gameObject.GetComponentInParent<Rigidbody>();
            if (otherEntityRB == null)
            {
                otherEntityRB = hit.transform.GetComponent<Rigidbody>();
            }
        }
        /*
        Rigidbody temp = Target.GetComponentInParent<Rigidbody>();
        if (temp == null)
        {
            temp = Target.GetComponent<Rigidbody>();
        }
        if (temp != null)
        {
            Debug.Log("Exploded on " + temp.gameObject.ToString() + " vec: " + Head.transform.forward);
            //PlayerController tmp_cont = Target.transform.gameObject.GetComponent<PlayerController>();
            /*if(tmp_cont != null)
            {
                tmp_cont.setPickableInfluence(pickable.getAimTransform().forward * charge);
            }
            temp.AddForce(Head.transform.forward * charge * temp.mass);
        }
        if (otherEntityRB != null && otherEntityRB != temp)
        {
            Debug.Log("Shot on " + otherEntityRB.gameObject.ToString());
            //otherEntityRB.AddExplosionForce(charge, shootAt, radius);
            otherEntityRB.AddForce(Head.transform.forward * charge * otherEntityRB.mass);
        }*/
        if (otherEntityRB != null)
        {
            Debug.Log("Shot on " + otherEntityRB.gameObject.ToString());
            //otherEntityRB.AddExplosionForce(charge, shootAt, radius);
            otherEntityRB.AddForce(Head.transform.forward * charge * otherEntityRB.mass);
        }
        /*else if(otherEntityRB == temp)
        {
            Debug.Log("Hit Player");
            Physics.Raycast(hit.point, pickable.getAimTransform().forward, out hit);
            if (hit.collider != null)
            {
                //shootAt = hit.point;
                otherEntityRB = hit.transform.gameObject.GetComponentInParent<Rigidbody>();
                if (otherEntityRB == null)
                {
                    otherEntityRB = hit.transform.GetComponent<Rigidbody>();
                }
                otherEntityRB.AddForce(pickable.getAimTransform().forward * charge * otherEntityRB.mass);
            }
        }*/

        tmpLightNing = Instantiate(LightNing);
        tmpLightNing.GetComponent<LightNingScript>().stretch = hit.distance/2f;
        tmpLightNing.transform.position = (Head.transform.position + Head.transform.forward*3f + hit.point) / 2f;
        tmpLightNing.transform.LookAt(hit.point);
        au.PlayOneShot(shoot_sound);

    }
}
