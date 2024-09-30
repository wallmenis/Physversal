using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRespawnerScript : MonoBehaviour
{
    public GameObject toSpawn;
    public Transform pickableFather;
    public List<KillTrigg> killTriggs;
    public Transform spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KillTrigg killTrigg in killTriggs)
        {
            if (killTrigg!=null)
            {
                if (killTrigg.killedWeapons.Contains(toSpawn.transform.GetChild(0).name))
                {
                    killTrigg.killedWeapons.Remove(toSpawn.transform.GetChild(0).name);
                    GameObject newSpawn;
                    newSpawn = Instantiate(toSpawn);
                    newSpawn.transform.parent = pickableFather;
                    newSpawn.GetComponent<Pickable>().PickablesParent = pickableFather.gameObject;
                    newSpawn.transform.position = spawnLocation.position;
                }
            }
        }
        
    }
}
