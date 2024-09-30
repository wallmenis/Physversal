using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyXEnemies : MonoBehaviour
{
    public GameObject enemyBase;
    public List<KillTrigg> killTriggs;
    public int ammount;
    public string enemyName;
    int left;
    string text;
    ObjectiveThing thing;
    // Start is called before the first frame update
    void Start()
    {
        thing = GetComponent<ObjectiveThing>();
        left = 0;
        text = "Drop " + ammount + " " + enemyName + " (" + left + "/" + ammount + ")";
        thing.setString(text);
        thing.setIfSatisfied(false);
        //Debug.Log(board + " " + thing);
        //while (board == null) ;
        /*if (board!=null)
        {
            board.objectiveStrings.Add(thing);
            //board.appendObjective(transform);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!thing.getIfSatisfied() && !thing.getIfToNotReveal())
        {
            text = "Drop " + ammount + " " + enemyName + " (" + left + "/" + ammount + ")";
            thing.setString(text);
            foreach (KillTrigg killTrigg in killTriggs)
            {
                if (killTrigg != null)
                {
                    /*
                    if (killTrigg.killedEnemies.Contains(enemyBase.transform.GetChild(1).name))
                    {
                        List<string> ko = killTrigg.killedEnemies.FindAll(str => str.Contains(enemyBase.transform.GetChild(1).name));
                        left += ko.Count;
                        foreach (string k in ko)
                        {
                            killTrigg.killedEnemies.Remove(k);
                        }
                        
                        //board.updateObjective(id, text);
                    }*/
                    left += killTrigg.popAtomicEnemies(enemyBase.transform.GetChild(1).name);
                    if (left >= ammount)
                    {
                        thing.setIfSatisfied(true);
                        //board.removeObjective(id);
                    }
                }
            }
        }
        
    }
}
