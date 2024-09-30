using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimerScript : MonoBehaviour
{
    float mins;
    float secs;
    float msecs;
    float stopper;
    // Start is called before the first frame update
    void Start()
    {
        stopper = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        secs += Time.deltaTime * stopper;
        msecs +=  (Time.deltaTime * 1000f) * stopper;
        //mins += secs / 60f * stopper;
        if (msecs >= 1000f)
        {
            msecs = 0;
        }
        if (secs >= 60)
        {
            mins += 1;
            secs = 0;
        }
        Debug.Log(mins + " " + secs + " " + msecs);
    }

    public int getMinutes()
    {
        return Mathf.FloorToInt(mins);
    }
    public int getSeconds()
    {
        return Mathf.FloorToInt(secs);
    }
    public int getMilliseconds()
    {
        return Mathf.FloorToInt(msecs);
    }
    public void startTimer()
    {
        stopper = 1f;
    }

    public void stopTimer()
    {
        stopper = 0f;
    }
    public void resetTimer()
    {
        mins = 0; secs = 0; msecs = 0;
    }


}
