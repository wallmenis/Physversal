using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveThing : MonoBehaviour
{
    public ObjectiveBoardScript board;
    public List<ObjectiveThing> required;
    string text;
    bool isSatisfied;
    bool dontReveal;
    // Start is called before the first frame update
    void Start()
    {
        dontReveal = false;
        isSatisfied = false;
        text = "";
        if (!board.objectiveStrings.Contains(this))
        {
            Debug.Log("NotContaining, Adding");
            board.objectiveStrings.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ObjectiveThing thing in required)
        {
            if (thing.gameObject.activeSelf)
            {
                isSatisfied = false;
                dontReveal = true;
            }
            else
            {
                dontReveal = false;
            }
        }
        if(!isSatisfied)
        {
            if (!board.objectiveStrings.Contains(this))
            {
                Debug.Log("NotContaining, Adding");
                board.objectiveStrings.Add(this);
            }
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }

    public string getText()
    {
        return text;
    }

    public void setString(string inp)
    {
        text = inp;
    }

    public bool getIfSatisfied()
    {
        return isSatisfied;
    }

    public void setIfSatisfied(bool inp)
    {
        isSatisfied = inp;
    }

    public bool getIfToNotReveal()
    {
        return dontReveal;
    }
}
