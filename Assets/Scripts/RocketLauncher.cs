using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.FilePathAttribute;

public class RocketLauncher : MonoBehaviour
{
    Pickable pickable;
    public GameObject Rocket;
    public int ammoLeft;
    public float fireRate;
    float fireCounter;
    public TextMeshPro ammoText;
    GameObject curRocket;

    bool shot;

    // Start is called before the first frame update
    void Start()
    {
        //au = GetComponent<AudioSource>();
        fireCounter = 0f;
        shot = false;
        pickable = GetComponent<Pickable>();
        ammoText.text = "" + ammoLeft;
        if (fireRate <= 0f)
        {
            fireRate = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(pickable.getAimTransform().position, pickable.getAimTransform().TransformDirection(pickable.getAimTransform().forward) * 10f, Color.yellow);
        if (pickable.getPickerTransform() == null || pickable.isHidden()) 
        {
            ammoText.text = "";
        }
        else
        {
            ammoText.text = "" + ammoLeft;
        }
        if (pickable.getPrimaryFire()>0f && !shot && ammoLeft > 0 && fireCounter <= 0f)
        {
            shot = true;
            ammoLeft--;
            fireCounter = 60f/fireRate; //Rockets a minute  
            Shoot();
            //ammoText.text = "" + ammoLeft;
        }
        if (pickable.getPrimaryFire()<=0f)
        {
            shot = false;
            fireCounter -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        
        
        Vector3 spawnPos = transform.position + transform.forward;
        //Vector3 spawnPos = pickable.getAimTransform().position + pickable.getAimTransform().forward;
        Quaternion spawnRotation = pickable.getAimTransform().rotation;
        curRocket = Instantiate(Rocket,spawnPos, spawnRotation, null);
        RaycastHit hit;
        Physics.Raycast(pickable.getAimTransform().position + pickable.getAimTransform().forward, pickable.getAimTransform().forward, out hit);
        if (hit.collider != null)
        {
            curRocket.transform.LookAt(hit.point,Vector3.up);
        }
        curRocket.GetComponent<RocketScript>().setTag(pickable.getPickerTransform().tag);
        Debug.Log("Tag: " + curRocket.tag);
    }
}
