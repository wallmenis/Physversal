using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightNingScript : MonoBehaviour
{
    public float stretch;
    public Vector3 Lookat;
    public float lifeTime;
    public Light[] lightList;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        //transform.LookAt(Lookat);
        //transform.localScale = new Vector3 (1f, stretch, 1f);
        transform.RotateAround(transform.position, transform.forward, Random.Range(0f,180f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(1f,  1f, stretch/2f);
        foreach (Light light in lightList)
        {
            light.intensity = stretch;
        }
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
