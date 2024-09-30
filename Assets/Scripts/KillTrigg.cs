using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillTrigg : MonoBehaviour
{
    public GameObject player;
    public GameObject HUD;
    PauseRespawnMenu respMenu;
    public List<string> killedEnemies;
    public List<string> killedWeapons;
    bool enaccessed;
    // Start is called before the first frame update
    void Start()
    {
        enaccessed = false;
        killedEnemies = new List<string>();
        killedWeapons = new List<string>();
        //player = GameObject.Find("Player");
        if (HUD != null)
        {
            respMenu = HUD.GetComponent<PauseRespawnMenu>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider collision)
    {
        //DestroyImmediate(collision.transform.root.gameObject);
        //Debug.Log( "Killtrigg " + collision.gameObject); // + " " + GameObjectToBeEnabledFrom.ToString() == other.gameObject.ToString());
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log(collision.gameObject.transform.parent.gameObject);
            killedEnemies.Add(collision.gameObject.transform.parent.gameObject.transform.GetChild(1).name);
            //Debug.Log(collision.gameObject.transform.parent.gameObject.transform.GetChild(1).name);
            Debug.Log(killedEnemies.Count);
            Destroy(collision.gameObject.transform.parent.gameObject);
            //Destroy(collision.transform.parent.gameObject);
        }

        if(collision.gameObject.GetComponent<Pickable>() != null)
        {
            killedWeapons.Add(collision.gameObject.transform.GetChild(0).name);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<RocketScript>() != null)
        {
            Destroy(collision.gameObject);
        }

        if (player != null)
        {
            if (player == collision.gameObject)
            {
                respMenu.playerKilled();
            }
        }

    }

    public int popAtomicEnemies(string inp)
    {
        if (killedEnemies.Contains(inp) && !enaccessed)
        {
            enaccessed = true;
            int cnt;
            List<string> ko = killedEnemies.FindAll(str => str.Contains(inp));
            cnt = ko.Count;
            foreach (string k in ko)
            {
                killedEnemies.Remove(k);
            }
            enaccessed = false;
            return cnt;
            //board.updateObjective(id, text);
        }
        return 0;
    }
}
