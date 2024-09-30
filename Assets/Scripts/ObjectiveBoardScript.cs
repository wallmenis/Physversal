using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBoardScript : MonoBehaviour
{
    // Start is called before the first frame update

    public List<ObjectiveThing> objectiveStrings;
    public TextMeshProUGUI txt;
    Image img;
    void Start()
    {
        img = GetComponent<Image>();
        //objectiveStrings = new List<ObjectiveThing>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "";
        if (objectiveStrings.Count > 0) 
        {
            img.enabled = true;
            txt.text = "Objectives:\n";
            for (int i = 0; i< objectiveStrings.Count; i++) //(ObjectiveThing obj in objectiveStrings)
            {
                if (!objectiveStrings[i].gameObject.activeSelf)
                {
                    objectiveStrings.Remove(objectiveStrings[i]);
                }
                else
                {
                    
                    if (!objectiveStrings[i].getIfToNotReveal())
                    {
                        txt.text += " - " + objectiveStrings[i].getText() + "\n";
                    }
                }
                //Debug.Log(txt.text + " " + obj);
            }
        }
        else
        {
            img.enabled = false;
        }
        
    }

    /*public void appendObjective(Transform thing)
    {
        ObjectiveThing tg = thing.GetComponent<ObjectiveThing>();
        Debug.Log(tg);
        objectiveStrings.Add(tg);
    }
    /*
    public void updateObjective(int id, string text)
    {
        //https://www.tutorialspoint.com/how-to-find-the-index-of-an-item-in-a-chash-list-in-a-single-step
        //objectiveStrings.Find(ob => ob.id == id).text = text;
        updateText();
    }*/
}


