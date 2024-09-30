using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public float speed;
    public float explosionForce;
    public float explosionRadius;
    Rigidbody rb;
    AudioSource au;
    public AudioClip explosion;
    public float damage;
    public GameObject visExplosion;
    Vector3 visibleExplosionSize;
    Color prevColor;
    Material prevMaterial;
    float expTimer;
    // Start is called before the first frame update
    void Start()
    {
        visibleExplosionSize = visExplosion.transform.localScale;
        visExplosion.SetActive(false);   
        au = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        //sc = GetComponentInChildren<SphereCollider>();
        transform.parent = null;
        prevColor = visExplosion.GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
        prevMaterial = visExplosion.GetComponent<MeshRenderer>().material;
        visExplosion.GetComponent<MeshRenderer>().material = new Material(prevMaterial);    //Disassociate with original material to not make any new changes
    }
    void FixedUpdate()
    {
        //Debug.Log(transform.position);
        rb.AddForce(transform.forward*speed, ForceMode.VelocityChange);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collided");
        if(!other.transform.CompareTag(transform.tag) && !other.isTrigger)
        {
            Debug.Log("OtherTag: " + other.transform.tag);
            if (rb.isKinematic != true)
            {
                expTimer = 0f;
                ExplosionDamage(transform.position, explosionRadius, damage, explosionForce);
                au.Stop();
                au.volume = 1f;
                au.PlayOneShot(explosion);
                visExplosion.SetActive(true);
            }
            expTimer += Time.deltaTime;
            rb.isKinematic = true; 
            //transform.GetChild(0).gameObject.GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);

            //https://stackoverflow.com/questions/36133356/play-and-wait-for-audio-to-finish-playing
            //while (au.isPlaying)
            //{
            //
            //}
            //while (au.time < explosion.length) ;

            visExplosion.transform.transform.localScale = Vector3.Lerp(visibleExplosionSize, explosionRadius*Vector3.one, (expTimer) / explosion.length);
            //visExplosion.transform.transform.localScale = explosionRadius * Vector3.one;

            visExplosion.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(prevColor.r, prevColor.g, prevColor.b, (1f - Mathf.Pow((expTimer) / explosion.length,3f))));
            visExplosion.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(prevColor.r, prevColor.g, prevColor.b, (1f - Mathf.Pow((expTimer) / explosion.length, 3f))));
            Destroy(transform.gameObject, explosion.length);
        }
    }

    void ExplosionDamage(Vector3 center, float radius, float damage, float force)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if(!hitCollider.isTrigger)
            {
                Debug.Log((hitCollider.gameObject.ToString() == GameObject.Find("Player").ToString()) + " " + hitCollider.transform.gameObject.name);
                Rigidbody temp = hitCollider.transform.gameObject.GetComponentInParent<Rigidbody>();
                if (hitCollider.transform.gameObject.layer == 9)
                {
                    Debug.Log("Collider 9" + temp);
                    //Debug.Log(temp != null);
                }
                if (temp == null)
                {
                    temp = hitCollider.transform.gameObject.GetComponent<Rigidbody>();
                }
                if (temp != null)
                {
                    Debug.Log("Exploded on " + temp.gameObject.ToString());
                    temp.AddExplosionForce(force, center, radius);
                }
            }
            //hitCollider.SendMessage("AddDamage");
        }
    }

    public void setTag(string tag)
    {
        transform.tag = tag;
        transform.GetChild(0).tag = tag;
    }
}
