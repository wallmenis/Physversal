using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndMenuScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayTimer(int m, int s, int ms)
    {
        text.text = "took you " + m + "m" + s + "sec " + ms + "ms \n can you do better?";
    }
}
