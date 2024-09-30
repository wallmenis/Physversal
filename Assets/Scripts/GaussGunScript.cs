using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussGunScript : MonoBehaviour
{
    Pickable pickable;
    float diff;
    float prevFire;
    float nowFire;
    float charge;
    public float maxCharge;
    public float chargeTime;
    //public float radius;
    public float ammo;
    public float holderRecoilPercentage;
    public Transform ammoBox;
    public Transform chargeBox;
    public Transform ammoBoxBackdrop;
    public Transform chargeBoxBackdrop;

    public GameObject LightNing;
    GameObject tmpLightNing;

    AudioSource au;
    public AudioSource pewau;
    public AudioClip charge_sound;
    AudioClip charge_sound_final;
    public AudioClip start_sound;
    public AudioClip sustain_sound;
    public AudioClip shoot_sound;

    float startAmmo;
    Vector3 realAmmoBoxScale;
    Vector3 realAmmoBoxPosition;
    Material realAmmoBoxMaterial;
    Color realAmmoBoxEmissionColor;
    Color realAmmoBoxBaseColor;

    Vector3 realChargeBoxScale;
    Vector3 realChargeBoxPosition;
    Material realChargeBoxMaterial;
    Color realChargeBoxEmissionColor;
    Color realChargeBoxBaseColor;
    float generalTMP;
    // Start is called before the first frame update
    void Start()
    {
        //maxCharge = 10000f;
        //chargeSpeed = 10000f;
        //radius = 100f;
        diff = 0;
        prevFire = 0;
        nowFire = 0;
        charge = 0;
        pickable = GetComponent<Pickable>();

        au = GetComponent<AudioSource>();


        realAmmoBoxScale = ammoBox.localScale;
        realAmmoBoxPosition = ammoBox.localPosition;
        realAmmoBoxMaterial = new Material(ammoBox.gameObject.GetComponent<MeshRenderer>().material);
        ammoBox.gameObject.GetComponent<MeshRenderer>().material = realAmmoBoxMaterial;
        realAmmoBoxEmissionColor = realAmmoBoxMaterial.GetColor("_EmissionColor");
        realAmmoBoxBaseColor = realAmmoBoxMaterial.GetColor("_BaseColor");

        realChargeBoxScale = chargeBox.localScale;
        realChargeBoxPosition = chargeBox.localPosition;
        realChargeBoxMaterial = new Material(chargeBox.gameObject.GetComponent<MeshRenderer>().material);
        chargeBox.gameObject.GetComponent<MeshRenderer>().material = realChargeBoxMaterial;
        realChargeBoxEmissionColor = realChargeBoxMaterial.GetColor("_EmissionColor");
        realChargeBoxBaseColor = realChargeBoxMaterial.GetColor("_BaseColor");


        startAmmo = ammo;
        if (startAmmo <=0f)
        {
            startAmmo = 1f;
        }
        if (maxCharge <=0f)
        {
            maxCharge = 1f;
        }

        //https://docs.unity3d.com/ScriptReference/AudioClip.GetData.html
        //https://docs.unity3d.com/2021.3/Documentation/ScriptReference/AudioClip.Create.html

        float[] samples = new float[charge_sound.samples * charge_sound.channels];
        //Debug.LogWarning(samples.Length);
        charge_sound.GetData(samples, 0);
        int length_of_sound = (int) (chargeTime * charge_sound.frequency * charge_sound.channels);
        charge_sound_final = AudioClip.Create(charge_sound.name, length_of_sound/ charge_sound.channels, charge_sound.channels, charge_sound.frequency, false);
        float[] samples2 = new float[length_of_sound];
        for (int i = 0; i<samples2.Length; i++)
        {
            samples2[i] = samples[i + samples.Length - length_of_sound];
        }
        charge_sound_final.SetData(samples2, 0); //(int)(charge_sound.length * charge_sound.frequency) - length_of_sound
    }

    // Update is called once per frame
    void Update()
    {
        if (pickable.getPickerTransform() == null || pickable.isHidden())
        {
            ammoBox.gameObject.SetActive(false);
            ammoBoxBackdrop.gameObject.SetActive(false);
            chargeBox.gameObject.SetActive(false);
            chargeBoxBackdrop.gameObject.SetActive(false);
        }
        else
        {
            ammoBox.gameObject.SetActive(true);
            ammoBoxBackdrop.gameObject.SetActive(true);
            chargeBox.gameObject.SetActive(true);
            chargeBoxBackdrop.gameObject.SetActive(true);

            ammoBox.localScale = new Vector3(realAmmoBoxScale.x * ammo/startAmmo, realAmmoBoxScale.y, realAmmoBoxScale.z);
            chargeBox.localScale = new Vector3(realChargeBoxScale.x * charge / maxCharge, realChargeBoxScale.y, realChargeBoxScale.z);

            ammoBox.localPosition = new Vector3(realAmmoBoxPosition.x  - 0.17f*(1f -ammo/startAmmo), realAmmoBoxPosition.y, realAmmoBoxPosition.z + 0.171f * (1f - ammo / startAmmo));
            chargeBox.localPosition = new Vector3(realChargeBoxPosition.x  - 0.17f * (1f - charge/maxCharge), realChargeBoxPosition.y, realChargeBoxPosition.z + 0.171f * (1f - charge / maxCharge));


            realAmmoBoxMaterial.SetColor("_EmissionColor", new Color(realAmmoBoxEmissionColor.r ,realAmmoBoxEmissionColor.g * ammo / startAmmo, realAmmoBoxEmissionColor.b * ammo / startAmmo * ammo / startAmmo * ammo / startAmmo));
            realAmmoBoxMaterial.SetColor("_BaseColor", new Color(realAmmoBoxBaseColor.r , realAmmoBoxBaseColor.g * ammo / startAmmo, realAmmoBoxBaseColor.b * ammo / startAmmo * ammo / startAmmo * ammo / startAmmo));
            realChargeBoxMaterial.SetColor("_EmissionColor", new Color(realChargeBoxEmissionColor.r,realChargeBoxEmissionColor.g * (1f - charge / maxCharge), realChargeBoxEmissionColor.b * (1f - charge / maxCharge) * (1f - charge / maxCharge) * (1f - charge / maxCharge)));
            realChargeBoxMaterial.SetColor("_BaseColor", new Color(realChargeBoxBaseColor.r, realChargeBoxBaseColor.g * (1f - charge / maxCharge), realChargeBoxBaseColor.b * (1f - charge / maxCharge) * (1f - charge / maxCharge) * (1f - charge / maxCharge)));
        }

        prevFire = nowFire;
        nowFire = pickable.getPrimaryFire();
        diff = nowFire - prevFire;
        //Debug.Log("Diff: " + diff);
        if (nowFire>0f && ammo > 0f)
        {
            generalTMP = maxCharge;
            if(maxCharge > ammo)
            {
                generalTMP = ammo;
            }
            if (charge < generalTMP)
            {
                charge += Time.deltaTime * maxCharge/chargeTime;
                //ammo -= Time.deltaTime * chargeSpeed;
                //Debug.Log("Charging " + charge);
                if (au.clip == start_sound)
                {
                    au.Stop();
                }
                if(!au.isPlaying)
                {
                    au.loop = false;
                    au.clip = charge_sound_final;
                    au.Play();
                }
            }
            else
            {
                if (au.clip == charge_sound)
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
        }
        else if (diff < 0f)
        {
            Shoot();
            ammo -= charge;
            charge = 0;
            au.Stop();
            au.loop = false;
            au.clip = start_sound;
            au.Play();
            

            Debug.Log("Shot");
        }
        Debug.DrawRay(pickable.getAimTransform().position, pickable.getAimTransform().forward * 10f, Color.yellow);
    }

    void Shoot()
    {
        //Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        //Vector3 shootAt = pickable.getAimTransform().forward * 100f;
        Rigidbody otherEntityRB = null; 
        RaycastHit hit;
        Physics.Raycast(pickable.getAimTransform().position, pickable.getAimTransform().forward, out hit);
        if (hit.collider != null)
        {
            //shootAt = hit.point;
            otherEntityRB = hit.transform.gameObject.GetComponentInParent<Rigidbody>();
            if (otherEntityRB == null)
            {
                otherEntityRB = hit.transform.GetComponent<Rigidbody>();
            }
        }
        Rigidbody temp = pickable.getPickerTransform().gameObject.GetComponentInParent<Rigidbody>();
        if (temp == null)
        {
            temp = pickable.getPickerTransform().gameObject.GetComponent<Rigidbody>();
        }
        if (temp != null)
        {
            Debug.Log("Exploded on " + temp.gameObject.ToString() + " vec: " + pickable.getAimTransform().forward);
            PlayerController tmp_cont = pickable.getPickerTransform().gameObject.GetComponent<PlayerController>();
            /*if(tmp_cont != null)
            {
                tmp_cont.setPickableInfluence(pickable.getAimTransform().forward * charge);
            }*/
            temp.AddForce(pickable.getAimTransform().forward  * charge * holderRecoilPercentage/100f * temp.mass * -1f);
        }
        if (otherEntityRB != null && otherEntityRB != temp)
        {
            Debug.Log("Shot on " + otherEntityRB.gameObject.ToString());
            //otherEntityRB.AddExplosionForce(charge, shootAt, radius);
            otherEntityRB.AddForce(pickable.getAimTransform().forward * charge * otherEntityRB.mass);
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
        if(ammo > 0f)
        {
            tmpLightNing = Instantiate(LightNing);
            tmpLightNing.GetComponent<LightNingScript>().stretch = hit.distance;
            tmpLightNing.transform.position = (transform.position + hit.point) / 2f;
            tmpLightNing.transform.LookAt(hit.point);
            pewau.PlayOneShot(shoot_sound);
        }
    }
}
