using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosingBoxScript : MonoBehaviour
{
    public List<GameObject> enemiesToFreeze;
    public List<GameObject> gameobjectsToWaitForDeactivation;
    public List<GameObject> gameobjectsToWaitForActivation;
    public float startHeight;
    //float endHeight;
    Vector3 startVector;
    Vector3 endVector;
    float time;
    bool isSatisfied;
    bool toStart;
    bool hasRizen;

    // Start is called before the first frame update
    void Start()
    {
        hasRizen = false;
        isSatisfied = false;
        startVector = transform.position;
        startVector.y += startHeight;
        //endHeight = transform.position.y;
        endVector = transform.position;
        transform.position = startVector;
        time = 0f;
        foreach (GameObject go in enemiesToFreeze)
        {
            go.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isSatisfied = true;
        
        foreach (GameObject  obj in gameobjectsToWaitForDeactivation)
        {
            if (obj.activeSelf)
            {
                isSatisfied = false; 
                break;
            }
        }
        toStart = true;
        foreach (GameObject obj in gameobjectsToWaitForActivation)
        {
            if (obj.activeSelf)
            {
                toStart = false;
                break;
            }
        }
        if (toStart)
        {
            if(!hasRizen)
            {
                if (time < 1f)
                {
                    foreach (GameObject go in enemiesToFreeze)
                    {
                        go.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    time += Time.deltaTime;
                    transform.position = Vector3.Lerp(startVector, endVector, Mathf.Pow(time, 1 / 2f));
                }
                else
                {
                    hasRizen = true;
                }
            }
            else if(!isSatisfied)
            {
                foreach (GameObject go in enemiesToFreeze)
                {
                    if(go!=null)
                    {
                        if (go.GetComponent<Rigidbody>().isKinematic != false)
                        {
                            go.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                }
                time = 0f;
            }
            else
            {
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(endVector, startVector, Mathf.Pow(time, 1 / 2f));
                if (time > 1f)
                {
                    gameObject.SetActive(false);
                }
            }
            /*
            if (time < 1f && !isSatisfied)
            {
                foreach (GameObject go in enemiesToFreeze)
                {
                    go.GetComponent<Rigidbody>().isKinematic = true;
                }
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(startVector, endVector, Mathf.Pow(time, 1 / 2f));
            }
            else if (!isSatisfied)
            {
                
                foreach (GameObject go in enemiesToFreeze)
                {
                    go.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            else
            {
                isSatisfied = true;
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(endVector, startVector, Mathf.Pow(time, 1 / 2f));
                if (time > 1f)
                {
                    gameObject.SetActive(false);
                }
            }*/
        }
    }
}
