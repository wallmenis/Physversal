using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveEnablerScript : MonoBehaviour
{
    public List<GameObject> gameObjectsToEnable;
    public List<GameObject> gameObjectsToDisable;
    public List<ObjectiveThing> objectivesToAwaitFor;

    bool toenable;
    // Start is called before the first frame update
    void Start()
    {
        toenable = false;
    }

    // Update is called once per frame
    void Update()
    {
        toenable = true;
        foreach (ObjectiveThing thing in objectivesToAwaitFor)
        {
            if (thing.gameObject.activeSelf) 
            {
                toenable = false;
            }
        }
        if (toenable)
        {
            foreach (GameObject thing in gameObjectsToEnable)
            {
                thing.SetActive(true);
            }
            foreach (GameObject thing in gameObjectsToDisable)
            {
                thing.SetActive(false);
            }
            transform.gameObject.SetActive(false);
        }
    }
}
