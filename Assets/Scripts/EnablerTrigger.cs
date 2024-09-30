using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablerTrigger : MonoBehaviour
{
    public GameObject[] GameObjectsToEnable;
    public GameObject GameObjectToBeEnabledFrom;
    public bool toDisable;
    public bool disableAfter;
    GameObject triggeringGameObject;
    bool hasTriggered;
    // Start is called before the first frame update
    void Start()
    {
        triggeringGameObject = null;
        hasTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        triggeringGameObject = null;
        hasTriggered = false;
    }

    private void OnTriggerStay(Collider other)
    {
        triggeringGameObject = other.gameObject;
        //Debug.Log(GameObjectToBeEnabledFrom + " " + other.gameObject); // + " " + GameObjectToBeEnabledFrom.ToString() == other.gameObject.ToString());
        //if (GameObjectToBeEnabledFrom == other.gameObject || GameObjectToBeEnabledFrom == null)
        if (GameObjectToBeEnabledFrom == other.gameObject)
        {
            hasTriggered = true;
            foreach (GameObject go in GameObjectsToEnable)
            {
                go.SetActive(!toDisable);
            }
            //GameObjectToEnable.SetActive(!toDisable);
            if (disableAfter)
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    public bool getIfTriggered()
    {
        return hasTriggered;
    }

    public GameObject getTriggeringGameObject()
    {
        return triggeringGameObject;
    }
}
