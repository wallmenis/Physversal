using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBarrier : MonoBehaviour
{
    public List<ObjectiveThing> objects;
    bool isSatisfied;
    // Start is called before the first frame update
    void Start()
    {
        isSatisfied = false;
    }

    // Update is called once per frame
    void Update()
    {
        isSatisfied = true;
        foreach (ObjectiveThing obj in objects) 
        {
            if(obj.gameObject.activeSelf)
            {
                isSatisfied = false;
            }
        }
        if (objects.Count == 0)
        {
            isSatisfied = false;
        }
        if (isSatisfied) 
        {
            transform.gameObject.SetActive(false);
        }
    }
}
